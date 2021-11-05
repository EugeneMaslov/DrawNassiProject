using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DrawNassiProject.Models
{
    class WhilePreBlock : Block
    {
        public WhilePreBlock()
        {

        }
        public WhilePreBlock(Color color, int key, int x, int y, int width, int height)
        {
            maxInCon = 1;
            maxOutCon = 1;
            blockInternalColor = color;
            blockInternalKey = key;
            blockInternalX = x;
            blockInternalY = y;
            Width = width;
            Height = height;
            type = 3;
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
            drawNassi.DrawFouth(this.blockInternalColor, Width, Height);
            return drawNassi;
        }
    }
}
