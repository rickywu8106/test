using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace OrderSystem
{
    public partial class Form1 : Form
    {
        SqlConnectionStringBuilder scsb;
        int Number = 0;//點餐數量變數
        List<string> Temp = new List<string>();




        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbo_class.Items.Add("App班");
            cbo_class.Items.Add("網工班");
            cbo_class.Items.Add("BigData班");

            scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = @".";
            scsb.InitialCatalog = "OrderSystem";
            scsb.IntegratedSecurity = true;

            timer1.Tag = DateTime.Now;
            //PictureBox1.Image = Image.FromFile(@"C:\Users\iii\Desktop\OrderSystem\資策會.png");
            this.tabPage1.Parent = this.tabOrder;
            this.tabPage2.Parent = null;
            this.tabPage3.Parent = null;
            this.tabPage4.Parent = null;
            this.tabPage5.Parent = null;
            this.tabPage6.Parent = null;
            this.tabPage7.Parent = null;
            this.tabPage8.Parent = null;
            this.tabPage9.Parent = null;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboStudent.Items.Clear();
            cboStore.Items.Clear();
            cboStore.Text = "";
            cboStudent.Text = "";
            cboMeals.Text = "";
            txtSOD.Text = "";

            SqlConnection con = new SqlConnection(scsb.ToString());
            con.Open();
            string strSQL = $"select * from Students where S_Class = '{cbo_class.Text}'";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                cboStudent.Items.Add(reader["S_Name"]);
            }
            reader.Close();
            con.Close();

            SqlConnection con2 = new SqlConnection(scsb.ToString());
            con2.Open();
            string strSQL2 = $@"select OD_SOD  ,OD_Store from OrderDetail 
                             where OD_Class = '{cbo_class.Text}' 
                             and OD_Date = CAST(CURRENT_TIMESTAMP AS DATE)";
            SqlCommand cmd2 = new SqlCommand(strSQL2, con2);
            SqlDataReader reader2 = cmd2.ExecuteReader();

            while (reader2.Read())
            {
                txtSOD.Text = reader2[0].ToString();
                cboStore.Text = reader2[1].ToString();
            }
            reader2.Close();
            con2.Close();

            if (txtSOD.Text == "")
            {
                SqlConnection con3 = new SqlConnection(scsb.ToString());
                con3.Open();
                string strSQL3 = $"SELECT SOD_Name  FROM SOD WHERE SOD_Date = CAST(CURRENT_TIMESTAMP AS DATE) and SOD_Class ='{cbo_class.Text}'";
                SqlCommand cmd3 = new SqlCommand(strSQL3, con3);
                SqlDataReader reader3 = cmd3.ExecuteReader();

                while (reader3.Read())
                {
                    txtSOD.Text = reader3[0].ToString();
                }
                reader3.Close();
                con3.Close();
            }


        }
        private void btnOrder_Click(object sender, EventArgs e)
        {
            if ((cbo_class.Text != "") && (cboStudent.Text != ""))
            {

                tabOrder.SelectedIndex = 1;

                this.tabPage1.Parent = null;
                this.tabPage2.Parent = this.tabOrder;
                this.tabPage3.Parent = null;
                this.tabPage4.Parent = null;
                this.tabPage5.Parent = null;
                this.tabPage6.Parent = null;
                this.tabPage7.Parent = null;
                this.tabPage8.Parent = null;
                this.tabPage9.Parent = null;


                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = $"SELECT OD_SOD  FROM OrderDetail WHERE OD_Date = CAST(CURRENT_TIMESTAMP AS DATE) and OD_Class ='{cbo_class.Text}'";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    txtSOD.Text = reader[0].ToString();
                }
                reader.Close();
                con.Close();


                if ((cboStudent.Text == txtSOD.Text) && (cboStore.Text == ""))
                {
                    cboStore.Items.Clear();
                    cboStore.Enabled = true;

                    SqlConnection con2 = new SqlConnection(scsb.ToString());
                    con2.Open();
                    string strSQL2 = "select * from Store";
                    SqlCommand cmd2 = new SqlCommand(strSQL2, con2);
                    SqlDataReader reader2 = cmd2.ExecuteReader();

                    while (reader2.Read())
                    {
                        cboStore.Items.Add(reader2["Sto_Name"]);
                    }
                    reader2.Close();
                    con2.Close();
                }
                else
                {
                    cboStore.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("請先選擇登入身分");
            }
        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
            if ((cbo_class.Text != "") && (cboStudent.Text != ""))
            {
                tabOrder.SelectedIndex = 8;
                this.tabPage1.Parent = null;
                this.tabPage2.Parent = null;
                this.tabPage3.Parent = null;
                this.tabPage4.Parent = null;
                this.tabPage5.Parent = null;
                this.tabPage6.Parent = null;
                this.tabPage7.Parent = null;
                this.tabPage8.Parent = null;
                this.tabPage9.Parent = this.tabOrder;
            }
            else
            {
                MessageBox.Show("請先選擇登入身分");
            }
        }

        private void btnStore_Click(object sender, EventArgs e)
        {
            if ((cbo_class.Text != "") && (cboStudent.Text != ""))
            {
                tabOrder.SelectedIndex = 7;
                this.tabPage1.Parent = null;
                this.tabPage2.Parent = null;
                this.tabPage3.Parent = null;
                this.tabPage4.Parent = null;
                this.tabPage5.Parent = null;
                this.tabPage6.Parent = null;
                this.tabPage7.Parent = null;
                this.tabPage8.Parent = this.tabOrder;
                this.tabPage9.Parent = null;
            }
            else
            {
                MessageBox.Show("請先選擇登入身分");
            }
        }

        private void cboMeals_SelectedIndexChanged(object sender, EventArgs e)
        {
            nudnumber.Value = 1;
            SqlConnection con = new SqlConnection(scsb.ToString());
            con.Open();
            string strSQL = $"select Unitprice from Menu where Item = '{cboMeals.Text}'";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                txtUnitPrice.Text = reader[0].ToString();
            }
            reader.Close();
            con.Close();
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            if (cboMeals.Text.Length > 0)
            {
                int a = 0;
                int b = Convert.ToInt32(nudnumber.Value);
                Int32.TryParse(txtUnitPrice.Text, out a);

                DataGridViewRowCollection rows = dataGridView1.Rows;
                rows.Add(cboMeals.Text, txtUnitPrice.Text, nudnumber.Value, a * b);
            }
            else
            {
                MessageBox.Show("您尚未點餐!");
            }


        }
        private void btnCancel_Click(object sender, EventArgs e)
        {

            DataGridViewRowCollection rows = dataGridView1.Rows;
            rows.Remove(dataGridView1.CurrentRow);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DialogResult confirm;
            confirm = MessageBox.Show(" 確認送出訂購單 ", "訂購單確認",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.No)
            {
                //
            }
            else if (confirm == DialogResult.Yes)
            {
                tabOrder.SelectedIndex = 3;

                this.tabPage1.Parent = null;
                this.tabPage2.Parent = null;
                this.tabPage3.Parent = null;
                this.tabPage4.Parent = this.tabOrder;
                this.tabPage5.Parent = null;
                this.tabPage6.Parent = null;
                this.tabPage7.Parent = null;
                this.tabPage8.Parent = null;
                this.tabPage9.Parent = null;

                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    SqlConnection con = new SqlConnection(scsb.ToString());
                    con.Open();
                    string strSQL = "insert into OrderDetail values(@OD_Class,@OD_SOD,@OD_Name,@OD_Meals,@OD_UnitPrice,@OD_Qty,@OD_Amount,@OD_Store,@OD_Date)";
                    SqlCommand cmd = new SqlCommand(strSQL, con);

                    cmd.Parameters.AddWithValue("@OD_Class", cbo_class.Text);
                    cmd.Parameters.AddWithValue("@OD_SOD", txtSOD.Text);
                    cmd.Parameters.AddWithValue("@OD_Name", txtStudent.Text);
                    cmd.Parameters.AddWithValue("@OD_Meals", dataGridView1.Rows[i].Cells[0].Value.ToString());
                    cmd.Parameters.AddWithValue("@OD_UnitPrice", dataGridView1.Rows[i].Cells[1].Value.ToString());
                    cmd.Parameters.AddWithValue("@OD_Qty", dataGridView1.Rows[i].Cells[2].Value.ToString());
                    cmd.Parameters.AddWithValue("@OD_Amount", dataGridView1.Rows[i].Cells[3].Value.ToString());
                    cmd.Parameters.AddWithValue("@OD_Store", cboStore.Text);
                    cmd.Parameters.AddWithValue("@OD_Date", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.mmm"));

                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                SqlConnection con2 = new SqlConnection(scsb.ToString());
                con2.Open();
                string strSQL2 = $@"select * from OrderDetail where OD_Class = '{cbo_class.Text}'
                                 and OD_Date =  CAST(CURRENT_TIMESTAMP AS DATE) order by OD_Date";
                SqlCommand cmd2 = new SqlCommand(strSQL2, con2);
                SqlDataReader reader2 = cmd2.ExecuteReader();

                while (reader2.Read())
                {
                    DataGridViewRowCollection rows2 = dataGridView2.Rows;
                    rows2.Add(reader2[0], reader2[1], reader2[2], reader2[3], reader2[4], reader2[5], reader2[6], reader2[7], reader2[8]);
                }
                reader2.Close();
                con2.Close();


                int sum = 0;
                int a = 0;
                for (int i = 0; i < dataGridView2.RowCount - 1; i++)
                {
                    Int32.TryParse(dataGridView2.Rows[i].Cells[6].Value.ToString(), out a);
                    sum += a;
                }

                txtamount.Text = sum.ToString();

                if (txtSOD.Text == cboStudent.Text)
                {
                    btnbill.Enabled = true;
                    btnsinglecancel.Enabled = true;
                    btndrop.Enabled = true;
                }
                else
                {
                    btnbill.Enabled = false;
                    btnsinglecancel.Enabled = false;
                    btndrop.Enabled = false;
                }

            }
        }
        private void cboStore_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            cboMeals.Items.Clear();
            SqlConnection con = new SqlConnection(scsb.ToString());
            con.Open();
            string strSQL = $"select * from Menu where Sto_Name = '{cboStore.Text}'";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                cboMeals.Items.Add(reader["Item"]);
            }
            reader.Close();
            con.Close();

        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            txtUnitPrice.Text = ""; 
            txtStore.Text = "";
            cboMeals.Text = "";
            nudnumber.Value = 1;
            tabOrder.SelectedIndex = 1;

            this.tabPage1.Parent = null;
            this.tabPage2.Parent = this.tabOrder;
            this.tabPage3.Parent = null;
            this.tabPage4.Parent = null;
            this.tabPage5.Parent = null;
            this.tabPage6.Parent = null;
            this.tabPage7.Parent = null;
            this.tabPage8.Parent = null;
            this.tabPage9.Parent = null;

        }

        private void btnbill_Click(object sender, EventArgs e)
        {
            SqlConnection con2 = new SqlConnection(scsb.ToString());
            con2.Open();
            string strSQL2 = $@"delete from Orders where O_Class ='{cbo_class.Text}' and  O_Date = CAST(CURRENT_TIMESTAMP AS DATE)";
            SqlCommand cmd2 = new SqlCommand(strSQL2, con2);
            cmd2.ExecuteNonQuery();
            con2.Close();


            string OrderList = "III餐飲訂購系統\r\n\r\n"
                               +$"班級 : {cbo_class.Text}" + "     "
                               + $"值日生 : {txtSOD.Text}" + "     "
                               + $"訂購店家 : {cboStore.Text}\r\n\r\n"
                               + "訂購明細\r\n"
                               + "-----------------------------------------------------------\r\n";


            SqlConnection con = new SqlConnection(scsb.ToString());
            con.Open();
            string strSQL = $@"select OD_Meals , OD_Unitprice ,sum(OD_Qty) from OrderDetail
                            where OD_Date = CAST(CURRENT_TIMESTAMP AS DATE) and OD_Class = '{cbo_class.Text}'
                            group by OD_Meals , OD_Unitprice";
            
            SqlCommand cmd = new SqlCommand(strSQL, con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                OrderList += reader[0];
                OrderList += "    ";
                OrderList += " X "+reader[2]+"     "+"$";
                OrderList += reader[1].ToString();
                OrderList += "\r\n";
            }
            reader.Close();
            con.Close();

            OrderList += "-----------------------------------------------------------\r\n"
                      +$"合計 :  ${txtamount.Text:c0}\r\n\r\n"
                      +DateTime.Now;
            MessageBox.Show(OrderList);

            SqlConnection con3 = new SqlConnection(scsb.ToString());
            con3.Open();
            string strSQL3 = $@"insert into Orders values('{cbo_class.Text}','{txtSOD.Text}','{cboStore.Text}','{txtamount.Text}','{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.mmm")}')";
            SqlCommand cmd3 = new SqlCommand(strSQL3, con3);
            cmd3.ExecuteNonQuery();
            con3.Close();

            string strFilePath = $@"OrderList-{cbo_class.Text}-{DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss")}.txt";
            StreamWriter myOrder = new StreamWriter(strFilePath);
            myOrder.Write(OrderList);
            myOrder.Close();
            MessageBox.Show("結帳完成!");

            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSOD_Click(object sender, EventArgs e)
        {
            txtSOD.Text = cboStudent.Text;
            cboStore.Enabled = true;
        }

        private void btnagent_Click(object sender, EventArgs e)
        {
            if (cboStore.Text == "")
            {
                txtSOD.Text = cboStudent.Text;
                cboStore.Items.Clear();
                cboStore.Enabled = true;

                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = "select * from Store";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cboStore.Items.Add(reader["Sto_Name"]);
                }
                reader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("今天已經有人負責訂餐!");
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (cboStore.Text != "")
            {
                tabOrder.SelectedIndex = 2;
                this.tabPage1.Parent = null;
                this.tabPage2.Parent = null;
                this.tabPage3.Parent = this.tabOrder;
                this.tabPage4.Parent = null;
                this.tabPage5.Parent = null;
                this.tabPage6.Parent = null;
                this.tabPage7.Parent = null;
                this.tabPage8.Parent = null;
                this.tabPage9.Parent = null;
                txtStudent.Text = cboStudent.Text;
                txtStore.Text = cboStore.Text;
                lblbSOD.Text = txtSOD.Text; ;

                cboMeals.Items.Clear();
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = $"select * from Menu where Sto_Name = '{cboStore.Text}'";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cboMeals.Items.Add(reader["Item"]);
                }
                reader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("今日尚無值日生負責訂餐");
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            tabOrder.SelectedIndex = 0;
            this.tabPage1.Parent = this.tabOrder;
            this.tabPage2.Parent = null;
            this.tabPage3.Parent = null;
            this.tabPage4.Parent = null;
            this.tabPage5.Parent = null;
            this.tabPage6.Parent = null;
            this.tabPage7.Parent = null;
            this.tabPage8.Parent = null;
            this.tabPage9.Parent = null;
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void btnsinglecancel_Click(object sender, EventArgs e)
        {
            if (txtSOD.Text == cboStudent.Text)
            {
                btnsinglecancel.Enabled = true;

                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = $@"delete from OrderDetail where OD_Name = '{ dataGridView2.CurrentRow.Cells[2].Value}'
                             and OD_Meals = '{dataGridView2.CurrentRow.Cells[3].Value}'
                             and OD_Date = '{dataGridView2.CurrentRow.Cells[8].Value}'";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.ExecuteNonQuery();
                con.Close();

                DataGridViewRowCollection rows2 = dataGridView2.Rows;
                rows2.Remove(dataGridView2.CurrentRow);

                int sum = 0;
                int a = 0;
                for (int i = 0; i < dataGridView2.RowCount - 1; i++)
                {
                    Int32.TryParse(dataGridView2.Rows[i].Cells[6].Value.ToString(), out a);
                    sum += a;
                }
                txtamount.Text = sum.ToString();
            }
        }

        private void btnordermnage_Click(object sender, EventArgs e)
        {
            if ((cbo_class.Text != "")&&(cboStudent.Text!=""))
            {
                tabOrder.SelectedIndex = 6;
                this.tabPage1.Parent = null;
                this.tabPage2.Parent = null;
                this.tabPage3.Parent = null;
                this.tabPage4.Parent = null;
                this.tabPage5.Parent = null;
                this.tabPage6.Parent = null;
                this.tabPage7.Parent = this.tabOrder;
                this.tabPage8.Parent = null;
                this.tabPage9.Parent = null;
            }
            else
            {
                MessageBox.Show("請先選擇登入身分");
            }


        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string User = "ricky";
            string Password = "password3";
                

            if ((txtUser.Text == User) && (txtPassword.Text == Password))
            {
                tabOrder.SelectedIndex = 3;
                this.tabPage1.Parent = null;
                this.tabPage2.Parent = null;
                this.tabPage3.Parent = null;
                this.tabPage4.Parent = this.tabOrder;
                this.tabPage5.Parent = null;
                this.tabPage6.Parent = null;
                this.tabPage7.Parent = null;
                this.tabPage8.Parent = null;
                this.tabPage9.Parent = null;

                if (txtSOD.Text == cboStudent.Text)
                {
                    btnsinglecancel.Enabled = true;
                    btnbill.Enabled = true;
                    btndrop.Enabled = true;
                }
                else
                {
                    btnsinglecancel.Enabled = false;
                    btnbill.Enabled = false;
                    btndrop.Enabled = false;
                }

                SqlConnection con2 = new SqlConnection(scsb.ToString());
                con2.Open();
                string strSQL2 = $@"select * from OrderDetail where OD_Class='{cbo_class.Text}'
                                 and OD_Date = CAST(CURRENT_TIMESTAMP AS DATE) order by OD_Date";
                SqlCommand cmd2 = new SqlCommand(strSQL2, con2);
                SqlDataReader reader2 = cmd2.ExecuteReader();

                while (reader2.Read())
                {
                    DataGridViewRowCollection rows2 = dataGridView2.Rows;
                    rows2.Add(reader2[0], reader2[1], reader2[2], reader2[3], reader2[4], reader2[5], reader2[6], reader2[7], reader2[8]);
                }
                reader2.Close();
                con2.Close();

                int sum = 0;
                int a = 0;
                for (int i = 0; i < dataGridView2.RowCount - 1; i++)
                {
                    Int32.TryParse(dataGridView2.Rows[i].Cells[6].Value.ToString(), out a);
                    sum += a;
                }
                txtamount.Text = sum.ToString();
            }
            else
            {
                MessageBox.Show("您輸入的帳號密碼錯誤!");
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            tabOrder.SelectedIndex = 0;
            this.tabPage1.Parent = this.tabOrder;
            this.tabPage2.Parent = null;
            this.tabPage3.Parent = null;
            this.tabPage4.Parent = null;
            this.tabPage5.Parent = null;
            this.tabPage6.Parent = null;
            this.tabPage7.Parent = null;
            this.tabPage8.Parent = null;
            this.tabPage9.Parent = null;
            txtUser.Text = "";
            txtPassword.Text = "";
        }

        private void btnlogin1_Click(object sender, EventArgs e)
        {
            string User2 = "ricky";
            string Password2 = "password3";


            if ((txtUser1.Text == User2) && (txtPassword1.Text == Password2))
            {
                tabOrder.SelectedIndex = 4;
                this.tabPage1.Parent = null;
                this.tabPage2.Parent = null;
                this.tabPage3.Parent = null;
                this.tabPage4.Parent = null;
                this.tabPage5.Parent = this.tabOrder;
                this.tabPage6.Parent = null;
                this.tabPage7.Parent = null;
                this.tabPage8.Parent = null;
                this.tabPage9.Parent = null;

                SqlConnection


            }
            else
            {
                MessageBox.Show("您輸入的帳號密碼錯誤!");
            }
        }

        private void btnlogout1_Click(object sender, EventArgs e)
        {
            tabOrder.SelectedIndex = 0;
            this.tabPage1.Parent = this.tabOrder;
            this.tabPage2.Parent = null;
            this.tabPage3.Parent = null;
            this.tabPage4.Parent = null;
            this.tabPage5.Parent = null;
            this.tabPage6.Parent = null;
            this.tabPage7.Parent = null;
            this.tabPage8.Parent = null;
            this.tabPage9.Parent = null;
            txtUser1.Text = "";
            txtPassword1.Text = "";
        }

        private void btnlogin2_Click(object sender, EventArgs e)
        {
            string User1 = "ricky";
            string Password1 = "password3";


            if ((txtUser2.Text == User1) && (txtPassword2.Text == Password1))
            {
                tabOrder.SelectedIndex = 5;
                this.tabPage1.Parent = null;
                this.tabPage2.Parent = null;
                this.tabPage3.Parent = null;
                this.tabPage4.Parent = null;
                this.tabPage5.Parent = null;
                this.tabPage6.Parent = this.tabOrder;
                this.tabPage7.Parent = null;
                this.tabPage8.Parent = null;
                this.tabPage9.Parent = null;


            }
            else
            {
                MessageBox.Show("您輸入的帳號密碼錯誤!");
            }
        }

        private void btnlogout2_Click(object sender, EventArgs e)
        {
            tabOrder.SelectedIndex = 0;
            this.tabPage1.Parent = this.tabOrder;
            this.tabPage2.Parent = null;
            this.tabPage3.Parent = null;
            this.tabPage4.Parent = null;
            this.tabPage5.Parent = null;
            this.tabPage6.Parent = null;
            this.tabPage7.Parent = null;
            this.tabPage8.Parent = null;
            this.tabPage9.Parent = null;
            txtUser2.Text = "";
            txtPassword2.Text = "";
        }

        private void btnreset_Click(object sender, EventArgs e)
        {
            DialogResult Reset;
            DialogResult confirm;
            Reset = MessageBox.Show(" 確認抽單 ", "取消今日訂單",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Reset == DialogResult.Yes)
            {
                SqlConnection con = new SqlConnection(scsb.ToString());
                con.Open();
                string strSQL = $@"delete from Orders where O_Class = '{cbo_class.Text}'
                                 and O_Date =  CAST(CURRENT_TIMESTAMP AS DATE)";
                SqlCommand cmd = new SqlCommand(strSQL, con);
                cmd.ExecuteNonQuery();
                con.Close();


                SqlConnection con2 = new SqlConnection(scsb.ToString());
                con2.Open();
                string strSQL2 = $@"delete from OrderDetail where OD_Class = '{cbo_class.Text}'
                                 and OD_Date =  CAST(CURRENT_TIMESTAMP AS DATE)";
                SqlCommand cmd2 = new SqlCommand(strSQL2, con2);
                cmd2.ExecuteNonQuery();
                con2.Close();

                dataGridView2.Rows.Clear();
                txtamount.Text = "";

                MessageBox.Show("抽單成功!");

                this.Close();

            }
            else
            {
                //
            }

        }
    }
}
