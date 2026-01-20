using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sharp7;

namespace Omnicheck360
{

    public partial class OmniMsgOriginal : Form
    {
        private string sMsg = "No Message Passed!";
        private int TypeSelection = 0;

        public OmniMsgOriginal(string s, int type)
        {
            InitializeComponent();
            ButtonAnimator.InitializeAnimation(btnNo, "blue");
            ButtonAnimator.InitializeAnimation(btnYes, "blue");


            sMsg = s;
            TypeSelection = type;
        }

        private void OmniMsgOriginal_Load(object sender, EventArgs e)
        {
            lblMsg.Text = sMsg;
            if (TypeSelection == 0)
            {
                btnOK.Visible = false;
                btnYes.Visible = false;
                btnNo.Visible = false;
            }
            else if (TypeSelection == 1)
            {
                btnOK.Visible = false;
                btnYes.Visible = true;
                btnNo.Visible = true;
            }
            else
            {
                btnOK.Visible = true;
                btnYes.Visible = false;
                btnNo.Visible = false;
            }
        }

        private void MessageDone()
        {
            Close();
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            MainForm.received_answer_value = 1;
            MainForm.Received_Answer = true;
            MessageDone();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            MainForm.received_answer_value = 0;
            MainForm.Received_Answer = true;
            MessageDone();
        }
    }
}
