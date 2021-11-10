﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DrawNassiProject.Models
{
    class Subroutine : Block
    {
        public Subroutine()
        {

        }
        public Subroutine(Color color, Color fontColor, int key, int x, int y, int width, int height, string text)
        {
            blockInCon = new List<Block>();
            blockOutCon = new List<Block>();
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
            drawNassi.DrawSixth(this.blockInternalColor, fontInternalColor, Width, Height, text);
            return drawNassi;
        }
    }
}
