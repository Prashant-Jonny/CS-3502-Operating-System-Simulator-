using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Operating_System
{
    public class PCB
    {
        List<MMU> Device_Queues;
        RAM ram;
        public bool changing = false;
        public PCB() {
            Device_Queues = new List<MMU>();
        }
        public PCB(RAM r)
        {
            ram = r;
            Device_Queues = new List<MMU>();
        }

        public void AddQueue(MMU c)
        {
            changing = true;
            Device_Queues.Add(c);
        }

        public MMU GetQueueAt(int pos)
        {
            return Device_Queues[pos];
        }

        public void RemoveQueueAt(int pos)
        {
            changing = true;
            MMU c = Device_Queues[pos];
            c.ChangeState(MMU.STATE.terminated);
            ram.Remove_Process(c.posinRAM);
            foreach (MMU con in Device_Queues)
            {
                if (con.posinRAM > c.posinRAM)
                {
                    c.ReposQueue();
                }
            }
            ram.changing = false;
            changing = false;
        }

        public int TotalQueues()
        {
            return Device_Queues.Count;
        }
    }
}
