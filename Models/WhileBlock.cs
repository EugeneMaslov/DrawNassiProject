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
        public WhileBlock(Color color, int key, int x, int y, int width, int height)
        {
            maxInCon = 1;
            maxOutCon = int.MaxValue;
            blockInternalColor = color;
            blockInternalKey = key;
            blockInternalX = x;
            blockInternalY = y;
            Width = width;
            Height = height;
            type = 2;
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
            drawNassi.DrawThird(blockInternalColor, Width, Height);
            return drawNassi;
        }
    }
}
