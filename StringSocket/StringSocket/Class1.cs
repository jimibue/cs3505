using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace CustomNetworking
{
    /// <summary>
    /// A StringSocket is a wrapper around a Socket.  It provides methods that
    /// asynchronously read lines of text (strings terminated by newlines) and 
    /// write strings. (As opposed to Sockets, which read and write raw bytes.)  
    ///
    /// StringSockets are thread safe.  This means that two or more threads may
    /// invoke methods on a shared StringSocket without restriction.  The
    /// StringSocket takes care of the synchonization.
    /// 
    /// Each StringSocket contains a Socket object that is provided by the client.  
    /// A StringSocket will work properly only if the client refrains from calling
    /// the contained Socket's read and write methods.
    /// 
    /// If we have an open Socket s, we can create a StringSocket by doing
    /// 
    ///    StringSocket ss = new StringSocket(s, new UTF8Encoding());
    /// 
    /// We can write a string to the StringSocket by doing
    /// 
    ///    ss.BeginSend("Hello world", callback, payload);
    ///    
    /// where callback is a SendCallback (see below) and payload is an arbitrary object.
    /// This is a non-blocking, asynchronous operation.  When the StringSocket has 
    /// successfully written the string to the underlying Socket, or failed in the 
    /// attempt, it invokes the callback.  The parameters to the callback are a
    /// (possibly null) Exception and the payload.  If the Exception is non-null, it is
    /// the Exception that caused the send attempt to fail.
    /// 
    /// We can read a string from the StringSocket by doing
    /// 
    ///     ss.BeginReceive(callback, payload)
    ///     
    /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
    /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
    /// string of text terminated by a newline character from the underlying Socket, or
    /// failed in the attempt, it invokes the callback.  The parameters to the callback are
    /// a (possibly null) string, a (possibly null) Exception, and the payload.  Either the
    /// string or the Exception will be non-null, but nor both.  If the string is non-null, 
    /// it is the requested string (with the newline removed).  If the Exception is non-null, 
    /// it is the Exception that caused the send attempt to fail.
    /// </summary>

    public class StringSocket
    {
        // These delegates describe the callbacks that are used for sending and receiving strings.
        public delegate void SendCallback(Exception e, object payload);
        public delegate void ReceiveCallback(String s, Exception e, object payload);

        private Socket socket;
        private Encoding encoding;

        /// <summary>
        /// Text that has been received from the client but not yet dealt with
        /// </summary>
        private String incoming;

        /// <summary>
        /// Text that needs to be sent to the client but has not yet gone
        /// </summary>
        private string outgoing;

        /// <summary>
        /// Records whether an asynchronous send attempt is ongoing
        /// </summary>
        private bool sendIsOngoing = false;

        /// <summary>
        /// For synchronizing sends
        /// </summary>
        private readonly object sendSync = new object();

        /// <summary>
        /// Holds send requests
        /// </summary>
        private Queue<SendRequest> SendQueue;

        /// <summary>
        /// Holds receive requests
        /// </summary>
        private Queue<ReceiveRequest> ReceiveQueue;

        //private SendCallback sendCallback;
        //private ReceiveCallback receiveCallback;
        //private Object payload;

        /// <summary>
        /// Creates a StringSocket from a regular Socket, which should already be connected.  
        /// The read and write methods of the regular Socket must not be called after the
        /// LineSocket is created.  Otherwise, the StringSocket will not behave properly.  
        /// The encoding to use to convert between raw bytes and strings is also provided.
        /// </summary>
        public StringSocket(Socket s, Encoding e)
        {
            socket = s;
            encoding = e;
            incoming = "";
            outgoing = "";
            SendQueue = new Queue<SendRequest>();
            ReceiveQueue = new Queue<ReceiveRequest>();
        }

        /// <summary>
        /// We can write a string to a StringSocket ss by doing
        /// 
        ///    ss.BeginSend("Hello world", callback, payload);
        ///    
        /// where callback is a SendCallback (see below) and payload is an arbitrary object.
        /// This is a non-blocking, asynchronous operation.  When the StringSocket has 
        /// successfully written the string to the underlying Socket, or failed in the 
        /// attempt, it invokes the callback.  The parameters to the callback are a
        /// (possibly null) Exception and the payload.  If the Exception is non-null, it is
        /// the Exception that caused the send attempt to fail. 
        /// 
        /// This method is non-blocking.  This means that it does not wait until the string
        /// has been sent before returning.  Instead, it arranges for the string to be sent
        /// and then returns.  When the send is completed (at some time in the future), the
        /// callback is called on another thread.
        /// 
        /// This method is thread safe.  This means that multiple threads can call BeginSend
        /// on a shared socket without worrying around synchronization.  The implementation of
        /// BeginSend must take care of synchronization instead.  On a given StringSocket, each
        /// string arriving via a BeginSend method call must be sent (in its entirety) before
        /// a later arriving string can be sent.
        /// </summary>
        public void BeginSend(String s, SendCallback callback, object payload)
        {

            SendRequest sendRequest = new SendRequest(s, callback, payload);
            SendQueue.Enqueue(sendRequest);

            SendMessage(SendQueue.Peek());
            SendRequest currnetSR = SendQueue.Dequeue();
            lock (sendSync)
            {
                ThreadPool.QueueUserWorkItem(x => currnetSR.sendCallBack(null, currnetSR.payload));
            }
        }

        /// <summary>
        /// Sends a string to the client
        /// </summary>
        private void SendMessage(SendRequest sr)
        {

            // Get exclusive access to send mechanism
            lock (sendSync)
            {
                String message = sr.message;
                // Append the message to the unsent string
                outgoing += message;

                // If there's not a send ongoing, start one.
                if (!sendIsOngoing)
                {
                    sendIsOngoing = true;
                    SendBytes();
                }
            }
        }

        /// <summary>
        /// Attempts to send the entire outgoing string.
        /// </summary>
        private void SendBytes()
        {
            if (outgoing == "")
            {
                sendIsOngoing = false;
                //End here?
            }
            else
            {
                byte[] outgoingBuffer = encoding.GetBytes(outgoing);
                outgoing = "";
                socket.BeginSend(outgoingBuffer, 0, outgoingBuffer.Length,
                                 SocketFlags.None, MessageSent, outgoingBuffer);
            }
        }

        /// <summary>
        /// Called when a message has been successfully sent
        /// </summary>
        private void MessageSent(IAsyncResult result)
        {
            // Find out how many bytes were actually sent
            int bytes = socket.EndSend(result);



            // Get exclusive access to send mechanism
            lock (sendSync)
            {
                // Get the bytes that we attempted to send
                byte[] outgoingBuffer = (byte[])result.AsyncState;

                // The socket has been closed
                if (bytes == 0)
                {

                    socket.Close();
                    //Console.WriteLine("Socket closed");
                }

                // Prepend the unsent bytes and try sending again.
                else
                {
                    outgoing = encoding.GetString(outgoingBuffer, bytes,
                                                  outgoingBuffer.Length - bytes) + outgoing;
                    SendBytes();
                }
            }
        }

        /// <summary>
        /// We can read a string from the StringSocket by doing
        /// 
        ///     ss.BeginReceive(callback, payload)
        /// 
        /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
        /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
        /// string of text terminated by a newline character from the underlying Socket, or
        /// failed in the attempt, it invokes the callback.  The parameters to the callback are
        /// a (possibly null) string, a (possibly null) Exception, and the payload.  Either the
        /// string or the Exception will be non-null, but nor both.  If the string is non-null, 
        /// it is the requested string (with the newline removed).  If the Exception is non-null, 
        /// it is the Exception that caused the send attempt to fail.
        /// 
        /// This method is non-blocking.  This means that it does not wait until a line of text
        /// has been received before returning.  Instead, it arranges for a line to be received
        /// and then returns.  When the line is actually received (at some time in the future), the
        /// callback is called on another thread.
        /// 
        /// This method is thread safe.  This means that multiple threads can call BeginReceive
        /// on a shared socket without worrying around synchronization.  The implementation of
        /// BeginReceive must take care of synchronization instead.  On a given StringSocket, each
        /// arriving line of text must be passed to callbacks in the order in which the corresponding
        /// BeginReceive call arrived.
        /// 
        /// Note that it is possible for there to be incoming bytes arriving at the underlying Socket
        /// even when there are no pending callbacks.  StringSocket implementations should refrain
        /// from buffering an unbounded number of incoming bytes beyond what is required to service
        /// the pending callbacks.        
        /// 
        /// <param name="callback"> The function to call upon receiving the data</param>
        /// <param name="payload"> 
        /// The payload is "remembered" so that when the callback is invoked, it can be associated
        /// with a specific Begin Receiver....
        /// </param>  
        /// 
        /// <example>
        ///   Here is how you might use this code:
        ///   <code>
        ///                    client = new TcpClient("localhost", port);
        ///                    Socket       clientSocket = client.Client;
        ///                    StringSocket receiveSocket = new StringSocket(clientSocket, new UTF8Encoding());
        ///                    receiveSocket.BeginReceive(CompletedReceive1, 1);
        /// 
        ///   </code>
        /// </example>
        /// </summary>
        public void BeginReceive(ReceiveCallback callback, object payload)
        {
            // Store the receive request, with its specified callback and payload.
            ReceiveRequest receiveRequest = new ReceiveRequest(callback, payload);
            ReceiveQueue.Enqueue(receiveRequest);

            // Ask the socket to call MessageReceive as soon as up to 1024 bytes arrive.
            byte[] buffer = new byte[1024];//this should work when this is set to a small number
            socket.BeginReceive(buffer, 0, buffer.Length,
                                SocketFlags.None, MessageReceived, buffer);
        }

        /// <summary>
        /// Called when some data has been received.
        /// </summary>
        private void MessageReceived(IAsyncResult result)
        {

            lock (sendSync)
            {
                // if  not processing
                //Dequeue

                // Get the buffer to which the data was written.
                byte[] buffer = (byte[])(result.AsyncState);

                // Figure out how many bytes have come in
                int bytes = socket.EndReceive(result);

                // simulate race condition
                //if ((int)ReceiveQueue.Peek().payload == 1)
                //{
                //    Thread.Sleep(1000);
                //}

                // If no bytes were received, it means the client closed its side of the socket.
                // Report that to the console and close our socket.
                if (bytes == 0)
                {

                    Console.WriteLine("Socket closed");
                    socket.Close();
                }
                // Otherwise, decode and display the incoming bytes.  Then request more bytes.
                else
                {
                    // Convert the bytes into a string
                    incoming += encoding.GetString(buffer, 0, bytes);
                    Console.WriteLine(incoming);

                    // Echo any complete lines, converted to upper case
                    int index;
                    while ((index = incoming.IndexOf('\n')) >= 0)
                    {
                        String line = incoming.Substring(0, index);
                        if (line.EndsWith("\r"))
                        {
                            line = line.Substring(0, index - 1);
                        }
                        incoming = incoming.Substring(index + 1);

                        // Call the appropriate callback.
                        ReceiveQueue.Peek().receiveCallBack(line, null, ReceiveQueue.Peek().payload);
                        ReceiveQueue.Dequeue();//wrong
                    }
                    //ReceiveQueue.Dequeue();
                    // Ask for some more data
                    socket.BeginReceive(buffer, 0, buffer.Length,
                        SocketFlags.None, MessageReceived, buffer);

                }
            }
        }

        /// <summary>
        /// Calling the close method will close the String Socket (and the underlying
        /// standard socket).  The close method  should make sure all 
        ///
        /// Note: ideally the close method should make sure all pending data is sent
        ///       
        /// Note: closing the socket should discard any remaining messages and       
        ///       disable receiving new messages
        /// 
        /// Note: Make sure to shutdown the socket before closing it.
        ///
        /// Note: the socket should not be used after closing.
        /// </summary>
        public void Close()
        {

        }

        /// <summary>
        /// Struct used to stores a single send request
        /// </summary>
        private struct SendRequest
        {
            public String message;
            public SendCallback sendCallBack;
            public Object payload;
            public SendRequest(String m, SendCallback cb, Object o)
            {
                message = m;
                sendCallBack = cb;
                payload = o;
            }
        }
        /// <summary>
        /// Struct used to stores a single receive request
        /// </summary>
        private struct ReceiveRequest
        {

            public ReceiveCallback receiveCallBack;
            public Object payload;
            public ReceiveRequest(ReceiveCallback cb, Object o)
            {
                receiveCallBack = cb;
                payload = o;
            }
        }
    }
}
