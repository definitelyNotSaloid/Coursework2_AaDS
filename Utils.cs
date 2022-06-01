using Lab1_AaDS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework2_AaDS
{
    public static class YetAnotherPackOfListExtentions
    {
        public static void Remove<T>(this NotAList<T> list, Predicate<T> predicate)    // remove first element matching predicate
        {
            // its O(n^2), i know its bad, but i doubt it will make a huge perfomance impact considering that local lists are at most 10 elements len
            foreach (var el in list)
            {
                if (predicate(el))
                {
                    list.Remove(el);
                    return;
                }
            }
        }

        public static bool Find<T>(this NotAList<T> list, Predicate<T> predicate, out T item)        // returns first element matching predicate, or default (sadly i cant use nullable T)
        {
            foreach (var el in list)
            {
                if (predicate(el))
                {
                    item = el;
                    return true;
                }
            }

            item = default;
            return false;
        }

        public static int IndexOf<T>(this NotAList<T> list, Predicate<T> predicate)
        {
            int index = 0;
            foreach (var el in list)
            {
                if (predicate(el))
                    return index;

                index++;
            }

            return -1;
        }
    }
}
