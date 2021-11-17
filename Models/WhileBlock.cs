using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DrawNassiProject.Models
{
    class WhileBlock : Block
    {
        public WhileBlock()
        {

        }
        public WhileBlock(Color color, Color fontColor, Color contrColor, int key, int x, int y, int width, int height, string text)
        {
            blockInCon = new List<Block>();
            blockOutCon = new List<Block>();
            maxInCon = 1;
            maxOutCon = int.MaxValue;
            blockInternalColor = color;
            fontInternalColor = fontColor;
            blockInternalKey = key;
            blockInternalX = x;
            blockInternalY = y;
            Width = width;
            Height = height;
            this.text = text;
            contrInternalColor = contrColor;
            type = 2;
        }
        public override DrawNassi Draw(DrawNassi drawNassi)
        {
            if (group.UnitWidth < 3*text.Length * (int)drawNassi.font.Size)
            {
                if (text.Length > 0)
                {
                    Width = 3*text.Length * (int)drawNassi.font.Size;
                    group.UnitWidth = Width;
                }
            }
            else Width = group.UnitWidth;
            Height = (int)drawNassi.font.Size * 4;
            drawNassi.DrawThird(blockInternalColor, fontInternalColor, contrInternalColor, Width, Height, text);
            return drawNassi;
        }
    }
}
