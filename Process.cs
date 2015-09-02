using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operating_System
    {
    public class Process
        {
        List<string> Job_opcode;
        List<string> Data_opcode;
        public List<page> total_page;
        public int JobNumber { get; set; }
        public int NumofInstruction { get; set; }
        public int priority { get; set; }
        public int inputBufferSize { get; set; }
        public int OutputbufferSize { get; set; }
        public int TempBufferSize { get; set; }
        public int DataAddr { get; set; }
        public Process()
            {
            JobNumber = 0;
            NumofInstruction = 0;
            priority = 0;
            inputBufferSize = 0;
            OutputbufferSize = 0;
            TempBufferSize = 0;
            DataAddr = 0;
            Job_opcode = new List<string>();
            Data_opcode = new List<string>();
            total_page = new List<page>();
            }

        public void Set_Job_Info(int jobnum, int instrtotal, int pr)
            {
            JobNumber = jobnum;
            NumofInstruction = instrtotal;
            priority = pr;
            }

        public void Set_Data_info(int input, int output, int temp)
            {
            inputBufferSize = input;
            OutputbufferSize = output;
            TempBufferSize = temp;
            }

        public void Add_Job_opcode(string op)
            {
            Job_opcode.Add(op);
            //added by Jun from here
            //data dataline = new data(Ultility.ConvertHexToDecimal(op) / 4,
            //                         Ultility.ConvertHexToDecimal(op) % 4,
            //                         op);
            //DataList.Add(dataline);
            //to here
            int pagenum = (Job_opcode.Count - 1) / 4;
            if (total_page.Count == pagenum)
                {
                total_page.Add(new page(total_page.Count));
                total_page[total_page.Count - 1].set_word(new data((Job_opcode.Count - 1) / 4,
                                                                   (Job_opcode.Count - 1) % 4,
                                                                    op));
                Console.WriteLine("current opcode is " + op);

                Console.WriteLine("page number {0} now contain:", total_page.Count - 1);
                foreach (Process.data d in total_page[total_page.Count - 1].words)
                    {
                    if (d != null)
                        Console.WriteLine("opcode {1} at position {0}", d.remainder, d.word);
                    }
                }
            else
                {
                total_page[total_page.Count - 1].set_word(new data((Job_opcode.Count - 1) / 4,
                                                                   (Job_opcode.Count - 1) % 4,
                                                                    op));
                Console.WriteLine("current opcode is " + op);

                Console.WriteLine("page number {0} now contain:", total_page.Count - 1);
                foreach (Process.data d in total_page[total_page.Count - 1].words)
                    {
                    if (d != null)
                        Console.WriteLine("opcode {1} at position {0}", d.remainder, d.word);
                    }
                }
            }

        public void Add_Data_opcode(string op)
            {
            DataAddr = Data_opcode.Count;
            Data_opcode.Add(op);

            if (total_page.Count == ((Job_opcode.Count - 1 + Data_opcode.Count) / 4))
                {
                total_page.Add(new page(total_page.Count));
                total_page[total_page.Count - 1].set_word(new data((Job_opcode.Count - 1 + Data_opcode.Count) / 4,
                                                                   (Job_opcode.Count - 1 + Data_opcode.Count) % 4,
                                                                    op));
                Console.WriteLine("current opcode is " + op);

                Console.WriteLine("page number {0} now contain:", total_page.Count - 1);
                foreach (Process.data d in total_page[total_page.Count - 1].words)
                    {
                    if (d != null)
                        Console.WriteLine("opcode {1} at position {0}", d.remainder, d.word);
                    }
                }
            else
                {
                total_page[total_page.Count - 1].set_word(new data((Job_opcode.Count - 1 + Data_opcode.Count) / 4,
                                                                   (Job_opcode.Count - 1 + Data_opcode.Count) % 4,
                                                                    op));
                Console.WriteLine("current opcode is " + op);

                Console.WriteLine("page number {0} now contain:", total_page.Count - 1);
                foreach (Process.data d in total_page[total_page.Count - 1].words)
                    {
                    if (d != null)
                        Console.WriteLine("opcode {1} at position {0}", d.remainder, d.word);
                    }
                }
            }

        public int Opcode_Size()
            {
            int Size = Job_opcode.Count + Data_opcode.Count;
            return Size;
            }
        public string Opcode_Value(int pc)
            {
            return (pc <= Job_opcode.Count - 1 ? Job_opcode[pc] : Data_opcode[pc - Job_opcode.Count]);
            }

        public void Write(int pos, string NewValue)
            {
            if (pos < Data_opcode.Count)
                {
                Data_opcode[pos] = NewValue;
                }
            else
                {
                Data_opcode[pos - Data_opcode.Count] = NewValue;
                }
            }
        public void PageWriteBack(page p)
            {
            total_page[p.PageNumber] = p;
            }
        //wrote by Jun
        public void writebackRAM(page swaped)
            {
            total_page.Insert(swaped.PageNumber, swaped);
            }

        //wrote by Jun
        public class data
            {

            public int quotient { get; set; }
            public int remainder { get; set; }
            public string word { get; set; }

            public data()
                {
                quotient = 0;
                remainder = 0;
                word = "";
                }
            public data(int qua, int rem, string dt)
                {
                quotient = qua;
                remainder = rem;
                word = dt;
                }

            }
        }
    }
