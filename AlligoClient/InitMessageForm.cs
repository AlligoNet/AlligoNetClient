using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlligoClient
{
    public partial class InitMessageForm : Form
    {
        Timer timer;
        Form1 clientForm;

        public InitMessageForm()
        {
            InitializeComponent();
        }

        private void InitMessage_Shown(object sender, EventArgs e)
        {
            timer = new Timer();
            timer.Interval = 12000;
            timer.Start();
            timer.Tick += timer_finished;

            //Start Client Init
            clientForm = new Form1();
        }

        private void timer_finished(object sender, EventArgs e)
        {
            timer.Stop();

            try
            {
                clientForm.Show();
            }
            catch (Exception error)
            {
                Application.Exit();
            }


            this.Hide();
        }
    }
}
