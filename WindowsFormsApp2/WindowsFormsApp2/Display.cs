using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Display : Form
    {
        public Display()
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
        public static extern int getNoOfProducts();
        [DllImport("D:\\Apps\\Visual Studio Projects\\SupermarketWrapper\\Debug\\SupermarketBillingLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        //[return: MarshalAs(UnmanagedType.SysInt)]
        public static unsafe extern IntPtr getAllProductNos();

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin a = new Admin();
            a.ShowDialog();
            this.Close();
        }

        private void Display_Load(object sender, EventArgs e)
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
                        Product* Pr = getProduct(prnos[i]);
                        byte[] b = new byte[300];
                        getProductName(Pr, b);
                        string s = Encoding.ASCII.GetString(b);
                        s = s.Replace("\0", string.Empty);
                        dataGridView1.Rows.Add(getProductNo(Pr).ToString(), s, getProductPrice(Pr).ToString(), getProductQuan(Pr).ToString(), getProductDiscount(Pr).ToString());
                        i++;
                    }                    
                }
                dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
            }
            else
            {
                MessageBox.Show("No products to display", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Hide();
                Admin a = new Admin();
                a.ShowDialog();
                this.Close();
            }
        }
    }
}
