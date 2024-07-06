using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pwd = textBox1.Text;
            if(pwd == "12345")
            {
                this.Hide();
                Admin a = new Admin();
                a.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Password!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                textBox1.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Front f = new Front();
            f.ShowDialog();
            this.Close();
        }
    }
}
