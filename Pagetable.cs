using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operating_System
    {
    public class Pagetable
        {
        public page[] table;

        public Pagetable()
            {
            table = new page[4];
            }

        public page getpage(int pos)
            {
            return table[pos];
            }

        public void writepage(int pagenum, int remainder, int op)
            { int i;
            
            for(i=0; i<4; i++)
                {
                if(table[i].PageNumber==pagenum)
                    {
                    break;
                    }
                }
            table[i].words[remainder] = new Process.data(op/4, op%4,Ultility.ConvertDecimalToBinary(op));
            }

        public Boolean page_exist(int pos)
            {
            for(int i=0;i<4;i++)
                {
                if(table[i].PageNumber==pos)
                    {
                    return true;
                    }
                }
            return false;
            }

        public Process.data get_data(int quotient, int remainder)
            {
            int i;
            for (i = 0; i < 4; i++)
                {
                if (table[i].PageNumber == quotient)
                    {
                    break;
                    }
                }
            table[i].frequency++;
            return table[i].get_word(remainder);
            }


        public bool quotient_exist(int quotient)
            {

            for (int i = 0; i < 4; i++)
                {
                if (table[i] != null && table[i].PageNumber == quotient)
                    {
                    Console.WriteLine("Page number {0} located at the position {1} in the page table", quotient, i);
                    return true;
                    }
                }
            Console.WriteLine("Page number {0} is not currently in the page table", quotient);
            return false;
            }

        public void pageswap(page newpage)
            {
            Console.WriteLine("Swaping Opcode...");
            for (int pos = 0; pos < 4; pos++)
                {
                if (table[pos] == null)
                    {
                    table[pos] = newpage;
                    return;
                    }
                }
            int smallest = table[0].frequency;
            int swapwith = 0;
            for (int i = 0; i < 4; i++)
                {
                if (smallest > table[i].frequency)
                    {
                    smallest = table[i].frequency;
                    swapwith = i;
                    }
                }
            table[swapwith] = newpage;

            }
        public page leastused()
            {
            int smallest = table[0].frequency;
            int leastused = 0;
            for (int i = 0; i < 4; i++)
                {
                if (smallest > table[i].frequency)
                    {
                    smallest = table[i].frequency;
                    leastused = i;
                    }
                }
            return table[leastused];
            }

        public Boolean isfull()
            {

            for (int i = 0; i < 4; i++)
                {
                if (table[i] == null)
                    {
                    return false;
                    }
                }
            return false;
            }

        }
    public class page
        {
        //public int index { get; set; }
        public int PageNumber { get; set; }
        public Process.data[] words;
        public int frequency { get; set; }

        public page()
            {
            //index = 0;
            PageNumber = 0;
            words = new Process.data[4];
            frequency = 0;
            }

        public page(int qu)
            {
            //index = 0;
            PageNumber = qu;
            words = new Process.data[4];
            frequency = 0;
            }
        public void set_word(Process.data word)
            {
            words[word.remainder] = word;
            }
        public Process.data get_word(int remainder)
            {
            return words[remainder];
            }
        public Process.data get_process(int remainder)
            {
            return words[remainder];
            }

        }
    }
