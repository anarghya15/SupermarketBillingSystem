using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApp2
{
    public partial class Add : Form
    {
        public Add()
        {
            InitializeComponent();
        }

        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int addProduct(int num, string name, float price, float quantity, float discount);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        //[return: MarshalAs(UnmanagedType.SysInt)]
        public static unsafe extern IntPtr getAllProductNos();
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getNoOfProducts();

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox5.Text))
                MessageBox.Show("Please enter values in all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (!int.TryParse(textBox1.Text, out int i))
            {
                MessageBox.Show("Enter only numbers for product number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!float.TryParse(textBox3.Text, out float j))
            {
                MessageBox.Show("Enter only numbers for price", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!int.TryParse(textBox4.Text, out int k))
            {
                MessageBox.Show("Enter only numbers for quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!float.TryParse(textBox5.Text, out float l))
            {
                MessageBox.Show("Enter only numbers for quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                int num = int.Parse(textBox1.Text);
                int no = getNoOfProducts();
                int[] prnos = new int[no];
                IntPtr p = getAllProductNos();
                Marshal.Copy(p, prnos, 0, no);
                if (prnos.Contains(num))
                    MessageBox.Show("Product number already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    string name = textBox2.Text;
                    float price = float.Parse(textBox3.Text);
                    float quantity = float.Parse(textBox4.Text);
                    float discount = float.Parse(textBox5.Text);

                    int r = addProduct(num, name, price, quantity, discount);
                    if (r == 1)
                    {
                        MessageBox.Show("Product added successfully!", "Success");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                    }
                    else
                        MessageBox.Show("Could not add product", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin a = new Admin();
            a.ShowDialog();
            this.Close();
        }
    }
}
