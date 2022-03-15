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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int t1 = 0;
            ;

            t1 = int.Parse(textBox1.Text);
            

            if(t1%2 == 0)
            {
                textBox2.Text = t1.ToString("짝수입니다");
            }
            else
            {
                textBox2.Text = t1.ToString("홀수입니다");
            }
        }
    }
}
