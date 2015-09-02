using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Operating_System
    {
    public class RAM
        {
        public int total_size { get; set; }
        public int remain_size { get; set; }
        List<Process> T_Process;
        List<string> pageopcode = new List<string>(4);
        public bool changing = false;
        public RAM()
            {
            total_size = 1024;
            remain_size = 1024;
            T_Process = new List<Process>();
            }

        private void RecalculateRemain_Size(int prsize)
            {
            remain_size -= prsize;
            }
        public void Add_Process(Process p)
            {
            changing = true;
            RecalculateRemain_Size(p.Opcode_Size());
            T_Process.Add(p);
            }
        public int Total_Process()
            {
            return T_Process.Count;
            }
        public Process GetProcess(int pos)
            {
            return T_Process[pos];
            }

        public string Get_Opcode(int processloc, int opcodeloc)
            {
            changing = true;
            Process p = T_Process[processloc];
            return p.Opcode_Value(opcodeloc);
            }

        public void Remove_Process(int pos)
            {
            changing = true;
            remain_size += T_Process[pos].Opcode_Size();
            int jobnum = T_Process[pos].JobNumber;
            T_Process.RemoveAt(pos);
            Console.WriteLine("Remove Job Number " + jobnum + " out of RAM");
            }

        public void WriteData(int pos, int pc, string Value)
            {
            T_Process[pos].Write(pc, Value);
            }
        //wrote by Jun
        public int GetPercentage()
            {
            return ((total_size - remain_size) / total_size) * 100;
            }
        }
    }
