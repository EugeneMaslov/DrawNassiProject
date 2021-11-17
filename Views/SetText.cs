using DrawNassiProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrawNassiProject.Views
{
    public partial class SetText : Form
    {
        public Color clr;
        public Color fontColor;
        public Color contrColor;
        public Block Block;
        public SetText(DrawNassi drawNassi, Block block)
        {
            InitializeComponent();
            textBox1.Text = block.text;
            Block = block;
            clr = Block.blockInternalColor;
            fontColor = Block.fontInternalColor;
            contrColor = Block.contrInternalColor;
            colorDialog1.Color = clr;
            colorDialog2.Color = fontColor;
            colorDialog3.Color = contrColor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            clr = colorDialog1.Color;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Save();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            colorDialog2.ShowDialog();
            fontColor = colorDialog2.Color;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            colorDialog3.ShowDialog();
            contrColor = colorDialog3.Color;
        }
        private void Save()
        {
            Block.blockInternalColor = clr;
            Block.text = textBox1.Text;
            Block.fontInternalColor = fontColor;
            Block.contrInternalColor = contrColor;
            this.Close();
        }
        private void SetText_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Save();
            }
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Save();
            }
        }
    }
}
