using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DrawNassiProject.Models
{
    class WhilePostBlock : Block
    {
        public WhilePostBlock()
        {

        }
        public WhilePostBlock(Color color, int key, int x, int y, int width, int height)
        {
            maxInCon = 1;
            maxOutCon = 1;
            blockInternalColor = color;
            blockInternalKey = key;
            blockInternalX = x;
            blockInternalY = y;
            Width = width;
            Height = height;
            type = 4;
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
            drawNassi.DrawFifth(this.blockInternalColor, Width, Height);
            return drawNassi;
        }
    }
}
