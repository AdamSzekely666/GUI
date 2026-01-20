using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omnicheck360
{
    public partial class PopUpKeyPad : Form
    {

        public static string oPassedField;
        SplashScreen splash;

        public PopUpKeyPad(SplashScreen _splash)
        {
            InitializeComponent();
            splash = _splash;
        }

        private void PopUpKeyPad_Load(object sender, EventArgs e)
        {
            txtValue.Text = oPassedField;
            this.TopMost = true;
        }

        private void AddToOutput(string character)
        {
                txtValue.Text = txtValue.Text + character;
        }

        private void btn_Enter_Click(object sender, EventArgs e)
        {
            //string temp = txtValue.Text;
            //MessageBox.Show(temp);
            splash.OmnifissionPassword = txtValue.Text;
            this.Close();
        }

        private void btn_backspace_Click(object sender, EventArgs e)
        {
            string CurrentString = txtValue.Text;
            int StringLength = txtValue.TextLength;

            if (StringLength > 0)
            {
                txtValue.Text = CurrentString.Substring(0, StringLength - 1);
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txtValue.Text = "";
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        }

        private void btn_0_Click(object sender, EventArgs e)
        {
            AddToOutput("0");
        }

        private void btn_1_Click(object sender, EventArgs e)
        {
            AddToOutput("1");
        }

        private void btn_2_Click(object sender, EventArgs e)
        {
            AddToOutput("2");
        }

        private void btn_3_Click(object sender, EventArgs e)
        {
            AddToOutput("3");
        }

        private void btn_4_Click(object sender, EventArgs e)
        {
            AddToOutput("4");
        }

        private void btn_5_Click(object sender, EventArgs e)
        {
            AddToOutput("5");
        }

        private void btn_6_Click(object sender, EventArgs e)
        {
            AddToOutput("6");
        }

        private void btn_7_Click(object sender, EventArgs e)
        {
            AddToOutput("7");
        }

        private void btn_8_Click(object sender, EventArgs e)
        {
            AddToOutput("8");
        }

        private void btn_9_Click(object sender, EventArgs e)
        {
            AddToOutput("9");
        }

        private void btn_q_Click(object sender, EventArgs e)
        {
            AddToOutput("Q");
        }

        private void btn_w_Click(object sender, EventArgs e)
        {
            AddToOutput("W");
        }

        private void btn_e_Click(object sender, EventArgs e)
        {
            AddToOutput("E");
        }

        private void btn_r_Click(object sender, EventArgs e)
        {
            AddToOutput("R");
        }

        private void btn_t_Click(object sender, EventArgs e)
        {
            AddToOutput("T");
        }

        private void btn_y_Click(object sender, EventArgs e)
        {
            AddToOutput("Y");
        }

        private void btn_u_Click(object sender, EventArgs e)
        {
            AddToOutput("U");
        }

        private void btn_i_Click(object sender, EventArgs e)
        {
            AddToOutput("I");
        }

        private void btn_o_Click(object sender, EventArgs e)
        {
            AddToOutput("O");
        }

        private void btn_p_Click(object sender, EventArgs e)
        {
            AddToOutput("P");
        }

        private void btn_a_Click(object sender, EventArgs e)
        {
            AddToOutput("A");
        }

        private void btn_s_Click(object sender, EventArgs e)
        {
            AddToOutput("S");
        }

        private void btn_d_Click(object sender, EventArgs e)
        {
            AddToOutput("D");
        }

        private void btn_f_Click(object sender, EventArgs e)
        {
            AddToOutput("F");
        }

        private void btn_g_Click(object sender, EventArgs e)
        {
            AddToOutput("G");
        }

        private void btn_h_Click(object sender, EventArgs e)
        {
            AddToOutput("H");
        }

        private void btn_j_Click(object sender, EventArgs e)
        {
            AddToOutput("J");
        }

        private void btn_k_Click(object sender, EventArgs e)
        {
            AddToOutput("K");
        }

        private void btn_l_Click(object sender, EventArgs e)
        {
            AddToOutput("L");
        }

        private void btn_z_Click(object sender, EventArgs e)
        {
            AddToOutput("Z");
        }

        private void btn_x_Click(object sender, EventArgs e)
        {
            AddToOutput("X");
        }

        private void btn_c_Click(object sender, EventArgs e)
        {
            AddToOutput("C");
        }

        private void btn_v_Click(object sender, EventArgs e)
        {
            AddToOutput("V");
        }

        private void btn_b_Click(object sender, EventArgs e)
        {
            AddToOutput("B");
        }

        private void btn_n_Click(object sender, EventArgs e)
        {
            AddToOutput("N");
        }

        private void btn_m_Click(object sender, EventArgs e)
        {
            AddToOutput("M");
        }
    }
}
