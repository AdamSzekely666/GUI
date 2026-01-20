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

    public partial class OmniMsg : Form
    {
        private string sMsg = "No Message Passed!";
        private int TypeSelection = 0;
        public int nAnswer = 0;
        private bool Servo_Done = false;
        private int Address = 0;
        private int Bit = 0;


        public static byte[] DB_ServoDone_Read_Buffer = new byte[1];

        public OmniMsg(string s, int type, byte[] done, int _address, int _bit)
        {
            InitializeComponent();
            ButtonAnimator.InitializeAnimation(btnNo, "blue");
            ButtonAnimator.InitializeAnimation(btnYes, "blue");

            sMsg = s;
            TypeSelection = type;

            DB_ServoDone_Read_Buffer = done;
            Address = _address;
            Bit = _bit;
        }

        private void OmniMsg_Load(object sender, EventArgs e)
        {
            lblMsg.Text = sMsg;
            if (TypeSelection == 0)
            {
                btnOK.Visible = false;
                btnYes.Visible = false;
                btnNo.Visible = false;
                Servo_Done = false;
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
            MainForm.Client.DBRead(MainForm.DataBlock, Address, 1, DB_ServoDone_Read_Buffer);
            Servo_Done = S7.GetBitAt(DB_ServoDone_Read_Buffer, 0, Bit);

            if (Servo_Done == true)
            {
                MessageDone();
            }
        }
    }
}
