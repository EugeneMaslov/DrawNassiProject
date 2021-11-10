﻿using DrawNassiProject.Models.Composite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DrawNassiProject.Models
{
    public class BranchingBlock : Block
    {
        public BranchingBlock()
        {

        }
        public BranchingBlock(Color color, Color fontColor, int key, int x, int y, int width, int height, string text)
        {
            blockInCon = new List<Block>();
            blockOutCon = new List<Block>();
            maxInCon = 1;
            maxOutCon = 2;
            blockInternalColor = color;
            fontInternalColor = fontColor;
            blockInternalKey = key;
            blockInternalX = x;
            blockInternalY = y;
            Width = width;
            Height = height;
            this.text = text;
            type = 1;
        }
        public override DrawNassi Draw(DrawNassi drawNassi)
        {
            if (group.UnitWidth < 2*text.Length * (int)drawNassi.font.Size)
            {
                if (text.Length > 0)
                {
                    Width = 2 * text.Length * (int)drawNassi.font.Size;
                    group.UnitWidth = Width;
                }
            }
            else Width = group.UnitWidth;
            addWidth = 0;
            Height = (int)drawNassi.font.Size * 4 + addWidth;
            drawNassi.DrawSecond(blockInternalColor, fontInternalColor, Width, Height, text, addWidth);
            return drawNassi;
        }
    }
}
