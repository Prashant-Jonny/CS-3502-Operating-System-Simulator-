using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operating_System
{
    class Register
    {
        int[] CPURegister;
        public Register()
        {
            CPURegister = new int[16];
            for (int i = 0; i < CPURegister.Count(); i++)
            {
                CPURegister[i] = 0;
            }
        }

        public void Assign(int AssignVal, int pos)
        {
            if (pos == 1)
            {
                return;
            }
            else if (pos < CPURegister.Count())
            {
                CPURegister[pos] = AssignVal;
            }
            else
            {
                Console.WriteLine("Invalid Register.");
            }
        }

        public int Retrieve(int pos)
        {
            return CPURegister[pos];
        }
    }
}

