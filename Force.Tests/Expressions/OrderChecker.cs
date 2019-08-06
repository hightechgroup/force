using System;
using System.Collections.Generic;
using Force.Tests.Expressions;

static internal class OrderChecker
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
                ? String.Compare(p.Name, current) >= 0
                : String.Compare(p.Name, current) <= 0;
                
            if (!flag)
            {
                break;
            }
        }

        return flag;
    }
}