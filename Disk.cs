using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operating_System
{
    public class Disk
    {
        int totalsize { get; set; }
        int remainsize { get; set; }
        List<Process> Processes;
        public Disk()
        {
            totalsize = 2048;
            remainsize = 2048;
            Processes = new List<Process>();
        }

        void RecalculateRemainderSize()//int prsize)
        {
            remainsize--;
        }

        public void AddProcess(Process p)
        {
            RecalculateRemainderSize();//p.Opcode_Size());
            Processes.Add(p);
        }

        public int TotalProcess()
        {
            return Processes.Count;
        }

        public Process GetProcess(int pc)
        {
            return Processes[pc];
        }
    }
}
