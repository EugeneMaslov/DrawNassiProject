using System;
using System.Collections.Generic;
using System.Drawing;

namespace DrawNassiProject.Models.Composite
{
    [SerializableAttribute]
    public class Unit : Block
    {
        public List<Block> Blocks { get; set; }
        public int UnitWidth = int.MinValue;
        public override DrawNassi Add(DrawNassi drawNassi, Block block)
        {
            if (Blocks != null)
            {
                Block block1 = Blocks[Blocks.Count - 1];
                block.blockInternalX = block1.blockInternalX;
                block.blockInternalY = block1.blockInternalY + block1.Height;
            }
            else Blocks = new List<Block>();
            block.group = this;
            block.index = Blocks.IndexOf(block);
            Blocks.Add(block);
            return drawNassi;
        }
        public override DrawNassi Draw(DrawNassi drawNassi)
        {
            if (Blocks.Count > 0)
            {
                Block block = Blocks[0];
                foreach (var cur in Blocks)
                {

                    drawNassi.graf = Graphics.FromImage(drawNassi.newbtmp);
                    if (block != cur)
                    {
                        cur.blockInternalX = block.blockInternalX;
                        cur.blockInternalY = block.blockInternalY + block.Height;
                    }
                    cur.Draw(drawNassi);
                    block = cur;
                    drawNassi.graf = Graphics.FromImage(drawNassi.newbtmp);
                    drawNassi.graf.DrawImage(drawNassi.bitmap, cur.blockInternalX, cur.blockInternalY);
                }
            }
            return drawNassi;
        }
        public override Unit GetChild(DrawNassi drawNassi, Unit unit, Block block)
        {
            index = unit.Blocks.IndexOf(block);
            Unit newUnit = new Unit() { Blocks = new List<Block>() };
            if (index < Blocks.Count)
            {
                for (int i = index; i < unit.Blocks.Count; i++)
                {
                    newUnit.Blocks.Add(Blocks[i]);
                }
                foreach (var blocks in newUnit.Blocks)
                {
                    blocks.group = newUnit;
                }
                block.group = newUnit;
                unit = unit.RemoveChild(drawNassi, unit, newUnit);
                Blocks = unit.Blocks;
                return newUnit;
            }
            return null;
        }
        public Unit RemoveChild(DrawNassi drawNassi, Unit parent, Unit child)
        {
            parent.Remove(drawNassi, child);
            return parent;
        }
        public override DrawNassi Remove(DrawNassi drawNassi, Unit unit)
        {
            index = Blocks.IndexOf(unit.Blocks[0]);
            if (index < Blocks.Count && Blocks.Count > 0)
            {
                if (index >= 0)
                {
                    int n = Blocks.Count;
                    for (int i = Blocks.Count - 1; i >= index; i--)
                    {
                        Blocks[i].group = Blocks[index].group;
                        Blocks.Remove(Blocks[i]);
                    }
                }
            }
            return drawNassi;
        }
    }
}
