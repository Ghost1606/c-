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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double t1 = 0; //키
            double t2 = 0; //몸무게
            double p1 = 0; //표준몸무게
            double b1 = 0; //비만도

            t1 = int.Parse(textBox1.Text);
            t2 = int.Parse(textBox2.Text);

            if(t1 >= 160)
            {
                p1 = (t1 - 100) * 0.9;
            }
            else
            {
                if(t1 >= 150 )
                {
                    p1 = (t1 - 150) / 2 + 50;
                }
                else
                {
                    p1 = (t1 - 100);
                }
            }

            //

            b1 = (t2 - p1) * 100 / p1;

            textBox3.Text = b1.ToString();

            if(b1 <= 10)
            {
                textBox4.Text = "정상";
            }
            else
            {
                if (b1 >= 20)
                {
                    textBox4.Text = "비만";
                }
                else
                {
                    textBox4.Text = "과체중";
                }
            }
        }
    }
}
