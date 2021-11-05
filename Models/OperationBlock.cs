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
        public OperationBlock(Color color, int key, int x, int y, int width, int height)
        {
            maxInCon = 1;
            maxOutCon = 1;
            blockInternalColor = color;
            blockInternalKey = key;
            blockInternalX = x;
            blockInternalY = y;
            Width = width;
            Height = height;
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
            drawNassi.DrawFirst(this.blockInternalColor, Width, Height);
            return drawNassi;
        }
    }
}
