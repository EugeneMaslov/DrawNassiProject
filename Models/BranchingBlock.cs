using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DrawNassiProject.Models
{
    class BranchingBlock : Block
    {
        public BranchingBlock()
        {

        }
        public BranchingBlock(Color color, int key, int x, int y, int width, int height)
        {
            maxInCon = 1;
            maxOutCon = 2;
            blockInternalColor = color;
            blockInternalKey = key;
            blockInternalX = x;
            blockInternalY = y;
            Width = width;
            Height = height;
            type = 1;
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
            drawNassi.DrawSecond(blockInternalColor, Width, Height);
            return drawNassi;
        }
    }
}
