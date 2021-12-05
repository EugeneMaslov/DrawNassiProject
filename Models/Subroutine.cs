using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DrawNassiProject.Models
{
    [Serializable]
    class Subroutine : Block
    {
        public Subroutine() {}
        public Subroutine(Color color, Color fontColor, Color contrColor, int key, int x, int y, int width, int height, string text)
        {
            blockInternalColor = color;
            fontInternalColor = fontColor;
            blockInternalKey = key;
            blockInternalX = x;
            blockInternalY = y;
            Width = width;
            Height = height;
            this.text = text;
            contrInternalColor = contrColor;
            type = 5;
        }
        public override DrawNassi Draw(DrawNassi drawNassi)
        {
            if (group.UnitWidth < 2*text.Length * (int)drawNassi.font.Size)
            {
                if (text.Length > 0)
                {
                    Width = 2*text.Length * (int)drawNassi.font.Size;
                    group.UnitWidth = Width;
                }
            }
            else Width = group.UnitWidth;
            Height = (int)drawNassi.font.Size*2;
            drawNassi.DrawSixth(this.blockInternalColor, fontInternalColor, contrInternalColor, (int)Width, Height, text);
            return drawNassi;
        }
    }
}
