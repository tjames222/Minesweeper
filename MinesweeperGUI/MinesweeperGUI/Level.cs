using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesweeperGUI
{
    public partial class Level : Form
    {
        public Level()
        {
            InitializeComponent();
        }

        private void Level_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            int diff = 0;
            int size = 5;

            if (radioButton1.Checked)
            {
               diff = 1;
            }
            else if (radioButton2.Checked)
            {
                diff = 2;
            }
            else if (radioButton3.Checked)
            {
                diff = 3;
            }

            if (radioButton4.Checked)
            {
                size = 5;
            }
            else if (radioButton5.Checked)
            {
                size = 10;
            }
            else if (radioButton6.Checked)
            {
                size = 20;
            }

            Form1 f1 = new Form1(diff, size);

            f1.ShowDialog();
        }
    }
}
