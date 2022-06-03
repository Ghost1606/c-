using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; 
using System.Data.SqlClient;  

namespace _201987043cyw
{
    public partial class Form1 : Form
    {
        public static String mysql_str = "server=127.0.0.1;port=3307;Database=cyw2;Uid=root;Pwd=1234;Charset=utf8"; //로컬호스트,설정포트,DB이름,루트,비밀번호,한글설정
        MySqlCommand cmd; //sql문장을 실행시킬때
        MySqlCommand cmd1; //sql문장을 실행시킬때
        MySqlCommand cmd2;
        MySqlDataReader reader; //sql 문장을 실행시키고 결과받을때
        MySqlConnection conn = new MySqlConnection(mysql_str); //연결 정보 객체 생성

        int i = 0;
        int namt = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();//DB서버 오픈
            }
            iodt.Text = DateTime.Now.ToString("yyyyMMdd"); //오늘 날짜
        }
        private void hp_display()
        {
            String sql1 = " select ordname from ordcyw1 " +
                          " where ordohp=@ordohp1 " +
                          " order by (orddat+ordseq) desc limit 1";

            if (reader != null) reader.Close();

            cmd = new MySqlCommand(); //cmd sql위한 준비작업
            cmd.Connection = conn;
            cmd.CommandText = sql1; //실행시킬 sql문장이 무엇인지 지정
            cmd.Parameters.AddWithValue("@ordohp1", iohp.Text);

            reader = cmd.ExecuteReader();

            if (reader.Read() == true)
            {
                ionm.Text = (String)reader["ordname"];
                ioseq.ReadOnly = true;
            }
            else
            {
                ionm.Text = "";
                ioseq.ReadOnly = false;
            }

            String sql2 = " select IFNULL(max(ordseq),0) as maxseq from ordcyw1 " +
                         " where ordohp=@ordohp2 and orddat = @orddat2";

            if (reader != null) reader.Close();

            cmd1 = new MySqlCommand(); //cmd sql위한 준비작업
            cmd1.Connection = conn;
            cmd1.CommandText = sql2; //실행시킬 sql문장이 무엇인지 지정
            cmd1.Parameters.AddWithValue("@ordohp2", iohp.Text);
            cmd1.Parameters.AddWithValue("@orddat2", iodt.Text);

            reader = cmd1.ExecuteReader();

            int iseq = 0;

            if (reader.Read() == true)
            {
                iseq = ((int)reader["maxseq"] + 1);
                ioseq.Text = iseq.ToString();
            }
            else
            {
                ioseq.Text = "";

            }
        }

        private void iohp_Leave(object sender, EventArgs e)
        {
            hp_display();
        }

        private void iohp_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                hp_display();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                //중복체크
                //MessageBox.Show(dataGridView1.Rows.Count.ToString()); //그리드 메세지 박스 출력
                int j = 0;
                String schk = "N";
                while (j < dataGridView1.Rows.Count) //행 컬럼 있는거 -1 인덱스 번호가 0부터 시작
                {
                    if (dataGridView1.Rows[j].Cells[0].Value.ToString() == comboBox1.Text)
                    {
                        schk = "Y";
                    }
                    j++;
                }
                if (schk == "Y")
                {
                    MessageBox.Show(comboBox1.Text + "가 이미 선택되어 있습니다.");
                    return;
                }

                int famt = 0; //버거값
                int fqty = int.Parse(comboBox2.Text); //갯수
                int gamt = 0; //추가 옵션

                //커피 가격
                if (comboBox1.Text == "치즈버거세트") famt = 4500;
                if (comboBox1.Text == "치킨버거세트") famt = 5000;
                if (comboBox1.Text == "에그버거세트") famt = 4000;
                if (comboBox1.Text == "슈슈버거세트") famt = 7500;

                int total = famt * fqty + gamt; //금액
                namt = namt + total;

                dataGridView1.Rows.Add(); //행 하나 추가
                dataGridView1.Rows[i].Cells[0].Value = comboBox1.Text;
                dataGridView1.Rows[i].Cells[1].Value = famt;
                dataGridView1.Rows[i].Cells[2].Value = comboBox2.Text;
                dataGridView1.Rows[i].Cells[3].Value = total;
                i++;

                textBox4.Text = namt.ToString();

                amt_cal();
            }
        }
        private void amt_cal()
        {
            int ramt = 0;
            textBox6.Text = (namt + (int.Parse(textBox5.Text)) + ramt).ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int tmp_amt = 0;
            //int tmp_text4 = int.Parse(textBox4.Text);
            try
            {
                if (dataGridView1.CurrentRow.Index >= 0)
                {
                    tmp_amt = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value.ToString());
                    dataGridView1.Rows.Remove(dataGridView1.Rows[dataGridView1.CurrentRow.Index]);
                    i--;
                    namt = namt - tmp_amt;
                    textBox4.Text = namt.ToString();

                    amt_cal();
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("삭제할 데이터가 없습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            int tmp_amt = int.Parse(textBox5.Text);
            if (checkBox2.Checked == true)
            {
                tmp_amt += 20000;
            }
            else
            {
                tmp_amt -= 20000;
            }
            textBox5.Text = tmp_amt.ToString();
            amt_cal();
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            int tmp_amt = int.Parse(textBox5.Text);
            if (checkBox3.Checked == true)
            {
                tmp_amt += 10000;
            }
            else
            {
                tmp_amt -= 10000;
            }
            textBox5.Text = tmp_amt.ToString();
            amt_cal();
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            int tmp_amt = int.Parse(textBox5.Text);
            if (checkBox4.Checked == true)
            {
                tmp_amt += 5000;
            }
            else
            {
                tmp_amt -= 5000;
            }
            textBox5.Text = tmp_amt.ToString();
            amt_cal();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            int imak = 0; //맥너겟
            int ichi = 0; //치즈스틱
            int ihot = 0; //핫케익
            // 텍스트박스 비어있는지 확인
            if (iohp.Text == "")
            {
                MessageBox.Show("전화번호를 입력하세요!");
                return;
            }
            if (ionm.Text == "")
            {
                MessageBox.Show("주문자성명을 입력하세요!");
                return;
            }
            if (iodt.Text == "")
            {
                MessageBox.Show("주문날짜를 입력하세요!");
                return;
            }
            // 그리드뷰에 버거가 1개이상 주문되어 있는지 확인
            if (dataGridView1.RowCount <= 0)
            {
                MessageBox.Show("버거를 추가해주세요!");
                return;
            }

            if (checkBox2.Checked == true)
            {
                imak += 20000;
            }
            else
            {
                imak -= 0;
            }

            if (checkBox3.Checked == true)
            {
                ichi += 10000;
            }
            else
            {
                ichi -= 0;
            }

            if (checkBox4.Checked == true)
            {
                ihot += 5000;
            }
            else
            {
                ihot -= 0;
            }

            String sql1 = "INSERT INTO ordcyw1(ordohp, ordname, orddat, ordseq, ordmak, ordchi, ordhot, ordamt) " +
                          "VALUES(@ordohp, @ordname, @orddat, @ordseq, @ordmak, @ordchi, @ordhot, @ordamt )";

            if (reader != null) reader.Close();

            cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql1;
            cmd.Parameters.AddWithValue("@ordohp", iohp.Text); //seq
            cmd.Parameters.AddWithValue("@ordname", ionm.Text);
            cmd.Parameters.AddWithValue("@orddat", iodt.Text);
            cmd.Parameters.AddWithValue("@ordseq", ioseq.Text);

            cmd.Parameters.AddWithValue("@ordmak", imak);
            cmd.Parameters.AddWithValue("@ordchi", ichi);
            cmd.Parameters.AddWithValue("@ordhot", ihot);
            cmd.Parameters.AddWithValue("@ordamt", int.Parse(textBox6.Text));
            cmd.ExecuteNonQuery(); //ordcyw1

            int i = 0;
            int rowcnt = dataGridView1.RowCount; //그리드갯수

            String iknd; //버거종류
            int iamt = 0; //버거값
            int iqty = 0; //개수
            int itot = 0; //전체버거값

            String sql2 = "INSERT INTO kndcyw1(kndohp, knddat, kndseq, kndknd, kndamt, kndqty, kndtot) " +
                          "VALUES(@kndohp, @knddat, @kndseq, @kndknd, @kndamt, @kndqty, @kndtot)";
            while (i < rowcnt)
            {
                iknd = dataGridView1.Rows[i].Cells[0].Value.ToString();
                iamt = int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                iqty = int.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                itot = int.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());

                if (reader != null) reader.Close();

                cmd2 = new MySqlCommand();
                cmd2.Connection = conn;
                cmd2.CommandText = sql2;
                cmd2.Parameters.AddWithValue("@kndohp", iohp.Text); //seq
                cmd2.Parameters.AddWithValue("@knddat", iodt.Text);
                cmd2.Parameters.AddWithValue("@kndseq", int.Parse(ioseq.Text));

                cmd2.Parameters.AddWithValue("@kndknd", iknd);
                cmd2.Parameters.AddWithValue("@kndamt", iamt);
                cmd2.Parameters.AddWithValue("@kndqty", iqty);
                cmd2.Parameters.AddWithValue("@kndtot", itot);
                cmd2.ExecuteNonQuery();
                i++;
            }
            MessageBox.Show("주문이 완료되었습니다 !");
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (iohp.Text == "")
            {
                MessageBox.Show("전화번호를 입력하세요!");
                return;
            }
            String sql1 = "select count(*) as cnt from ordcyw1 " +
                          "where ordohp = @ordohp1 and orddat = @orddat1";

            String sql2 = "select ordohp,ordname,orddat,ordseq,ordmak,ordchi,ordhot,ordamt from ordcyw1 " +
                         "where ordohp = @ordohp2 and " +
                         "      orddat = @orddat2 and " +
                         "      ordseq = 1";

            if (reader != null) reader.Close();

            cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql1;
            cmd.Parameters.AddWithValue("@ordohp1", iohp.Text); //seq
            cmd.Parameters.AddWithValue("@orddat1", iodt.Text);
            cmd.ExecuteNonQuery(); 

            reader = cmd.ExecuteReader();

            reader.Read();
            int aa = int.Parse(reader["cnt"].ToString());

            if (aa == 1)  //해당 날짜에 주문전화번호가 1건
            {
                if (reader != null) reader.Close();

                cmd1 = new MySqlCommand();
                cmd1.Connection = conn;
                cmd1.CommandText = sql2;
                cmd1.Parameters.AddWithValue("@ordohp2", iohp.Text); //seq
                cmd1.Parameters.AddWithValue("@orddat2", iodt.Text);
                cmd1.ExecuteNonQuery(); //corder

                reader = cmd1.ExecuteReader();
                reader.Read();

                int temp_mak = int.Parse(reader["ordmak"].ToString());
                if (temp_mak > 0)
                {
                    checkBox2.Checked = true;
                }
                else
                {
                    checkBox2.Checked = false;
                }
                int temp_chi = int.Parse(reader["ordchi"].ToString());
                if (temp_chi > 0)
                {
                    checkBox3.Checked = true;
                }
                else
                {
                    checkBox3.Checked = false;
                }
                int temp_hot = int.Parse(reader["ordhot"].ToString());
                if (temp_hot > 0)
                {
                    checkBox4.Checked = true;
                }
                else
                {
                    checkBox4.Checked = false;
                }

                textBox5.Text = (temp_mak + temp_chi + temp_hot).ToString();
                textBox6.Text = reader["ordamt"].ToString(); //전체금액
                ioseq.Text = "1"; //seq를 1로 고정  

                String sql3 = "select kndohp, knddat, kndseq, kndknd, kndamt, kndqty, kndtot from kndcyw1 " +
                              "where kndohp = @kndohp3 and knddat = @knddat3 and kndseq = @kndseq3";

                if (reader != null) reader.Close();

                cmd2 = new MySqlCommand();
                cmd2.Connection = conn;
                cmd2.CommandText = sql3;
                cmd2.Parameters.AddWithValue("@kndohp3", iohp.Text); //seq
                cmd2.Parameters.AddWithValue("@knddat3", iodt.Text);
                cmd2.Parameters.AddWithValue("@kndseq3", int.Parse(ioseq.Text));
                cmd2.ExecuteNonQuery(); //corder

                reader = cmd2.ExecuteReader();

                int k = 0;
                int tot = 0;
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(); //행 하나 추가
                    dataGridView1.Rows[k].Cells[0].Value = reader["kndknd"].ToString();
                    dataGridView1.Rows[k].Cells[1].Value = reader["kndamt"].ToString();
                    dataGridView1.Rows[k].Cells[2].Value = reader["kndqty"].ToString();
                    dataGridView1.Rows[k].Cells[3].Value = reader["kndtot"].ToString();
                    k++;
                    tot = tot + int.Parse(reader["kndtot"].ToString());
                }
                textBox4.Text = tot.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            String sql1 = "delete from ordcyw1 " +
                          "where ordohp = @ordohp1 and " +
                          "      orddat = @orddat1 and " +
                          "      ordseq = 1";
            String sql2 = "delete from kndcyw1 " +
                          "where kndohp = @kndohp1 and " +
                          "      knddat = @knddat1 and " +
                          "      kndseq = 1";

            if (reader != null) reader.Close();

            cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql1;
            cmd.Parameters.AddWithValue("@ordohp1", iohp.Text); //seq
            cmd.Parameters.AddWithValue("@orddat1", iodt.Text);
            cmd.ExecuteNonQuery(); 

            cmd1 = new MySqlCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = sql2;
            cmd1.Parameters.AddWithValue("@kndohp1", iohp.Text); //seq
            cmd1.Parameters.AddWithValue("@knddat1", iodt.Text);
            cmd1.ExecuteNonQuery(); 

            MessageBox.Show("주문이 삭제되었습니다!");
        }
        //할인율 계산
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            double tmp_amt = int.Parse(textBox6.Text);
            if (radioButton2.Checked == true)
            {
                tmp_amt *= 0.9;
                textBox6.Text = tmp_amt.ToString();
            }
            else
            {
                amt_cal();
            }
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            double tmp_amt = int.Parse(textBox6.Text);
            if (radioButton3.Checked == true)
            {
                tmp_amt *= 0.8;
                textBox6.Text = tmp_amt.ToString();
            }
            else
            {
                amt_cal();
            }
        }
    }
}
