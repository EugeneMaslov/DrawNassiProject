using DrawNassiProject.Models.Composite;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DrawNassiProject.Models
{
    [Serializable]
    public class BranchingBlock : Block
    {
        public BranchingBlock() { }
        public BranchingBlock(Color color, Color fontColor, Color contrColor, int key, int x, int y, int width, int height, string text)
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
            subgroup = new List<Unit>() { new Unit() { Blocks = new List<Block>() }, new Unit() { Blocks = new List<Block>() } };
            type = 1;
        }
        public override DrawNassi Draw(DrawNassi drawNassi)
        {
            if (group.UnitWidth < 2 * text.Length * (int)drawNassi.font.Size)
            {
                if (text.Length > 0)
                {
                    Width = 2 * text.Length * (int)drawNassi.font.Size;
                    group.UnitWidth = Width;
                }
            }
            else if (text.Length == 0) text = "Unnamed Block";
            Width = group.UnitWidth;
            addHeight = 0;
            int preHeight;
            foreach (var item in subgroup)
            {
                preHeight = 0;
                foreach (var block in item.Blocks)
                {
                    preHeight += block.Height;
                    if (block.group.UnitWidth * 2 > group.UnitWidth)
                    {
                        group.UnitWidth = block.Width * 2;
                    }
                    else block.group.UnitWidth = group.UnitWidth / 2;
                }
                if (preHeight > addHeight)
                {
                    addHeight = preHeight;
                }
            }
            if (addHeight > 0)
            {
                addHeight = addHeight - (int)drawNassi.font.Size * 2;
            }
            Height = (int)drawNassi.font.Size * 4 + addHeight;
            Width = group.UnitWidth;
            if (subgroup[0].Blocks.Count > 0)
            {
                subgroup[0].Blocks[0].group.UnitWidth = group.UnitWidth / 2;
                subgroup[0].Blocks[0].blockInternalX = blockInternalX;
                subgroup[0].Blocks[0].blockInternalY = blockInternalY + (Height - addHeight) / 2;

            }
            if (subgroup[1].Blocks.Count > 0)
            {
                subgroup[1].Blocks[0].group.UnitWidth = group.UnitWidth / 2;
                subgroup[1].Blocks[0].blockInternalX = blockInternalX + Width / 2;
                subgroup[1].Blocks[0].blockInternalY = blockInternalY + (Height - addHeight) / 2;
            }
            drawNassi.DrawSecond(blockInternalColor, fontInternalColor, contrInternalColor, Width, Height, text, addHeight, subgroup.Count > 0);
            return drawNassi;
        }
    }
}
