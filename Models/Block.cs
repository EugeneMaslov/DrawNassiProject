using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DrawNassiProject.Models.Composite;

namespace DrawNassiProject.Models
{
    [Serializable]
    /// <summary>
    /// Общий класс блоков
    /// </summary>
    public abstract class Block
    {
        public Unit group;
        public Block outBlock;
        public List<Unit> subgroup;
        public int addHeight;
        public int index;
        public int maxInCon; // максимальное количесво связей (вход)
        public int maxOutCon; // максимальное количество связей (выход)
        public List<Block> blockInCon { get; set; } // связи блока (in)
        public List<Block> blockOutCon { get; set; } // связи блока (out)
        public Color blockInternalColor; // цвет блока
        public Color fontInternalColor;
        public Color contrInternalColor;
        public int blockInternalKey; // код блока
        public int blockInternalX; // x-координата блока
        public int blockInternalY; // y-координата блока
        public int Width;
        public int Height;
        public byte type;
        public string text;
        public void AddInCon(Block newCon)
        {
            if (!blockInCon.Contains(newCon))
            {
                blockInCon.Add(newCon);
            }
        }
        public void AddOutCon(Block newCon)
        {
            if (!blockOutCon.Contains(newCon))
            {
                blockOutCon.Add(newCon);
            }
        }
        public virtual DrawNassi Add(DrawNassi drawNassi, Block block)
        {
            return drawNassi;
        }
        public abstract DrawNassi Draw(DrawNassi drawNassi);
        public virtual Unit GetChild(DrawNassi drawNassi, Unit unit, Block block)
        {
            return unit;
        }

        public virtual DrawNassi Remove(DrawNassi drawNassi, Unit unit)
        {
            return drawNassi;
        }
    }
}
