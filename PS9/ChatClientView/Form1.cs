using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CC;

namespace BoggleClientView
{
    public partial class Form1 : Form
    {
        BoggleClientModel model;

        public Form1()
        {
            InitializeComponent();
            model = new BoggleClientModel();
            model.IncomingLineEvent += MessageReceived;
            model.IncomingTimeEvent += TimeReceived;
        }

        private void connect_clicked(object sender, EventArgs e)
        {
            //model.Connect("localhost", 2000, textBox_connect.Text);
         
        }

        private void go_clicked(object sender, EventArgs e)
        {
            model.SendGoMessage(textBox_go.Text);
        }


        private void MessageReceived(String line)
        {
            textBox_main.Invoke(new Action(() => { textBox_main.Text += line + "\r\n"; }));
        }
        private void TimeReceived(String line)
        {
            textBox_time.Invoke(new Action(() => { textBox_time.Text = line + "\r\n"; }));
        }

        protected override void OnClosed(EventArgs e)
        {
            model.disconnect();
            base.OnClosed(e);
        }
    }
}
