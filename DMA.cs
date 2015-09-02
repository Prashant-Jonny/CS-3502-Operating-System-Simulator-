using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Operating_System
{
    public class DMA
    {
        Dispatcher dis;
        PCB pcb;
        public bool working = false;

        public DMA(Dispatcher d, PCB b)
        {
            dis = d;
            pcb = b;
        }

        public string GetOpcode(int pcbl, int pc)
        {
            //----call page table in MMU------
            string opcode = pcb.GetQueueAt(pcbl).GetPageOpcode(pc); //phase 2
            //--------------------------------
            //string opcode = dis.GET_OPCODE(pcbl, pc); //phase 1
            return opcode;
        }

        public void writedata(int pcbidx, int pc, string Value)
        {
            dis.ram.WriteData(pcb.GetQueueAt(pcbidx).posinRAM, pc, Value);
        }
    }
}
