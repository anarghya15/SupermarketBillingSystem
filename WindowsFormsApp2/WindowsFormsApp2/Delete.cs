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
    public partial class Delete : Form
    {
        public Delete()
        {
            InitializeComponent();
        }

        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern Product* getProduct(int pno);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern int getProductNo(Product* P);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void getProductName(Product* P, byte[] s);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern float getProductPrice(Product* P);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern float getProductQuan(Product* P);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern float getProductDiscount(Product* P);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setProductNo(int no);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int deleteProduct();
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getNoOfProducts();
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                MessageBox.Show("Enter a product number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                unsafe
                {
                    Product* Pr = getProduct(int.Parse(textBox1.Text));
                    if (Pr == null)
                        MessageBox.Show("Invalid number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                    {
                        panel1.Visible = true;
                        byte[] b = new byte[300];
                        getProductName(Pr, b);
                        string s = Encoding.ASCII.GetString(b);
                        s = s.Replace("\0", string.Empty);
                        dataGridView1.Rows.Add(getProductNo(Pr).ToString(), s, getProductPrice(Pr).ToString(), getProductQuan(Pr).ToString(), getProductDiscount(Pr).ToString());
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            panel1.Visible = false;
            textBox1.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setProductNo(int.Parse(textBox1.Text));
            int r = deleteProduct();
            if(r == 1)
            {
                MessageBox.Show("Product removed successfully!","Success");
                dataGridView1.Rows.Clear();
                panel1.Visible = false;
                textBox1.Clear();
            }
            else
            {
                MessageBox.Show("Could not remove the product","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                dataGridView1.Rows.Clear();
                panel1.Visible = false;
                textBox1.Clear();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin a = new Admin();
            a.ShowDialog();
            this.Close();
        }

        private void Delete_Load(object sender, EventArgs e)
        {
            int no = getNoOfProducts();
            if(no == 0)
            {
                MessageBox.Show("No products present to remove!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Hide();
                Admin a = new Admin();
                a.ShowDialog();
                this.Close();
            }
        }
    }
}
