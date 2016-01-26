using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form2 : Form
    {
        
        public Form2()
        {
            
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int field_size = (int)numericUpDown1.Value;
            Form1 f1 = new Form1(field_size);
            f1.Show();
            this.Hide();
        }
    }
}
