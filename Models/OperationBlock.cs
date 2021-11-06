using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DrawNassiProject.Models
{
    class OperationBlock : Block
    {
        public OperationBlock()
        {
            
        }
        public OperationBlock(Color color, Color fontColor, int key, int x, int y, int width, int height, string text)
        {
            maxInCon = 1;
            maxOutCon = 1;
            blockInternalColor = color;
            fontInternalColor = fontColor;
            blockInternalKey = key;
            blockInternalX = x;
            blockInternalY = y;
            Width = width;
            Height = height;
            this.text = text;
            type = 0;
        }
        public void AddCon(int newCon)
        {
            if (!blockCon.Contains(newCon))
            {
                blockCon.Add(newCon);
            }
        }
        public override DrawNassi Draw(DrawNassi drawNassi)
        {
            Width = text.Length * (int)drawNassi.font.Size;
            Height = (int)drawNassi.font.Size * 2;
            drawNassi.DrawFirst(this.blockInternalColor, fontInternalColor, Width, Height, text);
            return drawNassi;
        }
    }
}
