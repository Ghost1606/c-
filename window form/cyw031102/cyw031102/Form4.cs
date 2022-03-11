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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double t1 = 0;
            double t2 = 0;
            double t3 = 0;

            t1 = double.Parse(textBox1.Text);
            t2 = double.Parse(textBox2.Text);

            t3 = t1 * t2;

            if (t1 >= 10)
            {
                if (t1 >= 20)
                {
                    
                }
                else
                {

                }
            }
            else
            {
                textBox3.Text = t3.ToString();
            }
        }
    }
}
