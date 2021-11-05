using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawNassiProject
{
    class Blocks
    {
        protected List<int> block_con; // связи блока
        public string block_internal_color; // цвет блока
        public byte block_internal_type; // тип блока
        public int block_internal_key; // код блока
        public int block_internal_x; // x-координата блока
        public int block_internal_y; // y-координата блока

        /// <summary>
        /// Создание нового блока
        /// </summary>
        /// <param name="color">Цвет блока</param>
        /// <param name="type">Тип блока</param>
        /// <param name="x">x-координата блока</param>
        /// <param name="y">y-координата блока</param>
        /// <returns>Блок создан</returns>
        public bool CreateBlock(int new_key, string color, byte type, int x, int y)
        {
            if (block_internal_key != 0)
            {
                block_internal_color = color;
                block_internal_type = type;
                block_internal_key = new_key;
                block_internal_x = x;
                block_internal_y = y;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Удаление блока
        /// </summary>
        /// <param name="key">Код блока</param>
        /// <returns>Удаление определенного кода</returns>
        static bool DeleteBlock(int key)
        {
            return false;
        }


        /// <summary>
        /// Изменение цвета блока
        /// </summary>
        /// <param name="key">Код блока</param>
        /// <param name="color">Цвет блока</param>
        /// <returns>Изменяет цвет определенного блока</returns>
        public bool SetColorBlock(int key, string color)
        {
            return false;
        }


        /// <summary>
        /// Изменение позиции блока
        /// </summary>
        /// <param name="key">Код блока</param>
        /// <param name="new_x">новая x-координата блока</param>
        /// <param name="new_y">новая y-координата блока</param>
        /// <returns>Изменяет позицию определенного блока</returns>
        public bool SetPosition(int key, int new_x, int new_y)
        {
            if (new_x != block_internal_x || new_y != block_internal_y)
            {
                block_internal_x = new_x;
                block_internal_y = new_y;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Добавление связи
        /// </summary>
        /// <param name="key">Код добавляемого блока в связь</param>
        /// <returns>Изменение связей блока</returns>
        private bool AddConnections(int key)
        {
            if (!block_con.Contains(key))
            {
                block_con.Add(key);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Функция прилипания блоков
        /// </summary>
        /// <param name="first_block_key">Код первого блока</param>
        /// <param name="second_block_key">Код второго блока</param>
        /// <param name="x">x-координата первого блока</param>
        /// <param name="y">y-координата первого блока</param>
        /// <returns>Два блока прилипают к друг другу, происходит связывание</returns>
        static bool StickingBlock(int first_block_key, int second_block_key, int x, int y)
        {
            return false;
        }

    }
}
