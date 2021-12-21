using DrawNassiProject.Models.Composite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DrawNassiProject.Models
{
    [Serializable]
    class WhilePreBlock : Block
    {
        public WhilePreBlock() {}
        public WhilePreBlock(Color color, Color fontColor, Color contrColor, int key, int x, int y, int width, int height, string text)
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
            subgroup = new List<Unit>() { new Unit() { Blocks = new List<Block>() } };
            type = 3;
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
            Width = group.UnitWidth;
            addHeight = 0;
            int parametr = 0;
            int preHeight;
            foreach (var item in subgroup)
            {
                preHeight = 0;
                foreach (var block in item.Blocks)
                {
                    preHeight += block.Height;
                    if (block.Width > (int)(group.UnitWidth*3.0/4))
                    {
                        group.UnitWidth = (int)(block.Width*5.0/4);
                    }
                    else
                    {
                        block.Width = (int)(group.UnitWidth*3.0/4);
                    }
                    if (parametr < group.UnitWidth - block.Width)
                    {
                        parametr = group.UnitWidth - block.Width;
                    }
                    else
                    {
                        block.Width = group.UnitWidth - parametr;
                    }
                }
                if (preHeight > addHeight)
                {
                    addHeight = preHeight;
                }
            }
            if (addHeight > 0)
            {
                addHeight = addHeight - (int)drawNassi.font.Size*2;
            }
            Height = (int)drawNassi.font.Size * 4 + addHeight;
            if (group.UnitWidth > Width)
            {
                Width = group.UnitWidth;
            }
            if (subgroup[0].Blocks.Count > 0)
            {
                subgroup[0].Blocks[0].group.UnitWidth = group.UnitWidth - parametr;
                subgroup[0].Blocks[0].blockInternalX = blockInternalX + parametr;
                subgroup[0].Blocks[0].blockInternalY = blockInternalY + (Height - addHeight) / 2;
            }
            drawNassi.DrawFouth(blockInternalColor, fontInternalColor, contrInternalColor, Width, Height, text, addHeight);
            return drawNassi;
        }
    }
}
