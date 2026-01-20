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

    public partial class OmniMsgMoveAll : Form
    {
        private string sMsg = "No Message Passed!";
        private int TypeSelection = 0;
        public int nAnswer = 0;
        private bool Servo1_Done = false;
        private bool Servo2_Done = false;
        private bool Servo3_Done = false;

        private byte[] DB_ServoDone_Read_Buffer = new byte[2];

        public OmniMsgMoveAll(string s, int type, byte[] done)
        {
            InitializeComponent();
            ButtonAnimator.InitializeAnimation(btnNo, "blue");
            ButtonAnimator.InitializeAnimation(btnYes, "blue");


            sMsg = s;
            TypeSelection = type;

            DB_ServoDone_Read_Buffer = done;
        }

        private void OmniMsgMoveAll_Load(object sender, EventArgs e)
        {
            lblMsg.Text = sMsg;
            if (TypeSelection == 0)
            {
                btnOK.Visible = false;
                btnYes.Visible = false;
                btnNo.Visible = false;
                if (MainForm.Servo1Enable == 1)
                {
                    Servo1_Done = false;
                }
                else
                {
                    Servo1_Done = true;
                }

                if (MainForm.Servo2Enable == 1)
                {
                    Servo2_Done = false;
                }
                else
                {
                    Servo2_Done = true;
                }

                if (MainForm.Servo3Enable == 1)
                {
                    Servo3_Done = false;
                }
                else
                {
                    Servo3_Done = true;
                }

                ServoChangeDone.Start();
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
            nAnswer = 1;
            MessageDone();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            nAnswer = 0;
            MessageDone();
        }

        private void ServoChangeDone_Tick(object sender, EventArgs e)
        {
            MainForm.Client.DBRead(MainForm.DataBlock, 988, 2, DB_ServoDone_Read_Buffer);
            Servo1_Done = S7.GetBitAt(DB_ServoDone_Read_Buffer, 0, 6);
            Servo2_Done = S7.GetBitAt(DB_ServoDone_Read_Buffer, 0, 7);
            Servo3_Done = S7.GetBitAt(DB_ServoDone_Read_Buffer, 1, 0);

            if (Servo1_Done == true && Servo2_Done == true && Servo3_Done == true)
            {
                MessageDone();
            }
        }
    }
}
