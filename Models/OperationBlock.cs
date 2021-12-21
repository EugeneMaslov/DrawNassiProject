using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DrawNassiProject.Models
{
    [Serializable]
    class OperationBlock : Block
    {
        public OperationBlock() { }
        public OperationBlock(Color color, Color fontColor, Color contrColor, int key, int x, int y, int width, int height, string text)
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
            type = 0;
        }
        public override DrawNassi Draw(DrawNassi drawNassi)
        {
            if (group.UnitWidth < text.Length * (int)drawNassi.font.Size)
            {
                if (text.Length > 0)
                {
                    Width = text.Length * (int)drawNassi.font.Size;
                    group.UnitWidth = Width;
                }
            }
            else if (text.Length == 0) text = "Unnamed Block";
            else Width = group.UnitWidth;
            Height = (int)drawNassi.font.Size * 2;
            drawNassi.DrawFirst(this.blockInternalColor, fontInternalColor, contrInternalColor, Width, Height, text);
            return drawNassi;
        }
    }
}
