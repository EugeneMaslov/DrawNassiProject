using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrawNassiProject.Models;
using DrawNassiProject.Models.Composite;
using DrawNassiProject.ViewModels;
using DrawNassiProject.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DrawNassiProject
{
    public partial class DrawNassi : Form
    {
        #region Initialization
        
        public Color clr = Color.White;
        public int maxWidth = 52;
        public Color fontColor = Color.Black;
        public Color workingPlaceColor = Color.White;
        public Color contrColor = Color.Black;
        public Bitmap bitmap = new Bitmap(52, 28);
        public Graphics graf;
        public BlockViewModel blockView = new BlockViewModel();
        public Font font = new Font(FontFamily.GenericSerif, 8, FontStyle.Regular);
        public Bitmap newbtmp;

        string serialized;
        bool dragging = false;
        Button btmp;
        SolidBrush drawBrush = new SolidBrush(Color.Black);
        StringFormat drawFormat = new StringFormat();
        
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
            pictureBox7.DragOver += All_DragOver;
            pictureBox7.DragEnter += All_DragEnter;

            button1.DragDrop += Operation_DragDrop;

            button1.MouseDown += All_MouseDown;
            button2.MouseDown += All_MouseDown;
            button3.MouseDown += All_MouseDown;
            button4.MouseDown += All_MouseDown;
            button5.MouseDown += All_MouseDown;
            button6.MouseDown += All_MouseDown;

            graf = Graphics.FromImage(bitmap);
            graf.SmoothingMode = SmoothingMode.HighQuality;
            newbtmp = new Bitmap(pictureBox7.Width, pictureBox7.Height);
            DrawFirst(Color.White, drawBrush.Color, contrColor, 52, 28, "");
            button1.BackgroundImage = bitmap;
            DrawSecond(Color.White, drawBrush.Color, contrColor, 52, 28, "", 0, false);
            button2.BackgroundImage = bitmap;
            DrawThird(Color.White, drawBrush.Color, contrColor, 52, 28, "", 0);
            button3.BackgroundImage = bitmap;
            DrawFouth(Color.White, drawBrush.Color, contrColor, 52, 28, "", 0);
            button4.BackgroundImage = bitmap;
            DrawFifth(Color.White, drawBrush.Color, contrColor, 52, 28, "", 0);
            button5.BackgroundImage = bitmap;
            DrawSixth(Color.White, drawBrush.Color, contrColor, 52, 28, "");
            button6.BackgroundImage = bitmap;
            button7.ForeColor = Color.White;
            drawFormat.Trimming = StringTrimming.Word;
            drawFormat.FormatFlags |= StringFormatFlags.NoWrap;
            drawFormat.Alignment = StringAlignment.Center;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "NewProject.chm");
        }
        #endregion
        #region Loading and Saving
        private void DrawNassi_Load(object sender, EventArgs e)
        {
            openFileDialog1.FileName = @"Diagram.json";
            saveFileDialog1.FileName = $"{textBox1.Text.Replace(".", "_")}";
            saveFileDialog2.FileName = $"{textBox1.Text}";
            openFileDialog1.Filter =
                     "Json Файл (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog1.Filter =
                     "Точечный рисунок (*.png)|*.png|All files (*.*)|*.*";
            saveFileDialog2.Filter =
                     "Json Файл (*.json)|*.json|All files (*.*)|*.*";
        }
        private void RecurtsRelise(List<Unit> groups)
        {
            foreach (var item in groups)
            {
                Rec(item);
                item.group = item;
            }
        }
        private void Rec(Unit item)
        {
            foreach (var block in item.Blocks)
            {
                if (block.type >= 1 && block.type <= 4)
                {
                    foreach (var news in block.subgroup)
                    {
                        BlocksRecusrstRelise(news);
                        Rec(news);
                    }
                }
                block.group = item;
            }
        }
        private void BlocksRecusrstRelise(Unit item)
        {
            foreach (var block in item.Blocks)
            {
                if (block.type >= 1 && block.type <= 4)
                {
                    foreach (var news in block.subgroup)
                    {
                        BlocksRecusrstRelise(news);
                    }
                }
                block.group = item;
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == String.Empty) return;
            // Чтение файла
            try
            {
                serialized = "";
                var settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                };
                blockView.groups = JsonConvert.DeserializeObject<List<Unit>>(Encoding.GetEncoding(1251).GetString(File.ReadAllBytes(openFileDialog1.FileName)), settings);
                RecurtsRelise(blockView.groups);
                blockView.blocks.Clear();
                RefreshGroups();
                graf.Dispose();
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
                }
            }
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            serialized = "";
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto
            };
            serialized = JsonConvert.SerializeObject(blockView.groups, settings);
            if (serialized.Length > 1)
            {
                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    if (!File.Exists(saveFileDialog2.FileName))
                    {
                        File.Create(saveFileDialog2.FileName).Close();
                    }
                    File.WriteAllText(saveFileDialog2.FileName, serialized, Encoding.GetEncoding(1251));
                    textBox1.Text = saveFileDialog2.FileName;
                    textBox1.Width = textBox1.Text.Length * 7;
                }
            }
        }
        #endregion
        #region DebugMode
        private void включитьToolStripMenuItem_Click(object sender, EventArgs e) { textBox2.Show(); label2.Show(); }
        private void выключитьToolStripMenuItem_Click(object sender, EventArgs e) { textBox2.Hide(); label2.Hide(); }
        #endregion
        #region ColorChoise
        private void button7_Click(object sender, EventArgs e) => ReDrawBlock();
        private void ReDrawBlock()
        {
            colorDialog1.Color = clr;
            colorDialog1.ShowDialog();
            clr = colorDialog1.Color;
            Deploy();
        }
        private void Deploy()
        {
            DrawFirst(clr, drawBrush.Color, contrColor, 52, 28, "");
            button1.BackgroundImage = bitmap;
            DrawSecond(clr, drawBrush.Color, contrColor, 52, 28, "", 0, false);
            button2.BackgroundImage = bitmap;
            DrawThird(clr, drawBrush.Color, contrColor, 52, 28, "", 0);
            button3.BackgroundImage = bitmap;
            DrawFouth(clr, drawBrush.Color, contrColor, 52, 28, "", 0);
            button4.BackgroundImage = bitmap;
            DrawFifth(clr, drawBrush.Color, contrColor, 52, 28, "", 0);
            button5.BackgroundImage = bitmap;
            DrawSixth(clr, drawBrush.Color, contrColor, 52, 28, "");
            button6.BackgroundImage = bitmap;
            button7.ForeColor = clr;
            pictureBox7.BackgroundImage = newbtmp;
            pictureBox7.Refresh();
        }
        private void шрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = font;
            fontDialog1.ShowDialog();
            font = fontDialog1.Font;
            newbtmp = SizeCheck(newbtmp);
            newbtmp = Clear(newbtmp);
            RefreshGroups();
        }
        private void цветТекстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog2.Color = fontColor;
            colorDialog2.ShowDialog();
            fontColor = colorDialog2.Color;
            drawBrush = new SolidBrush(fontColor);
            newbtmp = SizeCheck(newbtmp);
            newbtmp = Clear(newbtmp);
            RefreshGroups();
        }
        private void цветРабочегоПоляToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog3.Color = workingPlaceColor;
            colorDialog3.ShowDialog();
            workingPlaceColor = colorDialog3.Color;
            pictureBox7.BackColor = workingPlaceColor;
            newbtmp = SizeCheck(newbtmp);
            newbtmp = Clear(newbtmp);
            RefreshGroups();
        }
        private void цветБлоковToolStripMenuItem_Click(object sender, EventArgs e) => ReDrawBlock();
        private void цветКонтураToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog4.Color = contrColor;
            colorDialog4.ShowDialog();
            contrColor = colorDialog4.Color;
            Deploy();
        }
        #endregion
        #region Drawing
        static byte id;
        static int i = 0;
        public void DrawFirst(Color color, Color fontColor, Color contrColor, int width, int height , string text)
        {
            DrawRectangle(color, contrColor, width, height);
            graf.DrawString(text, font, new SolidBrush(fontColor), width/2, height/2-font.Size, drawFormat);
        }
        public void DrawRectangle(Color color, Color contrColor,  int width, int height)
        {
            bitmap = new Bitmap(width, height);
            graf = Graphics.FromImage(bitmap);
            graf.SmoothingMode = SmoothingMode.HighQuality;
            graf.FillRectangle(new SolidBrush(color), 1, 1, width - 1, height - 1);
            graf.DrawRectangle(new Pen(contrColor, 1), 0, 0, width - 1, height - 1);
        }
        public void DrawSecond(Color color, Color fontColor, Color contrColor, int width, int height, string text, int addWidth, bool parable)
        {
            DrawRectangle(color, contrColor, width, height);
            graf.DrawLine(new Pen(contrColor, 1), new Point(0, 0), new Point(width/2, (height - addWidth)/ 2 ));
            graf.DrawLine(new Pen(contrColor, 1), new Point(width/2, (height - addWidth) / 2), new Point(width, 0));
            graf.DrawLine(new Pen(contrColor, 1), new Point(0, (height - addWidth) / 2), new Point(width, (height - addWidth) / 2));
            graf.DrawLine(new Pen(contrColor, 1), new Point(width/2, height), new Point(width/2, (height-addWidth)/2));
            graf.DrawString(text, font, new SolidBrush(fontColor), width / 2, 0, drawFormat);
            if (parable)
            {
                graf.DrawString("Да", font, new SolidBrush(fontColor), 0 + 2 * font.Size, ((height - addWidth) / 6), drawFormat);
                graf.DrawString("Нет", font, new SolidBrush(fontColor), width - 2 * font.Size, ((height - addWidth) / 6), drawFormat);
            }
        }
        public void DrawThird(Color color, Color fontColor, Color contrColor, int width, int height, string text, int addHeight)
        {
            DrawRectangle(color, contrColor, width, height);
            graf.DrawLine(new Pen(contrColor, 1), new Point(0, 0), new Point(width - width/4, (height - addHeight)/2));
            graf.DrawLine(new Pen(contrColor, 1), new Point(width - width / 4, (height - addHeight) / 2), new Point(width, 0));
            graf.DrawLine(new Pen(contrColor, 1), new Point(0, (height - addHeight) / 2), new Point(width, (height - addHeight) / 2));
            graf.DrawLine(new Pen(contrColor, 1), new Point(width - width / 4, (height - addHeight) / 2), new Point(width - width / 4, height));
            graf.DrawString(text, font, new SolidBrush(fontColor), width - width / 3, 0, drawFormat);
        }
        public void DrawFouth(Color color, Color fontColor, Color contrColor, int width, int height, string text, int addHeight)
        {
            DrawRectangle(color, contrColor, width, height);
            graf.DrawLine(new Pen(contrColor, 1), new Point(width/4, height), new Point(width/4, (height - addHeight)/2));
            graf.DrawLine(new Pen(contrColor, 1), new Point(width/4, (height - addHeight)/2), new Point(width, (height - addHeight)/2));
            graf.DrawString(text, font, new SolidBrush(fontColor), width / (width/2), height/ (int)(height/1.75));
        }
        public void DrawFifth(Color color, Color fontColor, Color contrColor, int width, int height, string text, int addHeight)
        {
            DrawRectangle(color, contrColor, width, height);
            graf.DrawLine(new Pen(contrColor, 1), new Point(width/4, 0), new Point(width/4, (height + addHeight) / 2));
            graf.DrawLine(new Pen(contrColor, 1), new Point(width/4, (height + addHeight) / 2), new Point(width, (height + addHeight) / 2));
            graf.DrawString(text, font, new SolidBrush(fontColor), width / (width / 2), height - font.Size * 2);
        }
        public void DrawSixth(Color color, Color fontColor, Color contrColor, int width, int height, string text)
        {
            DrawRectangle(color, contrColor, width, height);
            graf.FillRectangle(new SolidBrush(Color.Gray), 1, 1, width/4, height-2);
            graf.DrawLine(new Pen(contrColor, 1), new Point(width / 4, 0), new Point(width / 4, height));
            graf.FillRectangle(new SolidBrush(Color.Gray), width - width/4, 1, width / 4 -1, height-2);
            graf.DrawLine(new Pen(contrColor, 1), new Point(width - width / 4, 0), new Point(width - width / 4, height));
            graf.DrawString(text, font, new SolidBrush(fontColor), width / 2, height / 2 - font.Size, drawFormat);
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

        private void All_DragEnter(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Copy;
        private void Operation_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            blockView.CreateBlock(id, clr, drawBrush.Color, contrColor, PointToClient(new Point(e.X - 13, e.Y - 20)).X, PointToClient(new Point(e.X - 13, e.Y - 20)).Y, this, 52, 28, $"Block {++i}", maxWidth);
            graf = Graphics.FromImage(newbtmp);
            graf.DrawImage(bitmap, PointToClient(new Point(e.X - 13, e.Y - 20)));
            pictureBox7.BackgroundImage = newbtmp;
            pictureBox7.Refresh();
            textBox2.Text = "кол-во блоков:" + blockView.blocks.Count.ToString();
            textBox2.Text += "; кол-во групп: " + blockView.groups.Count.ToString();
        }

        private void All_DragOver(object sender, DragEventArgs e) => e.Effect = DragDropEffects.Copy;
        private void button1_DragLeave(object sender, EventArgs e) => id = 0;
        private void button2_MouseLeave(object sender, EventArgs e) => id = 1;
        private void button3_DragLeave(object sender, EventArgs e) => id = 2;
        private void button4_DragLeave(object sender, EventArgs e) => id = 3;
        private void button5_DragLeave(object sender, EventArgs e) => id = 4;
        private void button6_DragLeave(object sender, EventArgs e) => id = 5;
        #endregion
        #region Moving
        Block mainblock;
        int xR;
        int yR;
        
        private void pictureBox7_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                for (int i = blockView.blocks.Count - 1; i >= 0; i--)
                {
                    dragging = true;
                    pictureBox7.Focus();
                    if (blockView.blocks[i].blockInternalX <= e.X && blockView.blocks[i].blockInternalY <= e.Y
                        && blockView.blocks[i].Width + blockView.blocks[i].blockInternalX >= e.X && blockView.blocks[i].Height + blockView.blocks[i].blockInternalY >= e.Y)
                    {
                        xR = e.X - blockView.blocks[i].blockInternalX;
                        yR = e.Y - blockView.blocks[i].blockInternalY;
                        mainblock = blockView.blocks[i];
                        if (mainblock.group.Blocks.Count > 0)
                        {
                            Unit preNew = mainblock.group;
                            blockView.groups.Remove(mainblock.group);
                            if (mainblock.outBlock != null)
                            {
                                for (int k = 0; k < mainblock.group.outBlock.subgroup.Count; k++)
                                {
                                    if (mainblock.group.outBlock.subgroup[k] == mainblock.group.outBlock.group)
                                    {
                                        if (mainblock.group.outBlock.type == 2)
                                        {
                                            mainblock.group.outBlock.subgroup.Remove(mainblock.group);
                                        }
                                        mainblock.group = mainblock.group.GetChild(this, mainblock.group, mainblock);
                                        preNew.group = preNew.RemoveChild(this, preNew, mainblock.group);
                                        mainblock.group.outBlock.subgroup[k] = preNew.group;
                                    }
                                }
                            }
                            else
                            {
                                mainblock.group = mainblock.group.GetChild(this, mainblock.group, mainblock);
                                preNew.group = preNew.RemoveChild(this, preNew, mainblock.group);
                            }
                            if (preNew.group != null && preNew.group.Blocks.Count > 0)
                            {
                                blockView.groups.Add(preNew);
                            }
                            blockView.groups.Add(mainblock.group);
                        }
                        blockView.groups.Remove(mainblock.group);
                        blockView.groups.Add(mainblock.group);
                        Cursor = Cursors.Hand;
                        break;
                    }
                    RefreshGroups();
                }
            }
        }

        private void pictureBox7_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging && mainblock != null)
            {
                newbtmp = SizeCheck(newbtmp);
                newbtmp = Clear(newbtmp);
                blockView.SetPosition(mainblock.group, this, e.X - xR, e.Y - yR, mainblock.type);
                graf = Graphics.FromImage(newbtmp);
                RefreshGroups();
            }
            else RefreshGroups();
        }
        private void RefreshGroups()
        {
            graf = Graphics.FromImage(newbtmp);
            blockView.blocks.Clear();
            foreach (var item in blockView.groups)
            {
                item.Draw(this);
                BlocksRecusrst(item);
            }
            pictureBox7.BackgroundImage = newbtmp;
            pictureBox7.Refresh();
            graf.Dispose();
        }
        private void BlocksRecusrst(Unit item)
        {
            foreach (var block in item.Blocks)
            {
                blockView.blocks.Add(block);
                if (block.type >= 1 && block.type <= 4)
                {
                    foreach (var news in block.subgroup)
                    {
                        news.Draw(this);
                        BlocksRecusrst(news);
                    }
                }
            }
        }
        private Bitmap Clear(Bitmap bitmap)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            graf = Graphics.FromImage(bitmap);
            graf.FillRectangle(new SolidBrush(workingPlaceColor), 0, 0, bitmap.Width, bitmap.Height);
            textBox2.Text = "кол-во блоков:" + blockView.blocks.Count.ToString();
            textBox2.Text += "; кол-во групп: " + blockView.groups.Count.ToString();
            return bitmap;
        }
        private void pictureBox7_MouseUp(object sender, MouseEventArgs e)
        {
            if (mainblock != null)
            {
                for (int i = blockView.blocks.Count - 1; i >= 0; i--)
                {
                    if (blockView.blocks[i] != mainblock && blockView.blocks[i].group != mainblock.group)
                    {
                        if (mainblock.subgroup != null && blockView.blocks[i].group == mainblock.subgroup[0])
                        {

                        }
                        else
                        {
                            if (blockView.blocks[i].blockInternalX <= mainblock.blockInternalX + xR && blockView.blocks[i].blockInternalY + blockView.blocks[i].Height <= mainblock.blockInternalY + yR
    && blockView.blocks[i].Width + blockView.blocks[i].blockInternalX >= mainblock.blockInternalX && blockView.blocks[i].Height + blockView.blocks[i].blockInternalY + 10 >= mainblock.blockInternalY)
                            {
                                blockView.StickingBlock(blockView.blocks[i].group, mainblock.group, this);
                                break;
                            }
                            else if (blockView.blocks[i].blockInternalX <= mainblock.blockInternalX + xR && blockView.blocks[i].blockInternalY <= mainblock.blockInternalY + yR
        && blockView.blocks[i].Width + blockView.blocks[i].blockInternalX >= mainblock.blockInternalX && blockView.blocks[i].Height + blockView.blocks[i].blockInternalY >= mainblock.blockInternalY)
                            {
                                switch (blockView.blocks[i].type)
                                {
                                    case 1:
                                        {
                                            if (blockView.blocks[i].blockInternalX + blockView.blocks[i].Width / 2 <= mainblock.blockInternalX + xR && blockView.blocks[i].blockInternalY + (blockView.blocks[i].Height - blockView.blocks[i].addHeight) / 2 <= mainblock.blockInternalY + yR
        && blockView.blocks[i].Width + blockView.blocks[i].blockInternalX >= mainblock.blockInternalX
        && blockView.blocks[i].Height + blockView.blocks[i].blockInternalY >= mainblock.blockInternalY)
                                            {
                                                if (blockView.blocks[i].subgroup[1].Blocks.Count == 0)
                                                {
                                                    blockView.InBlocks(blockView.blocks[i], mainblock.group, this, 1);
                                                    break;
                                                }
                                            }
                                            else if (blockView.blocks[i].blockInternalX <= mainblock.blockInternalX + xR && blockView.blocks[i].blockInternalY + (blockView.blocks[i].Height - blockView.blocks[i].addHeight) / 2 <= mainblock.blockInternalY + yR
        && blockView.blocks[i].Width + blockView.blocks[i].blockInternalX >= mainblock.blockInternalX
        && blockView.blocks[i].Height + blockView.blocks[i].blockInternalY >= mainblock.blockInternalY)
                                            {
                                                if (blockView.blocks[i].subgroup[0].Blocks.Count == 0)
                                                {
                                                    blockView.InBlocks(blockView.blocks[i], mainblock.group, this, 0);
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    case 2:
                                        {
                                            if (blockView.blocks[i].blockInternalX + blockView.blocks[i].Width - blockView.blocks[i].Width/4 <= mainblock.blockInternalX + xR && blockView.blocks[i].blockInternalY + (blockView.blocks[i].Height - blockView.blocks[i].addHeight) / 2 <= mainblock.blockInternalY + yR
        && blockView.blocks[i].Width + blockView.blocks[i].blockInternalX >= mainblock.blockInternalX
        && blockView.blocks[i].Height + blockView.blocks[i].blockInternalY >= mainblock.blockInternalY)
                                            {
                                                if (blockView.blocks[i].subgroup[blockView.blocks[i].subgroup.Count - 1].Blocks.Count == 0)
                                                {
                                                    blockView.InBlocks(blockView.blocks[i], mainblock.group, this, blockView.blocks[i].subgroup.Count-1);
                                                    break;
                                                }
                                            }
                                            else if (blockView.blocks[i].blockInternalX <= mainblock.blockInternalX + xR && blockView.blocks[i].blockInternalY + (blockView.blocks[i].Height - blockView.blocks[i].addHeight) / 2 <= mainblock.blockInternalY + yR
        && blockView.blocks[i].Width + blockView.blocks[i].blockInternalX >= mainblock.blockInternalX
        && blockView.blocks[i].Height + blockView.blocks[i].blockInternalY >= mainblock.blockInternalY)
                                            {
                                                if (blockView.blocks[i].subgroup[0].Blocks.Count == 0)
                                                {
                                                    blockView.InBlocks(blockView.blocks[i], mainblock.group, this, 0);
                                                    break;
                                                }
                                                else if (blockView.blocks[i].subgroup.Count >= 2 && blockView.blocks[i].type == 2)
                                                {
                                                    blockView.blocks[i].subgroup.Insert(blockView.blocks[i].subgroup.Count - 2, mainblock.group);
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    case 3:
                                        {
                                            if (blockView.blocks[i].blockInternalX + blockView.blocks[i].Width / 4 <= mainblock.blockInternalX + xR && blockView.blocks[i].blockInternalY + (blockView.blocks[i].Height - blockView.blocks[i].addHeight) / 2 <= mainblock.blockInternalY + yR
        && blockView.blocks[i].Width + blockView.blocks[i].blockInternalX >= mainblock.blockInternalX
        && blockView.blocks[i].Height + blockView.blocks[i].blockInternalY >= mainblock.blockInternalY)
                                            {
                                                if (blockView.blocks[i].subgroup[0].Blocks.Count == 0)
                                                {
                                                    blockView.InBlocks(blockView.blocks[i], mainblock.group, this, 0);
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    case 4:
                                        {
                                            if (blockView.blocks[i].blockInternalX + blockView.blocks[i].Width / 4 <= mainblock.blockInternalX + xR && blockView.blocks[i].blockInternalY <= mainblock.blockInternalY + yR
        && blockView.blocks[i].Width + blockView.blocks[i].blockInternalX >= mainblock.blockInternalX
        && blockView.blocks[i].Height / 2 + blockView.blocks[i].blockInternalY >= mainblock.blockInternalY)
                                            {
                                                if (blockView.blocks[i].subgroup[0].Blocks.Count == 0)
                                                {
                                                    blockView.InBlocks(blockView.blocks[i], mainblock.group, this, 0);
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            newbtmp = SizeCheck(newbtmp);
            newbtmp = Clear(newbtmp);
            RefreshGroups();
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
            blockView.groups.Clear();
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
                blockView.DeleteBlock(this, mainblock.group);
                mainblock = null;
                dragging = false;
                Cursor = Cursors.Default;
                newbtmp = SizeCheck(newbtmp);
                newbtmp = Clear(newbtmp);
                RefreshGroups();
                Focus();
            }
            else if (e.KeyData == Keys.N && mainblock != null && dragging)
            {
                if (mainblock.subgroup != null && mainblock.subgroup.Count >= 2 && mainblock.type == 2)
                {
                    var y = new OperationBlock(clr, drawBrush.Color, contrColor, 0, 0, 0, 52, 28, $"Block {++i}");
                    Unit x = new Unit()
                    {
                        Blocks = new List<Block>()
                    };
                    x.Blocks.Add(y);
                    y.group = x;
                    form = new SetText(this, y);
                    form.ShowDialog();
                    y.text = form.Block.text;
                    y.blockInternalColor = form.Block.blockInternalColor;
                    y.contrInternalColor = form.Block.contrInternalColor;
                    mainblock.subgroup.Insert(mainblock.subgroup.Count - 1, x);
                    mainblock = null;
                    dragging = false;
                    newbtmp = SizeCheck(newbtmp);
                    newbtmp = Clear(newbtmp);
                    graf = Graphics.FromImage(newbtmp);
                    RefreshGroups();
                }
            }
            else if (e.KeyData == Keys.I && mainblock != null && dragging)
            {
                for (int i = blockView.blocks.Count - 1; i >= 0; i--)
                {
                    if (blockView.blocks[i] != mainblock && blockView.blocks[i].group != mainblock.group)
                    {
                        if (mainblock.subgroup != null && blockView.blocks[i].group == mainblock.subgroup[0])
                        {

                        }
                        else
                        {
                            if (blockView.blocks[i].subgroup != null && blockView.blocks[i].subgroup.Count >= 2 && blockView.blocks[i].type == 2)
                            {
                                blockView.groups.Remove(mainblock.group);
                                blockView.blocks[i].subgroup.Insert(blockView.blocks[i].subgroup.Count - 1, mainblock.group);
                                mainblock = null;
                                dragging = false;
                                break;
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region InBlockChanging
        SetText form;
        private void pictureBox7_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor = Cursors.Hand;
                for (int i = blockView.blocks.Count - 1; i >= 0; i--)
                {
                    if (blockView.blocks[i].blockInternalX <= e.X && blockView.blocks[i].blockInternalY <= e.Y
                        && blockView.blocks[i].Width + blockView.blocks[i].blockInternalX >= e.X && blockView.blocks[i].Height + blockView.blocks[i].blockInternalY >= e.Y)
                    {
                        form = new SetText(this, blockView.blocks[i]);
                        form.ShowDialog();
                        blockView.blocks[i].text = form.Block.text;
                        blockView.blocks[i].blockInternalColor = form.Block.blockInternalColor;
                        blockView.blocks[i].contrInternalColor = form.Block.contrInternalColor;
                        newbtmp = SizeCheck(newbtmp);
                        newbtmp = Clear(newbtmp);
                        graf = Graphics.FromImage(newbtmp);
                        RefreshGroups();
                        break;
                    }
                }
                Cursor = Cursors.Default;
            }
        }
        #endregion
    }
}
