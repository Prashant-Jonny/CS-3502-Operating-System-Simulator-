using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Operating_System
    {
    public class Readyqueue
        {
        List<int> totalPCB;
        CPU cpu;
        PCB pcb;
        TimeSpan compTime;
        TimeSpan waitTime;
        List<string> compTimeList;
        List<string> waitTimeList;
        public Readyqueue(CPU c, PCB b)
            {
            cpu = c;
            pcb = b;
            totalPCB = new List<int>();
            compTimeList = new List<string>();
            waitTimeList = new List<string>();
            }

        public int Total()
            {
            return totalPCB.Count;
            }

        public int GetPCB_AtFront()
            {
            //if (cpu.pc == pcb.GetQueueAt(totalPCB[0]).pc)
            //{
            //    pcb.RemoveQueueAt(totalPCB[0]);
            //    RemoveFrontPCB();
            //    //organize condition
            //}
            return totalPCB.Count == 0 ? -1 : totalPCB[0];
            }

        public void AddPCB(int i)
            {
            totalPCB.Add(i);

            //organize condition

            //start timer
            compTime = DateTime.Now.TimeOfDay;
            waitTime = DateTime.Now.TimeOfDay;
            }

        public void RemoveFrontPCB()
            {
            Console.WriteLine("Removing Job " + pcb.GetQueueAt(totalPCB[0]).Jobnum);

            Console.WriteLine("FIFO_Completion Time for JOB" + pcb.GetQueueAt(totalPCB[0]).Jobnum + ": " + (DateTime.Now.TimeOfDay - compTime).ToString());
            compTimeList.Add(("FIFO_Completion Time for JOB" + pcb.GetQueueAt(totalPCB[0]).Jobnum + ": " + (DateTime.Now.TimeOfDay - compTime).ToString()));

            totalPCB.RemoveAt(0);

            if (totalPCB.Count() == 0)
                {
                int i = 0;
                for (i = 0; i < compTimeList.Count; i++)
                    {
                    try
                        {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter("CompletionTime_FIFO.txt", true))
                            {
                            file.WriteLine(compTimeList[i]);
                            file.Close();
                            }
                        }
                    catch (ArgumentOutOfRangeException)
                        {
                        //ignore it
                        }
                    }

                for (i = 0; i < waitTimeList.Count; i++)
                    {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("WaitingTime_FIFO.txt", true))
                        {
                        file.WriteLine(waitTimeList[i]);
                        file.Close();
                        }
                    }


                Console.WriteLine("Operation Completed");
                string input= "";
                input=Console.ReadLine();
                if(input=="exit")
                    {
                     Environment.Exit(0);
                    }

                }
            }

        public void calculateWaitTime(int pc)
            {
            if (pc == 1)
                {
                if (waitTime != null)
                    {
                    Console.WriteLine("FIFO_Waiting Time for JOB" + pcb.GetQueueAt(totalPCB[0]).Jobnum + ": " + (DateTime.Now.TimeOfDay - waitTime).ToString());
                    waitTimeList.Add(("FIFO_Waiting Time for JOB" + pcb.GetQueueAt(totalPCB[0]).Jobnum + ": " + (DateTime.Now.TimeOfDay - waitTime).ToString()));
                    }
                }
            else
                {
                return;
                }
            }

        private void PriorityOrganize()
            {
            while (cpu.working) ;
            for (int i = 0; i < totalPCB.Count; i++)
                {
                for (int j = 1; j < totalPCB.Count; j++)
                    {
                    if (pcb.GetQueueAt(totalPCB[i]).priority < pcb.GetQueueAt(totalPCB[j]).priority)
                        {
                        int temp = totalPCB[i];
                        totalPCB[i] = totalPCB[j];
                        totalPCB[j] = temp;
                        }
                    }
                }
            }
        private void ShortestJobFirst()
            {
            while (cpu.working) ;
            for (int i = 0; i < totalPCB.Count; i++)
                {
                for (int j = 1; j < totalPCB.Count; j++)
                    {
                    if (pcb.GetQueueAt(totalPCB[i]).totalinstruction > pcb.GetQueueAt(totalPCB[j]).totalinstruction)
                        {
                        int temp = totalPCB[i];
                        totalPCB[i] = totalPCB[j];
                        totalPCB[j] = temp;
                        }
                    }
                }
            }
        }
    }
