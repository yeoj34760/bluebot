using System;
using System.Collections.Generic;
using System.Text;

namespace bluebot
{
    class utility
    {
        static public string Randoms(string[] vs)
        {
            Random random = new Random();
            return vs[random.Next(0, vs.Length)];
        }
        static public string NoBlank(string str)
        {
            return str.Replace(" ", null);
        }
    }
}
