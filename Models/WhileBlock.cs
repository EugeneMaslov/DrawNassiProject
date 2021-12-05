using DrawNassiProject.Models.Composite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DrawNassiProject.Models
{
    [Serializable]
    class WhileBlock : Block
    {
        public WhileBlock() { }
        public WhileBlock(Color color, Color fontColor, Color contrColor, int key, int x, int y, int width, int height, string text)
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
            type = 2;
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
            Width = group.UnitWidth;
            addHeight = 0;
            int preHeight;
            int param = 0;
            if (subgroup.Count > 0)
            {
                param = (int)((group.UnitWidth - group.UnitWidth / 4) / (double)(subgroup.Count - 1));
            }
            foreach (var item in subgroup)
            {
                preHeight = 0;
                foreach (var block in item.Blocks)
                {
                    preHeight += block.Height;
                    if (block.group == subgroup[subgroup.Count - 1])
                    {
                        if (block.group.UnitWidth > group.UnitWidth / 4)
                        {
                            group.UnitWidth = block.group.UnitWidth * 4;
                        }
                        else block.group.UnitWidth = group.UnitWidth / 4;
                    }
                    else
                    {
                        if (block.group.UnitWidth > param)
                        {
                            param = block.group.UnitWidth;
                        }
                        else block.group.UnitWidth = param;
                    }
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
            group.UnitWidth = param * (subgroup.Count - 1) + group.UnitWidth/4;
            Height = (int)drawNassi.font.Size * 4 + addHeight;
            Width = group.UnitWidth;
            int j = 0;
            for (int i = subgroup.Count - 2; i >= 0; i--)
            {
                if (subgroup[i].Blocks.Count < 1 && subgroup.Count > 2)
                {
                    subgroup.Remove(subgroup[i]);
                    param = (int)((group.UnitWidth - group.UnitWidth / 4) / (double)(subgroup.Count - 1));
                }
                else if (subgroup[i].Blocks.Count > 0 && subgroup[i] != subgroup[subgroup.Count-1])
                {
                    subgroup[i].Blocks[0].group.UnitWidth = param;
                    subgroup[i].Blocks[0].blockInternalX = blockInternalX + j;
                    j += param;
                    subgroup[i].Blocks[0].blockInternalY = blockInternalY + (Height - addHeight) / 2;
                }
            }
            if (subgroup[subgroup.Count-1].Blocks.Count > 0)
            {
                subgroup[subgroup.Count - 1].Blocks[0].group.UnitWidth = group.UnitWidth / 4;
                subgroup[subgroup.Count - 1].Blocks[0].blockInternalX = blockInternalX + group.UnitWidth - group.UnitWidth / 4;
                subgroup[subgroup.Count - 1].Blocks[0].blockInternalY = blockInternalY + (Height - addHeight) / 2;
            }
            drawNassi.DrawThird(blockInternalColor, fontInternalColor, contrInternalColor, Width, Height, text, addHeight);
            return drawNassi;
        }
    }
}
