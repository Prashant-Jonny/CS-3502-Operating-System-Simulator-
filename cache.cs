sing System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operating_System
    {
    class cache
        {
        int[] registerCopy;
        Process process;
        public cache()
            {
            //initialize registerCopy 
            registerCopy = new int[16];
            process = new Process();

            }
        public void copy(Register reg)
            {
            for (int i = 0; i < registerCopy.Count(); i++)
                {
                registerCopy[i] = reg.Retrieve(i);
                }
            }
        public Register resume()
            {
            Register reg = new Register();
            for (int i = 0; i < 16; i++)
                {
                reg.Assign(registerCopy[i], i);
                }

            return reg;
            }

        }
    }
