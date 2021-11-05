using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DrawNassiProject.Models
{
    /// <summary>
    /// Общий класс блоков
    /// </summary>
    public abstract class Block
    {
        protected int maxInCon; // максимальное количесво связей (вход)
        protected int maxOutCon; // максимальное количество связей (выход)
        protected List<int> blockCon { get; set; } // связи блока
        public Color blockInternalColor; // цвет блока
        public int blockInternalKey; // код блока
        public int blockInternalX; // x-координата блока
        public int blockInternalY; // y-координата блока
        public int Width;
        public int Height;
        public byte type;
        public string text;
        public override bool Equals(object obj)
        {
            Block block = obj as Block;
            return this.blockInternalKey == block.blockInternalKey;
        }
        public abstract DrawNassi Draw(DrawNassi drawNassi);
    }
}
