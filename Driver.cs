using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Operating_System
    {
    class Driver
        {
        Disk disk;
        RAM ram;
        PCB pcb;
        Dispatcher dis;
        DMA dma;
        CPU cpu;
        public List<CPU> totalCPU = new List<CPU>();
        private bool Complete = false;

        /*driver with option*/
        public Driver(string text)
            {
            string input = "";
            //do
            //    {
                Console.WriteLine("Enter 's' for sigle CPU, 'n' for 4-CPU");
                input = Console.ReadLine();

                if (input == "n")
                    {
                    disk = new Disk();
                    ram = new RAM();
                    pcb = new PCB(ram);
                    dis = new Dispatcher(ram, pcb);
                    dma = new DMA(dis, pcb);
                    Ultility.Loader(disk, text);
                    for (int i = 0; i < 4; i++)
                        {
                        totalCPU.Add(new CPU(dis, pcb));
                        }

                    Start();
                    Console.WriteLine("Operation Completed");
                    }

                else if (input == "s")
                    {
                    disk = new Disk();
                    ram = new RAM();
                    pcb = new PCB(ram);
                    dis = new Dispatcher(ram, pcb);
                    dma = new DMA(dis, pcb);
                    Ultility.Loader(disk, text);
                    cpu = new CPU(dis, pcb);
                    SingleCPUStart();
                    //  Start();
                    Console.WriteLine("Operation Completed");
                    }

                Console.WriteLine("Invalid input");
                //} while (input != "s" || input != "n");

            }




        //public Driver(string text)
        //    {
        //    disk = new Disk();
        //    ram = new RAM();
        //    pcb = new PCB(ram);
        //    dis = new Dispatcher(ram, pcb);
        //    dma = new DMA(dis, pcb);
        //    Ultility.Loader(disk, text);
        //    for (int i = 0; i < 4; i++)
        //        {
        //        totalCPU.Add(new CPU(dis, pcb));
        //        }

        //    Start();
        //    Console.WriteLine("Operation Completed");
        //    }

        //public Driver(string text)
        //    {
        //    disk = new Disk();
        //    ram = new RAM();
        //    pcb = new PCB(ram);
        //    dis = new Dispatcher(ram, pcb);
        //    dma = new DMA(dis, pcb);
        //    Ultility.Loader(disk, text);
        //    cpu = new CPU(dis, pcb);
        //    SingleCPUStart();
        //    //  Start();
        //    Console.WriteLine("Operation Completed");
        //    }

        public void Start()
            {
            for (int i = 0; i < 4; i++)
                {

                totalCPU[i].StartCPU();
                }
            int min = 0;
            while (!Complete)
                {
                for (int j = 1; j < totalCPU.Count; ++j)
                    {
                    if (totalCPU[min].readyqueue.Total() > totalCPU[j].readyqueue.Total())
                        min = j;
                    }
                Complete = Ultility.LongtermScheduler(disk, ram, pcb);
                Complete = Ultility.ShorttermScheduler(pcb, totalCPU[min].readyqueue) && Complete;

                }
            for (int i = 0; i < 4; i++)
                {
                if (totalCPU[i].readyqueue.Total() == 0 && totalCPU[i].Alive())
                    {
                    totalCPU[i].StopCPU();
                    }
                }
            }

        public void SingleCPUStart()
            {
            cpu.StartCPU();
            while (!Complete)
                {

                Complete = Ultility.LongtermScheduler(disk, ram, pcb);
                Complete = Ultility.ShorttermScheduler(pcb, cpu.readyqueue) && Complete;

                }
            }
        }
    }
