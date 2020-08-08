using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcedureGeneration
{
    public class PRNG
    {
        private int Seed = 50;

        public void SetSeed(int seed)
        {
            Seed = seed;
        }

        public int GetRandIntFromTwoNumbers(float a, float b, int maxvalue)
        {
            // Метод генерирует псевдослучайное число от 0 до maxvalue включительно на основе двух переменных a и b
            int numb = (int) (a * Seed + b) % ++maxvalue;
            
            return numb;
        }

        public int GetRandIntFromTwoNumbers(int a, int b, byte[] permTable)
        {
            // Метод генерирует псевдослучайное число от 0 до maxvalue на основе двух переменных a и b
            int numb = (int) Math.Abs(((a * 1836311903) ^ (b * 2971215073) + 4807526976) % permTable.Length);
            // Затем берётся случайный байт из таблицы по адресу ячейки numb
            numb = permTable[numb];
            return numb;
        }

    }
}
