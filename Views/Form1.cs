using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrawNassiProject.Models;
using DrawNassiProject.ViewModels;
using DrawNassiProject.Views;

namespace DrawNassiProject
{
    public partial class DrawNassi : Form
    {
        #region Initialization
        public Color clr = Color.White;
        public Color fontColor = Color.Black;
        public Color workingPlaceColor = Color.White;
        public Font font = new Font(FontFamily.GenericSerif, 8, FontStyle.Regular);
        Button btmp;
        Bitmap newbtmp;
        SolidBrush drawBrush = new SolidBrush(Color.Black);
        StringFormat drawFormat = new StringFormat();
        static Bitmap bitmap = new Bitmap(52, 28);
        Graphics graf = Graphics.FromImage(bitmap);
        BlockViewModel blockView = new BlockViewModel();
        bool dragging = false;
        public DrawNassi()
        {
            InitializeComponent();

            label2.Hide();
            textBox2.Hide();

            vScrollBar1.Hide();
            hScrollBar1.Hide();

            button1.AllowDrop = true;
            button2.AllowDrop = true;
            button3.AllowDrop = true;
            button4.AllowDrop = true;
            button5.AllowDrop = true;
            button6.AllowDrop = true;

            pictureBox7.AllowDrop = true;
            //pictureBox7.DragDrop += All_DragDrop;
            pictureBox7.DragOver += All_DragOver;
            pictureBox7.DragEnter += All_DragEnter;

            button1.DragDrop += Operation_DragDrop;
            //button2.DragDrop += All_DragDrop;
            //button3.DragDrop += All_DragDrop;
            //button4.DragDrop += All_DragDrop;
            //button5.DragDrop += All_DragDrop;
            //button6.DragDrop += All_DragDrop;

            button1.MouseDown += All_MouseDown;
            button2.MouseDown += All_MouseDown;
            button3.MouseDown += All_MouseDown;
            button4.MouseDown += All_MouseDown;
            button5.MouseDown += All_MouseDown;
            button6.MouseDown += All_MouseDown;

            button1.MouseMove += All_MouseMove;
            button2.MouseMove += All_MouseMove;
            button3.MouseMove += All_MouseMove;
            button4.MouseMove += All_MouseMove;
            button5.MouseMove += All_MouseMove;
            button6.MouseMove += All_MouseMove;
            newbtmp = new Bitmap(pictureBox7.Width, pictureBox7.Height);
            DrawFirst(Color.White, drawBrush.Color, 52, 28, "");
            button1.BackgroundImage = bitmap;
            DrawSecond(Color.White, 52, 28);
            button2.BackgroundImage = bitmap;
            DrawThird(Color.White, 52, 28);
            button3.BackgroundImage = bitmap;
            DrawFouth(Color.White, 52, 28);
            button4.BackgroundImage = bitmap;
            DrawFifth(Color.White, 52, 28);
            button5.BackgroundImage = bitmap;
            DrawSixth(Color.White, 52, 28);
            button6.BackgroundImage = bitmap;
            button7.ForeColor = Color.White;
            drawFormat.Trimming = StringTrimming.Word;
            drawFormat.FormatFlags = drawFormat.FormatFlags | StringFormatFlags.NoWrap;
            drawFormat.Alignment = StringAlignment.Center;
        }
        #endregion
        #region Loading and Saving
        private void DrawNassi_Load(object sender, EventArgs e)
        {
            openFileDialog1.FileName = @"Nassi.txt";
            saveFileDialog1.FileName = $"{textBox1.Text.Replace(".", "_")}";
            openFileDialog1.Filter =
                     "Текстовые файлы (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.Filter =
                     "Точечный рисунок (*.png)|*.png|All files (*.*)|*.*";
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == String.Empty) return;
            // Чтение текстового файла
            try
            {
                var reader = new System.IO.StreamReader(
                openFileDialog1.FileName, Encoding.GetEncoding(1251));
                // тут в будущем будет код

                reader.Close();
            }
            catch (System.IO.FileNotFoundException situation)
            {
                MessageBox.Show(situation.Message + "\nНет такого файла",
"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception situation)
            { // отчет о других ошибках
                MessageBox.Show(situation.Message,
                     "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (pictureBox7.BackgroundImage != null)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pictureBox7.BackgroundImage.Save(saveFileDialog1.FileName);
                    textBox1.Text = saveFileDialog1.FileName;
                    textBox1.Width = textBox1.Text.Length * 7;
                }
            }
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
        }
        #endregion
        #region DebugMode
        private void включитьToolStripMenuItem_Click(object sender, EventArgs e) { textBox2.Show(); label2.Show(); }
        private void выключитьToolStripMenuItem_Click(object sender, EventArgs e) { textBox2.Hide(); label2.Hide(); }
        #endregion
        #region ColorChoise
        private void button7_Click(object sender, EventArgs e)
        {
            ReDrawBlock();
        }
        private void ReDrawBlock()
        {
            colorDialog1.ShowDialog();
            clr = colorDialog1.Color;
            DrawFirst(clr, drawBrush.Color, 52, 28, "");
            button1.BackgroundImage = bitmap;
            DrawSecond(clr, 52, 28);
            button2.BackgroundImage = bitmap;
            DrawThird(clr, 52, 28);
            button3.BackgroundImage = bitmap;
            DrawFouth(clr, 52, 28);
            button4.BackgroundImage = bitmap;
            DrawFifth(clr, 52, 28);
            button5.BackgroundImage = bitmap;
            DrawSixth(clr, 52, 28);
            button6.BackgroundImage = bitmap;
            button7.ForeColor = clr;
            foreach (Block block1 in blockView.blocks)
            {
                if (block1.type == 0)
                {
                    OperationBlock operationBlock = block1 as OperationBlock;
                    operationBlock.Draw(this);
                    graf = Graphics.FromImage(newbtmp);
                    graf.DrawImage(bitmap, operationBlock.blockInternalX, operationBlock.blockInternalY);
                }
            }
            pictureBox7.BackgroundImage = newbtmp;
            pictureBox7.Refresh();
        }
        private void шрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            font = fontDialog1.Font;
            newbtmp = SizeCheck(newbtmp);
            newbtmp = Clear(newbtmp);
            RefreshBlocks();
        }
        private void цветТекстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog2.ShowDialog();
            fontColor = colorDialog2.Color;
            drawBrush = new SolidBrush(fontColor);
            newbtmp = SizeCheck(newbtmp);
            newbtmp = Clear(newbtmp);
            RefreshBlocks();
        }
        private void цветРабочегоПоляToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog3.ShowDialog();
            workingPlaceColor = colorDialog3.Color;
            pictureBox7.BackColor = workingPlaceColor;
            newbtmp = SizeCheck(newbtmp);
            newbtmp = Clear(newbtmp);
            RefreshBlocks();
        }
        private void цветБлоковToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReDrawBlock();
        }
        #endregion
        #region Drawing
        public void DrawFirst(Color color, Color fontColor,  int width, int height , string text)
        {
            DrawRectangle(color, width, height);
            graf.DrawString(text, font, new SolidBrush(fontColor), width/2, height/2-font.Size, drawFormat);
        }
        public void DrawRectangle(Color color, int width, int height)
        {
            bitmap = new Bitmap(width, height);
            graf = Graphics.FromImage(bitmap);
            graf.FillRectangle(new SolidBrush(color), 1, 1, width - 1, height - 1);
            graf.DrawRectangle(new Pen(Color.Black, 1), 0, 0, width - 1, height - 1);
        }
        public void DrawSecond(Color color, int width, int height)
        {
            DrawRectangle (color, width, height);
            graf.DrawLine(new Pen(Color.Black, 1), new Point(0, 0), new Point(width/2, height/2));
            graf.DrawLine(new Pen(Color.Black, 1), new Point(width/2, height/2), new Point(width, 0));
            graf.DrawLine(new Pen(Color.Black, 1), new Point(0, height/2), new Point(width, height/2));
            graf.DrawLine(new Pen(Color.Black, 1), new Point(width/2, height/2), new Point(width/2, height));
        }
        public void DrawThird(Color color, int width, int height)
        {
            DrawRectangle(color, width, height);
            graf.DrawLine(new Pen(Color.Black, 1), new Point(0, 0), new Point(width - width/4, height/2));
            graf.DrawLine(new Pen(Color.Black, 1), new Point(width - width / 4, height/2), new Point(width, 0));
            graf.DrawLine(new Pen(Color.Black, 1), new Point(0, height/2), new Point(width, height / 2));
            graf.DrawLine(new Pen(Color.Black, 1), new Point(width - width / 4, height/2), new Point(width - width / 4, height));
        }
        public void DrawFouth(Color color, int width, int height)
        {
            DrawRectangle(color, width, height);
            graf.DrawLine(new Pen(Color.Black, 1), new Point(width/4, height), new Point(width/4, height/2));
            graf.DrawLine(new Pen(Color.Black, 1), new Point(width/4, height/2), new Point(width, height/2));
        }
        public void DrawFifth(Color color, int width, int height)
        {
            DrawRectangle(color, width, height);
            graf.DrawLine(new Pen(Color.Black, 1), new Point(width/4, 0), new Point(width/4, height/2));
            graf.DrawLine(new Pen(Color.Black, 1), new Point(width/4, height/2), new Point(width, height/2));
        }
        public void DrawSixth(Color color, int width, int height)
        {
            DrawRectangle(color, width, height);
            graf.FillRectangle(new SolidBrush(Color.Gray), 1, 1, height/2-2, width/2);
            graf.DrawLine(new Pen(Color.Black, 1), new Point(width / 4, 0), new Point(width / 4, height));
            graf.FillRectangle(new SolidBrush(Color.Gray), width - width/4, 1, height / 2 - 2, width / 2);
            graf.DrawLine(new Pen(Color.Black, 1), new Point(width - width / 4, 0), new Point(width - width / 4, height));

        }
        #endregion
        #region DraggingPanel
        private void All_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                btmp = (Button)sender;
                btmp.DoDragDrop(btmp.BackgroundImage, DragDropEffects.Copy);
            }
        }
        private void All_MouseMove(object sender, MouseEventArgs e)
        {
        }
        static byte id;

        private void All_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        static int i = 0;
        private void Operation_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            blockView.CreateBlock(id, clr, drawBrush.Color, PointToClient(new Point(e.X - 13, e.Y - 20)).X, PointToClient(new Point(e.X - 13, e.Y - 20)).Y, this, 52, 28, $"Block {++i}");
            graf = Graphics.FromImage(newbtmp);
            graf.DrawImage(bitmap, PointToClient(new Point(e.X - 13, e.Y - 20)));
            pictureBox7.BackgroundImage = newbtmp;
            pictureBox7.Refresh();
            textBox2.Text = blockView.blocks.Count.ToString();
        }

        private void All_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        private void button1_DragLeave(object sender, EventArgs e)
        {
            id = 0;
        }
        private void button2_MouseLeave(object sender, EventArgs e)
        {
            id = 1;
        }
        private void button3_DragLeave(object sender, EventArgs e)
        {
            id = 2;
        }
        private void button4_DragLeave(object sender, EventArgs e)
        {
            id = 3;
        }

        private void button5_DragLeave(object sender, EventArgs e)
        {
            id = 4;
        }
        private void button6_DragLeave(object sender, EventArgs e)
        {
            id = 5;
        }
        #endregion
        #region Moving
        Block mainblock;
        int xR;
        int yR;
        private void pictureBox7_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var block in blockView.blocks)
            {
                dragging = true;
                pictureBox7.Focus();
                if (block.blockInternalX <= e.X && block.blockInternalY <= e.Y
                    && block.Width + block.blockInternalX >= e.X && block.Height + block.blockInternalY >= e.Y)
                {
                    xR = e.X - block.blockInternalX;
                    yR = e.Y - block.blockInternalY;
                    mainblock = block;
                    Cursor = Cursors.Hand;
                    break;
                }
            }
        }

        private void pictureBox7_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging && mainblock != null)
            {
                newbtmp = SizeCheck(newbtmp);
                newbtmp = Clear(newbtmp);
                blockView.SetPosition(mainblock, this, e.X - xR, e.Y - yR, mainblock.type);
                graf = Graphics.FromImage(newbtmp);
                RefreshBlocks();
            }
        }
        private void RefreshBlocks()
        {
            graf = Graphics.FromImage(newbtmp);
            foreach (Block block1 in blockView.blocks)
            {
                if (block1.type == 0)
                {
                    OperationBlock operationBlock = block1 as OperationBlock;
                    operationBlock.Draw(this);
                    graf = Graphics.FromImage(newbtmp);
                    graf.DrawImage(bitmap, operationBlock.blockInternalX, operationBlock.blockInternalY);
                }
                else if (block1.type == 1)
                {
                    BranchingBlock branchingBlock = block1 as BranchingBlock;
                    branchingBlock.Draw(this);
                    graf = Graphics.FromImage(newbtmp);
                    graf.DrawImage(bitmap, branchingBlock.blockInternalX, branchingBlock.blockInternalY);
                }
                else if (block1.type == 2)
                {
                    WhileBlock whileBlock = block1 as WhileBlock;
                    whileBlock.Draw(this);
                    graf = Graphics.FromImage(newbtmp);
                    graf.DrawImage(bitmap, whileBlock.blockInternalX, whileBlock.blockInternalY);
                }
                else if (block1.type == 3)
                {
                    WhilePreBlock whilepreBlock = block1 as WhilePreBlock;
                    whilepreBlock.Draw(this);
                    graf = Graphics.FromImage(newbtmp);
                    graf.DrawImage(bitmap, whilepreBlock.blockInternalX, whilepreBlock.blockInternalY);
                }
                else if (block1.type == 4)
                {
                    WhilePostBlock whilePostBlock = block1 as WhilePostBlock;
                    whilePostBlock.Draw(this);
                    graf = Graphics.FromImage(newbtmp);
                    graf.DrawImage(bitmap, whilePostBlock.blockInternalX, whilePostBlock.blockInternalY);
                }
                else if (block1.type == 5)
                {
                    Subroutine subroutine = block1 as Subroutine;
                    subroutine.Draw(this);
                    graf = Graphics.FromImage(newbtmp);
                    graf.DrawImage(bitmap, subroutine.blockInternalX, subroutine.blockInternalY);
                }
            }
            pictureBox7.BackgroundImage = newbtmp;
            pictureBox7.Refresh();
        }
        private Bitmap Clear(Bitmap bitmap)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            graf = Graphics.FromImage(bitmap);
            graf.FillRectangle(new SolidBrush(workingPlaceColor), 0, 0, bitmap.Width, bitmap.Height);
            textBox2.Text = blockView.blocks.Count.ToString();
            return bitmap;
        }
        private void pictureBox7_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            Cursor = Cursors.Default;
            mainblock = null;
            Focus();
        }
        private Bitmap SizeCheck(Bitmap newbtmppre)
        {
            if (newbtmp.Width < pictureBox7.Width && newbtmp.Height < pictureBox7.Height)
            {
                newbtmppre = new Bitmap(pictureBox7.Width, pictureBox7.Height);
            }
            else if (newbtmp.Width < pictureBox7.Width)
            {
                newbtmppre = new Bitmap(pictureBox7.Width, newbtmp.Height);
            }
            else if (newbtmp.Height < pictureBox7.Height)
            {
                newbtmppre = new Bitmap(newbtmp.Width, pictureBox7.Height);
            }
            return newbtmppre;
        }
        private void DrawNassi_SizeChanged(object sender, EventArgs e)
        {
            Bitmap newbtmppre = new Bitmap(newbtmp);
            newbtmppre = SizeCheck(newbtmppre);
            graf = Graphics.FromImage(newbtmppre);
            graf.DrawImage(newbtmp, 0, 0);
            pictureBox7.BackgroundImage = newbtmppre;
            pictureBox7.Refresh();
            newbtmp = newbtmppre;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            blockView.blocks.Clear();
            newbtmp = Clear(newbtmp);
            pictureBox7.BackgroundImage = newbtmp;
            pictureBox7.Refresh();
        }
        #endregion
        #region KeyCheck
        private void pictureBox7_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Delete && mainblock != null && dragging)
            {
                blockView.blocks.Remove(mainblock);
                mainblock = null;
                dragging = false;
                Cursor = Cursors.Default;
                newbtmp = SizeCheck(newbtmp);
                newbtmp = Clear(newbtmp);
                RefreshBlocks();
                Focus();
            }
        }
        #endregion
        #region InBlockChanging
        SetText form;
        private void pictureBox7_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (var block in blockView.blocks)
            {
                if (block.blockInternalX <= e.X && block.blockInternalY <= e.Y
                    && block.Width + block.blockInternalX >= e.X && block.Height + block.blockInternalY >= e.Y)
                {
                    form = new SetText(this, block);
                    form.ShowDialog();
                    block.text = form.Block.text;
                    block.blockInternalColor = form.Block.blockInternalColor;
                    newbtmp = SizeCheck(newbtmp);
                    newbtmp = Clear(newbtmp);
                    graf = Graphics.FromImage(newbtmp);
                    RefreshBlocks();
                    break;
                }
            }
        }
        #endregion
    }
}
