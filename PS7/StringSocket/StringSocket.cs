// Authors: James Yeates and Tyler Down
// CS 3500, November 2014

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

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

        /// <summary>
        /// The underlying socket that this StringSocket uses to send and receive bytes of data.
        /// </summary>
        private Socket socket;

        /// <summary>
        /// The character encoding to use to encode or decode data to send or receive through 
        /// the underlying socket.
        /// </summary>
        private Encoding encoding;

        /// <summary>
        /// Text that needs to be sent to the client but has not yet gone.
        /// </summary>
        private string outgoing;

        /// <summary>
        /// Text that has been received from the client but not yet been dealt with.
        /// </summary>
        private String incoming;

        /// <summary>
        /// Used by the BeginReceive private helper methods to hold the bytes
        /// that were received by the underlying socket.
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// Holds pending send requests.
        /// </summary>
        private Queue<SendRequest> SendQueue;

        /// <summary>
        /// Holds pending receive requests.
        /// </summary>
        private Queue<ReceiveRequest> ReceiveQueue;

        private Queue<string> ReceivedMsg;


        /// <summary>
        /// Creates a StringSocket from a regular Socket, which should already be connected.  
        /// The read and write methods of the regular Socket must not be called after the
        /// LineSocket is created.  Otherwise, the StringSocket will not behave properly.  
        /// The encoding to use to convert between raw bytes and strings is also provided.
        /// </summary>
        public StringSocket(Socket s, Encoding e)
        {
            // Initialize member variables.
            socket = s;
            encoding = e;
            outgoing = "";
            incoming = "";
            buffer = new byte[1024];
            SendQueue = new Queue<SendRequest>();
            ReceiveQueue = new Queue<ReceiveRequest>();
            ReceivedMsg = new Queue<string>();
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
            // Protect SendQueue
            lock (SendQueue)
            {
                // Create and store the send request.
                SendRequest sendRequest = new SendRequest(s, callback, payload);
                SendQueue.Enqueue(sendRequest);

                // If there's not a send ongoing, start one.
                if (SendQueue.Count() == 1)
                {
                    // Append the message to the unsent string
                    outgoing += s;

                    SendBytes();
                }

            }
        }

        /// <summary>
        /// Checks if the entire message has been sent. If the message hasn't been sent
        /// or there is another request, begin sending it.
        /// </summary>
        private void SendBytes()
        {
            if (SendQueue.Count > 0)
            {
                // If the entire message has been sent.
                if (outgoing == "")
                {
                    // Dequeue the send request.
                    SendRequest sr = SendQueue.Dequeue();

                    // Call the appropriate callback.
                    ThreadPool.QueueUserWorkItem(x => sr.sendCallback(null, sr.payload));

                    // If there's another request in the queue, get its message and begin sending it.
                    if (SendQueue.Count > 0)
                    {
                        outgoing = SendQueue.Peek().message;

                        byte[] outgoingBuffer = encoding.GetBytes(outgoing);
                        outgoing = "";
                        try
                        {
                            socket.BeginSend(outgoingBuffer, 0, outgoingBuffer.Length,
                                             SocketFlags.None, MessageSent, outgoingBuffer);
                        }
                        catch(Exception e)
                        {
                            SendRequest sr1 = SendQueue.Dequeue();
                            ThreadPool.QueueUserWorkItem(x => sr1.sendCallback(e, sr1.payload));
                        }
                    }
                }
                // Otherwise, the entire message has not been sent. Send more.
                else
                {
                    byte[] outgoingBuffer = encoding.GetBytes(outgoing);
                    outgoing = "";
                    try
                    {
                        socket.BeginSend(outgoingBuffer, 0, outgoingBuffer.Length,
                                         SocketFlags.None, MessageSent, outgoingBuffer);
                    }
                    catch (Exception e)
                    {
                        SendRequest sr2 = SendQueue.Dequeue();
                        ThreadPool.QueueUserWorkItem(x => sr2.sendCallback(e, sr2.payload));
                    }
                   
                }
            }
        }

        /// <summary>
        /// Called when a message has been successfully sent.
        /// 
        /// Decodes the unsent bytes and tries sending them again.
        /// </summary>
        private void MessageSent(IAsyncResult result)
        {

            // Protect SendQueue
            lock (SendQueue)
            {
                try
                {
                    // Find out how many bytes were actually sent
                    int bytes = socket.EndSend(result);

                    // Get the bytes that we attempted to send
                    byte[] outgoingBuffer = (byte[])result.AsyncState;

                    // If no bytes were received, close the socket.
                    //if (bytes == 0)
                    //{
                    //    //what to do here???
                    //    socket.Close();
                    //}

                    // Prepend the unsent bytes and try sending again.
                    //else
                    {
                        outgoing = encoding.GetString(outgoingBuffer, bytes,
                                                      outgoingBuffer.Length - bytes) + outgoing;
                        SendBytes();
                    }
                }
                catch(Exception e)
                {
                    SendRequest sr2 = SendQueue.Dequeue();
                    ThreadPool.QueueUserWorkItem(x => sr2.sendCallback(e, sr2.payload));

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
            // Protect the ReceiveQueue
            lock (ReceiveQueue)
            {
                // Create and store the receive request.
                ReceiveRequest receiveRequest = new ReceiveRequest(callback, payload);
                ReceiveQueue.Enqueue(receiveRequest);

                // If there is no receive ongoing, start receiving.
                if (ReceiveQueue.Count == 1)
                {
                    ProcessReceivedMessage();
                }
            }
        }

        /// <summary>
        /// Processes a received message by looking for a newline character to satisfy a receive 
        /// request. If no newlines are found and there are requests, asks for more data.
        /// </summary>
        private void ProcessReceivedMessage()
        {
            // Look for strings ending in a newline.
         
            while ((ReceivedMsg.Count() > 0 && ReceiveQueue.Count > 0))
            {
                string line = ReceivedMsg.Dequeue();
                ReceiveRequest rr = ReceiveQueue.Dequeue();//wrong? when do we do this?
                ThreadPool.QueueUserWorkItem(x => rr.receiveCallback(line, null, rr.payload));
            }
       
            ///////////////////////////////////////////////////////////////////

            // If there is no newline and you have a request, call BeginReceive.
            while (ReceiveQueue.Count > 0)
            {
                try{
                // Ask for some more data
                socket.BeginReceive(buffer, 0, buffer.Length,
                    SocketFlags.None, MessageReceived, buffer);
                    //not sure why Matt suggetest this break
                break;
                }
                catch(Exception e)
                {
                     ReceiveRequest rr = ReceiveQueue.Dequeue();//wrong? when do we do this?
                     ThreadPool.QueueUserWorkItem(x => rr.receiveCallback(null, e, rr.payload));
                     incoming = "";

                }
            }
        }

        /// <summary>
        /// Called when some data has been received.
        /// 
        /// Gets the bytes that were received, stores them in a string, and
        /// calls a method to process the received message.
        /// </summary>
        private void MessageReceived(IAsyncResult result)
        {
            // Protect the ReceiveQueue.
            lock (ReceiveQueue)
            {
                try
                {
                    // Get the buffer to which the data was written.
                    buffer = (byte[])(result.AsyncState);

                    // Figure out how many bytes have come in.
                    int bytes = socket.EndReceive(result);

                    // If no bytes were received, close the socket.
                    if (bytes == 0)
                    {

                        //socket.Close(); 
                        // Enqueue null message
                        ReceivedMsg.Enqueue(null);
                        // Call ProcessReceive
                        ProcessReceivedMessage();
                    }

                    // Otherwise, decode the incoming bytes, then process the message.
                    else
                    {
                        // Decode the bytes, and append them to the 'incoming' string member variable.
                        incoming += encoding.GetString(buffer, 0, bytes);

                        // Extract messages ending with newlines and enqueue them

                        //////////////////////////////////////////////////////////////////

                        // Look for strings ending in a newline.
                        int index;
                        while (((index = incoming.IndexOf('\n')) >= 0))
                        {
                            // Get the first string ending in a newline.
                            String line = incoming.Substring(0, index);
                            ReceivedMsg.Enqueue(line);
                            
                            incoming = incoming.Substring(index + 1);

                         
                        }
                        ///////////////////////////////////////////////////////////////////////////////

                        ProcessReceivedMessage();
                    }
                }
                    //Error occured
                catch (Exception e)
                {
                    ReceiveRequest rr = ReceiveQueue.Dequeue();
                    ThreadPool.QueueUserWorkItem(x => rr.receiveCallback(null, e, rr.payload));
                    ProcessReceivedMessage();
                    incoming = "";
                }
            }
        }

        public bool IsConnected() { return socket.Connected; }

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
            // Wait for all pending data to be sent.
            while (SendQueue.Count > 0)
                Thread.Sleep(50);

            // Shutdown and close the socket.
            if (socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            socket.Close();
            
        }

        /// <summary>
        /// Struct used to store a single send request.
        /// Each send request has a message, a SendCallback, and a payload.
        /// </summary>
        private struct SendRequest
        {
            public String message;
            public SendCallback sendCallback;
            public Object payload;

            /// <summary>
            /// Creates a SendRequest with the given message, SendCallback, and payload.
            /// </summary>
            public SendRequest(String m, SendCallback cb, Object o)
            {
                message = m;
                sendCallback = cb;
                payload = o;
            }
        }

        /// <summary>
        /// Struct used to store a single receive request.
        /// Each receive request has a ReceiveCallback and a payload.
        /// </summary>
        private struct ReceiveRequest
        {

            public ReceiveCallback receiveCallback;
            public Object payload;

            /// <summary>
            /// Creates a SendRequest with the given ReceiveCallback and payload.
            /// </summary>
            public ReceiveRequest(ReceiveCallback cb, Object o)
            {
                receiveCallback = cb;
                payload = o;
            }
        }

    }
}




