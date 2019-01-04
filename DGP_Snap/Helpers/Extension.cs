﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGP_Snap.Helpers
{
    public static class ListExtension
    {

        private static readonly Random random = new Random();

        public static T GetRandom<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
                return default(T);
            return list[random.Next(0, list.Count)];
        }

        public static List<T> SubArray<T>(this List<T> list, int startIndex, int length = -1)
        {
            if (length == -1) return list.Skip(startIndex).ToList();
            return list.Skip(startIndex).Take(length).ToList();
        }

        public static List<T> CombineWith<T>(this List<T> self, List<T> list)
        {
            if (self == null)
                return list;
            return (self.Union(list)).ToList();
        }
    }

}
