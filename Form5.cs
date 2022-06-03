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
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Form5 : Form
    {
        int i = 0;
        int namt = 0;
        int tmp_qty = 0;

        public static String mysql_str = "server=127.0.0.1; port=3306; Database=gbj; Uid=root; Pwd=1234; Charset=utf8";

        MySqlConnection conn = new MySqlConnection(mysql_str);
        MySqlCommand cmd;
        MySqlCommand cmd1;
        MySqlCommand cmd2;
        MySqlCommand cmd3;
        MySqlDataReader reader;

        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            
            iodateBox.Text = System.DateTime.Now.ToString("yyyyMMdd");
        }

        private void hp_Display()
        {
            //전화번호가 입력이 되면 gorder 테이블에서 주문 내역이 있는지 확인 후
            //내역이 있으면 Display
            //없으면 놔두기

            String sql1 = " select ioname from gbj " +
                          " where iohp=@iohp1 " +
                          " order by iodate+ioseq desc limit 1 ";

            if (reader != null) reader.Close();
            cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql1;

            cmd.Parameters.AddWithValue("@iohp1", (iohpBox.Text));

            reader = cmd.ExecuteReader();

            /*int i = 0;*/
            if (reader.Read() == true)
            {
                ionameBox.Text = (string)reader["ioname"];
                /*datBox.Text = (string)reader["orddat"];
                seqBox.Text = ((int)reader["ordseq"]).ToString();*/
                ionameBox.ReadOnly = true;
            }
            else
            {
                ionameBox.Text = "";
                /* addBox.Text = "";
                 datBox.Text = System.DateTime.Now.ToString("yyyyMMdd");
                 seqBox.Text = i.ToString();*/
                ionameBox.ReadOnly = false;
            }

            String sql2 = " select nvl(max(ioseq), 0) as maxseq from gbj " +
                          " where iohp = @iohp2 and iodate = @iodate2 ";

            if (reader != null) reader.Close();
            cmd1 = new MySqlCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = sql2;

            cmd1.Parameters.AddWithValue("@iohp2", (iohpBox.Text));
            cmd1.Parameters.AddWithValue("@iodate2", (iodateBox.Text));

            reader = cmd1.ExecuteReader();

            int iseq = 0;
            if (reader.Read() == true)
            {
                iseq = ((int)reader["maxseq"] + 1);
                //textBox2.Text = (string)reader["ordnam"];
                //textBox3.Text = (string)reader["ordadd"];
                //textBox6.Text = (string)reader["orddat"];
                ioseqBox.Text = (iseq).ToString();
                //seqBox.Text = ((int)reader["maxseq"]+1).ToString();
            }
        }

        private void choBtn_Click(object sender, EventArgs e)
        {
            int j = 0;
            int hcnt = 1;

            String chk = "N";            //햄버거 종류 체크

            while (j < dataGridView1.Rows.Count)
            {
                if (dataGridView1.Rows[j].Cells[0].Value.ToString() == kindBox.Text)
                {
                    chk = "Y";
                }
                j++;
            }
            if (chk == "Y")
            {
                MessageBox.Show(kindBox.Text + "가 이미 선택되어있습니다.");
                return;
            }

            int amt = 0;       //햄버거 값

            if (kindBox.Text == "치즈버거세트") { amt = 4500; }
            if (kindBox.Text == "치킨버거세트") { amt = 5000; }
            if (kindBox.Text == "에그버거세트") { amt = 4000; }
            if (kindBox.Text == "슈슈버거세트") { amt = 7500; }

            int total = amt * hcnt;  //금액 계산
            namt = namt + total;                //금액 누적

            dataGridView1.Rows.Add();

            dataGridView1.Rows[i].Cells[0].Value = kindBox.Text;
            dataGridView1.Rows[i].Cells[1].Value = amt;
            dataGridView1.Rows[i].Cells[2].Value = hcnt;
            dataGridView1.Rows[i].Cells[3].Value = total;
            i++;
            //tseq++;

            iototBox.Text = namt.ToString();

            calc();
        }

        private void plusBtn_Click(object sender, EventArgs e)
        {
            int tmp_qty = 0;

            try
            {
                if (dataGridView1.CurrentRow.Index >= 0)
                {
                    tmp_qty = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString());
                    int pre_qty = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString());
                    tmp_qty = tmp_qty + 1;
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value = tmp_qty.ToString();
                    /*dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.CurrentRow.Index]);
                    i--;*/
                    int pre_tot = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString()) * pre_qty; ;
                    int total = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString()) * tmp_qty;
                    namt = namt + total - pre_tot;

                    iototBox.Text = namt.ToString();
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value = total;
                    /*textBox5.Text = .ToString();*/

                    calc();
                }
            }
            catch (NullReferenceException ex1)
            {
                MessageBox.Show("추가시킬 항목이 없습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void calc()
        {
           /* double tmp_amt = int.Parse(ioftotBox.Text);

            if(iomac.Checked == true) {
                tmp_amt = 0;
            }
            else
            {
                if (iochz.Checked == true)
                {
                    tmp_amt = tmp_amt * 0.1;
                }
                else
                {
                    if (iocake.Checked == true)
                    {
                        tmp_amt = tmp_amt * 0.2;
                    }
                }
            }*/
            


            ioftotBox.Text = (int.Parse(ioptotBox.Text) + int.Parse(iototBox.Text)).ToString();
            //iototBox.Text = namt.ToString();
        }

        private void minusBtn_Click(object sender, EventArgs e)
        {
            int tmp_qty = 0;

            try
            {
                tmp_qty = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString());

                if (dataGridView1.CurrentRow.Index >= 0 && tmp_qty > 0)
                {
                    //tmp_qty = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString());
                    int pre_qty = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString());
                    tmp_qty = tmp_qty - 1;
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value = tmp_qty.ToString();
                    /*dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.CurrentRow.Index]);
                    i--;*/
                    int pre_tot = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString()) * pre_qty; ;
                    int total = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString()) * tmp_qty;
                    namt = namt + total - pre_tot;

                    iototBox.Text = namt.ToString();
                    dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value = total;
                    /*textBox5.Text = .ToString();*/

                    calc();
                }

                if (tmp_qty == 0)
                {
                    dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.CurrentRow.Index]);
                    i--;
                }
            }
            catch (NullReferenceException ex1)
            {
                MessageBox.Show("감소시킬 항목이 없습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void canBtn_Click(object sender, EventArgs e)
        {
            int tmp_amt = 0;

            //int samt = int.Parse(textBox4.Text);

            try
            {
                if (dataGridView1.CurrentRow.Index >= 0)
                {
                    tmp_amt = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value.ToString());
                    dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.CurrentRow.Index]);
                    i--;
                    namt = namt - tmp_amt;

                    iototBox.Text = namt.ToString();
                    ioftotBox.Text = tmp_amt.ToString();

                    calc();
                }
            }
            catch (NullReferenceException ex1)
            {
                MessageBox.Show("삭제 할 데이터가 없습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void iomac_CheckedChanged(object sender, EventArgs e)
        {
            int tmp_amt = int.Parse(ioptotBox.Text);
            if (iomac.Checked == true)
            {
                tmp_amt = tmp_amt + 20000;
            }
            else
            {
                tmp_amt = tmp_amt - 20000;
            }

            ioptotBox.Text = tmp_amt.ToString();

            calc();
        }

        private void iochz_CheckedChanged(object sender, EventArgs e)
        {
            int tmp_amt = int.Parse(ioptotBox.Text);
            if (iochz.Checked == true)
            {
                tmp_amt = tmp_amt + 10000;
            }
            else
            {
                tmp_amt = tmp_amt - 10000;
            }

            ioptotBox.Text = tmp_amt.ToString();

            calc();
        }

        private void iocake_CheckedChanged(object sender, EventArgs e)
        {
            int tmp_amt = int.Parse(ioptotBox.Text);
            if (iocake.Checked == true)
            {
                tmp_amt = tmp_amt + 5000;
            }
            else
            {
                tmp_amt = tmp_amt - 5000;
            }

            ioptotBox.Text = tmp_amt.ToString();

            calc();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            double tmp_amt = int.Parse(ioftotBox.Text);

            if(ionones.Checked == true)
            {
                tmp_amt = tmp_amt - (tmp_amt * 0);
                ioftotBox.Text = tmp_amt.ToString();
            }
            else
            {
                calc();
            }
        }

        private void iotels_CheckedChanged(object sender, EventArgs e)
        {

             double tmp_amt = int.Parse(ioftotBox.Text);

             if (iotels.Checked == true)
             {
                 tmp_amt = tmp_amt - (tmp_amt * 0.1);
                 ioftotBox.Text = tmp_amt.ToString();
             }
             else
             {
                 calc();
                 //ioftotBox.Text = tmp_amt.ToString();
             }
             //calc();
        }

        private void iomems_CheckedChanged(object sender, EventArgs e)
        {
            
            double tmp_amt = int.Parse(ioftotBox.Text);

            if (iomems.Checked == true)
            {
                tmp_amt = tmp_amt - (tmp_amt * 0.2);
                ioftotBox.Text = tmp_amt.ToString();
            }
            else
            {
                calc();
                
                //ioftotBox.Text = tmp_amt.ToString();
            }
            //calc();
        }

        private void ordBtn_Click(object sender, EventArgs e)
        {
            if (iohpBox.Text == "")
            {
                MessageBox.Show("전화번호 입력");
                iohpBox.Focus();
                return;
            }
            if (ionameBox.Text == "")
            {
                MessageBox.Show("이름 입력");
                ionameBox.Focus();
                return;
            }

            try
            {
                String iomacp;
                if (iomac.Checked == true)
                {
                    iomacp = "Y";
                }
                else
                {
                    iomacp = "N";
                }
                String iochzp;
                if (iochz.Checked == true)
                {
                    iochzp = "Y";
                }
                else
                {
                    iochzp = "N";
                }
                String iocakep;
                if (iocake.Checked == true)
                {
                    iocakep = "Y";
                }
                else
                {
                    iocakep = "N";
                }

                int k = dataGridView1.RowCount;
                if (k > 0)
                {
                    String sql1 = " insert into gbj(iohp, ioname, iodate, ioseq, iomac, iochz, iocake, ioftot) " +
                                  " values(@iohp, @ioname, @iodate, @ioseq, @iomac, @iochz, @iocake, @ioftot) ";

                    if (reader != null) reader.Close();
                    cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = sql1;

                    cmd.Parameters.AddWithValue("@iohp", (iohpBox.Text));
                    cmd.Parameters.AddWithValue("@ioname", (ionameBox.Text));
                    cmd.Parameters.AddWithValue("@iodate", (iodateBox.Text));
                    cmd.Parameters.AddWithValue("@ioseq", int.Parse(ioseqBox.Text));
                    cmd.Parameters.AddWithValue("@iomac", (iomacp));
                    cmd.Parameters.AddWithValue("@iochz", (iochzp));
                    cmd.Parameters.AddWithValue("@iocake", (iocakep));
                    cmd.Parameters.AddWithValue("@ioftot", int.Parse(ioftotBox.Text));

                    reader = cmd.ExecuteReader();


                    String tknd;
                    int tprice = 0;
                    int tcnt = 0;
                    int tamt = 0;

                    int i = 0;
                    while (i < k)
                    {
                        tknd = (dataGridView1.Rows[i].Cells[0].Value.ToString());
                        tprice = int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        tcnt = int.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        tamt = int.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());


                        String sql2 = " insert into gbj2(io2hp, io2date, io2seq, ioknd, ioprice, iocnt, ioamt) " +
                                      " values(@io2hp, @io2date, @io2seq, @ioknd, @ioprice, @iocnt, @ioamt) ";

                        if (reader != null) reader.Close();
                        cmd1 = new MySqlCommand();
                        cmd1.Connection = conn;
                        cmd1.CommandText = sql2;

                        cmd1.Parameters.AddWithValue("@io2hp", (iohpBox.Text));
                        cmd1.Parameters.AddWithValue("@io2date", (iodateBox.Text));
                        cmd1.Parameters.AddWithValue("@io2seq", int.Parse(ioseqBox.Text));
                        cmd1.Parameters.AddWithValue("@ioknd", (tknd));
                        cmd1.Parameters.AddWithValue("@ioprice", (tprice));
                        cmd1.Parameters.AddWithValue("@iocnt", (tcnt));
                        cmd1.Parameters.AddWithValue("@ioamt", (tamt));

                        cmd1.ExecuteNonQuery();
                        i++;

                    }
                    MessageBox.Show("주문 되었습니다.");
                }
                else
                {
                    MessageBox.Show(ToString());
                }

            }
            catch (NullReferenceException ex1)
            {
                MessageBox.Show(ex1.ToString());
            }
            dataGridView1.Rows.Clear();
            int t5t = int.Parse(ioftotBox.Text);
            int tot = int.Parse(iototBox.Text);
            t5t = 0;
            tot = 0;
            if (iohpBox.Text != "")
            {
                iohpBox.Text = "";
                ionameBox.Text = "";
                ioseqBox.Text = "";
                iototBox.Text = tot.ToString();
                iomac.Checked = false;
                iochz.Checked = false;
                iocake.Checked = false;
                ioftotBox.Text = t5t.ToString();
            }
            i = 0;
            namt = 0;
        }

        private void iohpBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                hp_Display();
            }
        }

        private void iohpBox_Leave(object sender, EventArgs e)
        {
            hp_Display();
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (iohpBox.Text == "")
            {
                MessageBox.Show("전화번호 입력");
                return;
            }

            dataGridView1.Rows.Clear();
            AccBtn.Enabled = true;

            String sql1 = " select count(*) as cnt from gbj " +
                          " where iohp = @iohp1 and iodate = @iodate1 ";

            String sql2 = " select iohp, ioname, iodate, ioseq, iomac, iochz, iocake, ioftot from gbj " +
                          " where iohp = @iohp2 and " +
                          " iodate = @iodate2 and " +
                          " ioseq = 1 ";

            if (reader != null) reader.Close();
            cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql1;

            cmd.Parameters.AddWithValue("@iohp1", (iohpBox.Text));
            cmd.Parameters.AddWithValue("@iodate1", (iodateBox.Text));

            reader = cmd.ExecuteReader();

            reader.Read();
            int aa = int.Parse(reader["cnt"].ToString());

            if (aa == 0)
            {
                MessageBox.Show("조회 할 데이터가 없습니다.");
                return;
            }

            if (aa == 1)
            {
                if (reader != null) reader.Close();
                cmd1 = new MySqlCommand();
                cmd1.Connection = conn;
                cmd1.CommandText = sql2;

                cmd1.Parameters.AddWithValue("@iohp2", (iohpBox.Text));
                cmd1.Parameters.AddWithValue("@iodate2", (iodateBox.Text));

                reader = cmd1.ExecuteReader();
                reader.Read();

                int amac = 0;
                int achz = 0;
                int acake = 0;
                String tmac = (reader["iomac"].ToString());
                if (tmac == "Y")
                {
                    iomac.Checked = true;
                    amac = 20000;
                }
                else
                {
                    iomac.Checked = false;
                    amac = 0;
                }

                String tchz = (reader["iochz"].ToString());
                if (tchz == "Y")
                {
                    iochz.Checked = true;
                    achz = 10000;
                }
                else
                {
                    iochz.Checked = false;
                    achz = 0;
                }

                String tcake = (reader["iocake"].ToString());
                if (tcake == "Y")
                {
                    iocake.Checked = true;
                    acake = 5000;
                }
                else
                {
                    iocake.Checked = false;
                    acake = 0;
                }

                ioptotBox.Text = (amac + achz + acake).ToString();

                ioftotBox.Text = reader["ioftot"].ToString();    //결재금액

                ioseqBox.Text = "1";

                String sql3 = " select io2hp, io2date, io2seq, ioknd, ioprice, iocnt, ioamt from gbj2 " +
                              " where io2hp=@io2hp and io2date=@io2date and io2seq=1 ";

                if (reader != null) reader.Close();
                cmd2 = new MySqlCommand();
                cmd2.Connection = conn;
                cmd2.CommandText = sql3;

                cmd2.Parameters.AddWithValue("@io2hp", (iohpBox.Text));
                cmd2.Parameters.AddWithValue("@io2date", (iodateBox.Text));

                reader = cmd2.ExecuteReader();

                int k = 0;
                int tot = 0;
                while (reader.Read())
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[k].Cells[0].Value = reader["ioknd"].ToString();
                    dataGridView1.Rows[k].Cells[1].Value = reader["ioprice"].ToString();
                    dataGridView1.Rows[k].Cells[2].Value = reader["iocnt"].ToString();
                    dataGridView1.Rows[k].Cells[3].Value = reader["ioamt"].ToString();

                    k++;

                    tot = tot + int.Parse(reader["ioamt"].ToString());
                }
                iototBox.Text = tot.ToString();
                if (aa >= 1)
                {
                    AccBtn.Enabled = true;
                }
                calc();
            }
        }

        private void AccBtn_Click(object sender, EventArgs e)
        {
            String sql1 = " delete from gbj " +
                          " where iohp = @iohp1 and " +
                          " iodate = @iodate1 and " +
                          " ioseq = 1 ";

            String sql2 = " delete from gbj2 " +
                          " where io2hp = @io2hp1 and " +
                          " io2date = @io2date1 and " +
                          " io2seq = 1 ";

            if (reader != null) reader.Close();
            cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql1;

            cmd.Parameters.AddWithValue("@iohp1", (iohpBox.Text));
            cmd.Parameters.AddWithValue("@iodate1", (iodateBox.Text));

            reader = cmd.ExecuteReader();

            if (reader != null) reader.Close();
            cmd1 = new MySqlCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = sql2;

            cmd1.Parameters.AddWithValue("@io2hp1", (iohpBox.Text));
            cmd1.Parameters.AddWithValue("@io2date1", (iodateBox.Text));

            cmd1.ExecuteNonQuery();
            MessageBox.Show("주문 삭제");

            AccBtn.Enabled = false;
            clearx();
        }

        private void clearx()
        {
            dataGridView1.Rows.Clear();
            int tot = int.Parse(iototBox.Text);
            tot = 0;
            if (iohpBox.Text != "")
            {
                iohpBox.Text = "";
                ionameBox.Text = "";
                ioseqBox.Text = "";
                iototBox.Text = tot.ToString();
                ioftotBox.Text = tot.ToString();
                iomac.Checked = false;
                iochz.Checked = false;
                iocake.Checked = false;
                ionones.Checked = true;
                iotels.Checked = false;
                iomems.Checked = false;
            }
        }
    }
}
