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
    public partial class Customer : Form
    {
        public static int[] pnos = new int[100];
        public static int[] quan = new int[100];
        int index = 0;
        public Customer()
        {
            InitializeComponent();
        }

        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern Product* getProduct(int prod_no);
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
        public static extern int getNoOfProducts();
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        //[return: MarshalAs(UnmanagedType.SysInt)]
        public static unsafe extern IntPtr getAllProductNos();
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getDetails(int no, int q);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getCount();

        private void Customer_Load(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            int no = getNoOfProducts();
            int[] prnos = new int[no];
            int i = 0;
            if (no != 0)
            {
                dataGridView1.Visible = true;
                unsafe
                {
                    IntPtr p = getAllProductNos();
                    Marshal.Copy(p, prnos, 0, no);
                    
                    while (i < no)
                    {
                        int a = prnos[i];
                        Product* Pr = getProduct(prnos[i]);
                        byte[] b = new byte[300];
                        getProductName(Pr, b);
                        string s = Encoding.ASCII.GetString(b);
                        s = s.Replace("\0", string.Empty);
                        dataGridView1.Rows.Add(getProductNo(Pr).ToString(), s, getProductPrice(Pr).ToString(), getProductDiscount(Pr).ToString());
                        i++;
                    }

                }
                dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
                string[] pnos = new string[100];
                i = 0;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    pnos[i] = (string)row.Cells[0].Value;
                    i++;
                }
                for (i = 0; i < dataGridView1.Rows.Count; i++)
                    comboBox1.Items.Add(pnos[i]);
                this.Controls.Add(comboBox1);
            }
            else
            {
                MessageBox.Show("No Products to display!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Hide();
                Front f = new Front();
                f.ShowDialog();
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                MessageBox.Show("Please select an item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                int pno = int.Parse(comboBox1.SelectedItem.ToString());
                int q = (int)numericUpDown1.Value;
                pnos[index] = pno;
                quan[index] = q;
                index++;
                int r = getDetails(pno, q);
                if (r == 0)
                    MessageBox.Show("Sorry, Item is out of stock, contact admin!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    MessageBox.Show("Item added to cart!", "Success");
                comboBox1.SelectedIndex = -1;
                numericUpDown1.Value = 1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (getCount() == 0)
                MessageBox.Show("Please purchase atleast one item!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            else
            {
                this.Hide();
                Billing b = new Billing();
                b.ShowDialog();
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Front f = new Front();
            f.ShowDialog();
            this.Close();
        }
    }
}
