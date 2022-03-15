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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int t1 = 0;

            t1 = int.Parse(textBox1.Text);
            


            if(t1 % 400 == 0)
            {
                textBox2.Text = t1.ToString("윤년");  
            }
            else
            {
                if(t1 % 4 == 0)
                {
                    if(t1 % 100 == 0)
                    {
                        textBox2.Text = t1.ToString("윤년");
                    }
                    else
                    {
                        textBox2.Text = t1.ToString("평년");
                    }
                }
                else
                {
                    textBox2.Text = t1.ToString("평년");
                }
            }
        }
    }
}
