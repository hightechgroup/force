using System.Collections.Generic;
using static System.String;

namespace Force.Tests.Expressions
{
    internal static class OrderChecker
    {
        public static bool CheckOrder(List<Product> res, bool asc)
        {
            bool flag = true;
            string current = null;
            foreach (var p in res)
            {
                if (current == null)
                {
                    current = p.Name;
                    continue;
                }

                flag = asc 
                    ? CompareOrdinal(p.Name, current) >= 0
                    : CompareOrdinal(p.Name, current) <= 0;
                
                if (!flag)
                {
                    break;
                }
            }

            return flag;
        }
    }
}