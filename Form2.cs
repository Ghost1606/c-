using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cyw031102
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double t1 = 0; //손님수
            double t2 = 0; //1인당식사비
            double t3 = 0; //총식사비

            t1 = double.Parse(textBox1.Text);
            t2 = double.Parse(textBox2.Text);

            t3 = t1 = t2;

            if(t1 > 4) //손님수가 4이상일때
            {
                textBox3.Text = (t3 * 0.9).ToString();
            }
            else //손님수가 4미만일때
            {
                textBox3.Text = t3.ToString();
            }
        }
    }
}
