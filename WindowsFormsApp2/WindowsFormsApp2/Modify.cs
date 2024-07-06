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
using System.Net.Http.Headers;


namespace WindowsFormsApp2
{
    public partial class Modify : Form
    {
        public Modify()
        {
            InitializeComponent();
        }

        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setProductNo(int no);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern Product* getProduct(int pno);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int getProductNo(Product* P);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void getProductName(Product *P,byte[] s);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern float getProductPrice(Product* P);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern float getProductQuan(Product* P);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern float getProductDiscount(Product* P);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int modifyProduct(int num, string name, float price, float quantity, float discount);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getNoOfProducts();

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin ad = new Admin();
            ad.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                MessageBox.Show("No product found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                unsafe
                {
                    Product* Pr = null;
                    Pr = getProduct(int.Parse(textBox1.Text));
                    if (Pr == null)
                        MessageBox.Show("No product found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //textBox1.Text = "";
                    else
                    {
                        textBox2.Text = getProductNo(Pr).ToString();
                        byte[] buf = new byte[300];
                        getProductName(Pr, buf);
                        string s = Encoding.ASCII.GetString(buf);
                        s.Replace("\0", string.Empty);
                        textBox3.Text = s;
                        textBox4.Text = getProductPrice(Pr).ToString();
                        textBox5.Text = getProductQuan(Pr).ToString();
                        textBox6.Text = getProductDiscount(Pr).ToString();
                    }
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox6.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox5.Text))
                MessageBox.Show("Please enter values in all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (!int.TryParse(textBox2.Text, out int i))
            {
                MessageBox.Show("Enter only numbers for product number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!float.TryParse(textBox4.Text, out float j))
            {
                MessageBox.Show("Enter only numbers for price", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!int.TryParse(textBox5.Text, out int k))
            {
                MessageBox.Show("Enter only numbers for quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!float.TryParse(textBox6.Text, out float l))
            {
                MessageBox.Show("Enter only numbers for discount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                int num = int.Parse(textBox2.Text);
                string name = textBox3.Text;
                float price = float.Parse(textBox4.Text);
                float quantity = float.Parse(textBox5.Text);
                float discount = float.Parse(textBox6.Text);
                setProductNo(int.Parse(textBox1.Text));

                int r = modifyProduct(num, name, price, quantity, discount);
                if (r == 1)
                    MessageBox.Show("Product modified successfully!", "Success");
                else
                    MessageBox.Show("Could not change product details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
        private void Modify_Load(object sender, EventArgs e)
        {
            int no = getNoOfProducts();
            if(no == 0)
            {
                MessageBox.Show("No products present!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Hide();
                Admin ad = new Admin();
                ad.ShowDialog();
                this.Close();
            }
        }
    }
}
