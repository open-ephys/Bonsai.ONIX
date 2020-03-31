using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPackage
{
    public class Context
    {
        public int State;

        public void SideEffect1()
        {
            Console.WriteLine("Effect1");
        }

        public void SideEffect2()
        {
            Console.WriteLine("Effect2");
        }
    }
}
