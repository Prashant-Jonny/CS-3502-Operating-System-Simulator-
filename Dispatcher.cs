using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Operating_System
    {
    public class Dispatcher
        {
        public RAM ram;
        public PCB pcb;
        public bool working = false;

        public Dispatcher()
            {
            }

        public Dispatcher(RAM r, PCB p)
            {
            ram = r;
            pcb = p;
            }

        public string GET_OPCODE(int pcbl, int pc)
            {
            working = true;
            while (ram.changing) ;
            string op = ram.Get_Opcode(pcb.GetQueueAt(pcbl).posinRAM, pc);
            ram.changing = false;
            return op;
            }
        }
    }
