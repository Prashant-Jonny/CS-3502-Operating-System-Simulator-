using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Operating_System
    {
    public class MMU
        {
        public enum STATE
            {
            ready = 0,
            waiting = 1,
            executing = 2,
            terminated = 3
            };
        Process process;
        public int posinRAM { get; set; }
        public int Jobnum { get; set; }
        public int totalinstruction { get; set; }
        public int priority { get; set; }
        public int IBsize { get; set; }
        public int OBsize { get; set; }
        public int TBsize { get; set; }
        public int totalopcode { get; set; }
        public STATE state { get; set; }
        public int pc;
        Pagetable page;
        public MMU(Process p, int pos)
            {
            posinRAM = pos;
            Jobnum = p.JobNumber;
            totalinstruction = p.NumofInstruction;
            totalopcode = p.Opcode_Size();
            priority = p.priority;
            IBsize = p.inputBufferSize;
            OBsize = p.OutputbufferSize;
            TBsize = p.TempBufferSize;
            state = STATE.waiting;
            process = p;
            page = new Pagetable();
            }

        public void ChangeState(STATE t)
            {
            state = t;
            }

        public void GETPC(int p)
            {
            pc = p;
            }

        public void ReposQueue()
            {
            posinRAM--;
            }

        public string GetPageOpcode(int pc)
            {
            Console.WriteLine("Require opcode at pos {0} in RAM", pc);
            Console.WriteLine("Opcode is currently at frame number {0} and position {1} in RAM", pc / 4, pc % 4);

            if (page.quotient_exist(pc / 4))
                {
                return page.get_data(pc / 4, pc % 4).word;
                }

            if (page.isfull())
                {
                page leastused = page.leastused();//Jun 
                process.writebackRAM(leastused);//Jun 
                }
            page.pageswap(process.total_page[pc / 4]);


            return page.get_data(pc / 4, pc % 4).word;
            }

        public void writepage(int pagenum, int remainder, int op)
            {

               if(!page.page_exist(pagenum)&&!page.isfull())
                   {
                   page.pageswap(process.total_page[pagenum]);
                   page.writepage(pagenum,remainder,op);
                   }

               else if (!page.page_exist(pagenum) && page.isfull())
                   {
                   page.writepage(pagenum,remainder, op);
                   page.pageswap(process.total_page[pagenum]);
                   page.writepage(pagenum,remainder, op);
                   }
               else
                   {
                   page.writepage(pagenum, remainder, op);
                   }
            }
        }
    }
