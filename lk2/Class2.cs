using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace lk2
{
    public class Class2
    {
        //Есть коллекция чисел и отдельное число Х. Надо вывести все пары чисел, которые в сумме равны заданному Х.
        public static void printPairSums(int[] array, int sum)
        {
            Array.Sort(array); //сортируем массив
            int first = 0; //первый элемент
            int last = array.Length - 1; //последний элемент 
            while (first < last)
            {
                int s = array[first] + array[last];
                if (s == sum)
                {
                    Trace.WriteLine(array[first] + "+" + array[last]);
                    first++;
                    last--;
                }
                else
                {
                    if (s < sum) first++; //first + last меньше sum, нет смысла проверять меньшие значения
                    else last--; 
                }
            }
        }
    }
}
