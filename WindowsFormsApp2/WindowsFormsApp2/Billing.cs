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
using System.IO;

namespace WindowsFormsApp2
{
    public partial class Billing : Form
    {
        bool stored = false;
        public Billing()
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
        public static unsafe extern void calculateBill(float[] a, float[] dis);
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int getCount();
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern float getTotalAmt();
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void storeCustomerInfo(string n, string p, int[] nos);

        private void Billing_Load(object sender, EventArgs e)
        {
            unsafe
            {
                float[] amount = new float[100];
                float[] disamount = new float[100];
                calculateBill(amount, disamount);
                int i = 0;
                Product* Pr = getProduct(Customer.pnos[i]);
                if (Pr == null)
                    MessageBox.Show("No Products to display!","Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                else
                {
                    do
                    {
                        byte[] b = new byte[300];
                        getProductName(Pr, b);
                        string s = Encoding.ASCII.GetString(b);
                        s = s.Replace("\0", string.Empty);
                        dataGridView1.Rows.Add(getProductNo(Pr).ToString(), s, getProductPrice(Pr).ToString(), Customer.quan[i].ToString(), amount[i].ToString(),disamount[i].ToString());
                        Pr = getProduct(Customer.pnos[++i]);
                    } while (i < getCount());
                }
                textBox1.Text = getTotalAmt().ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!stored)
            {
                if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
                    MessageBox.Show("Please enter values in all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    string name = textBox2.Text;
                    string phoneno = textBox3.Text;
                    storeCustomerInfo(name, phoneno, Customer.pnos);
                    this.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
                MessageBox.Show("Please enter values in all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                SaveFileDialog svd = new SaveFileDialog
                {
                    InitialDirectory = @"D:\AAT & Self study\C#\Docs\",
                    Title = "Save Receipt",
                    Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*"
                };
                if (svd.ShowDialog() == DialogResult.OK)
                {
                    using (TextWriter tw = new StreamWriter(svd.FileName))
                    {
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            if (i == 1)
                                tw.Write(dataGridView1.Columns[i].HeaderText + "\t\t");
                            else
                                tw.Write(dataGridView1.Columns[i].HeaderText + "\t   ");
                        }
                        tw.WriteLine();

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                string s = row.Cells[j].Value.ToString();
                                if (j == 0 || j == 3)
                                    tw.Write(s + "\t\t     ");
                                else
                                    tw.Write(s + "\t     ");
                            }
                            tw.WriteLine();
                        }
                        tw.Write("\nTotal = {0}", getTotalAmt().ToString());
                    }
                    MessageBox.Show("Bill saved to file!", "Success");
                }

                string name = textBox2.Text;
                string phoneno = textBox3.Text;
                storeCustomerInfo(name, phoneno, Customer.pnos);
                stored = true;
            }
        }
    }
}
