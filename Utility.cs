using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operating_System
    {
    public static class Ultility
        {
        public static CPU _cpu;
        private static int pc = 0;

        static Ultility() { }

        public static int ConvertHexToDecimal(string hex)
            {
            return int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            }

        public static string ConvertHexToBinary(string hex)
            {
            //return String.Join(String.Empty, hex.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
            String entry = String.Join(String.Empty, hex.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
            Console.WriteLine(entry);
            return entry;
            }

        public static int ConvertBinaryToDecimal(string bin)
            {
            return Convert.ToInt32(bin, 2);
            }

        public static string ConvertDecimalToBinary(int dec)
            {
            string bin = Convert.ToString(dec, 2);
            while (bin.Length < 10)
                {
                bin = "0" + bin;
                }
            return bin;
            }

        public static void Loader(Disk d, string InputFile)
            {
            string line;
            bool DataOpcode = false;
            Process p = new Process();
            StreamReader sr = new StreamReader(InputFile);
            if (!File.Exists(InputFile))
                {
                Console.WriteLine("File is not existed.");
                }
            else
                {
                try
                    {
                    while ((line = sr.ReadLine()) != null)
                        {
                        Console.WriteLine(line);
                        if (line[0] == '/')
                            {
                            if (line.Contains("// JOB"))
                                {
                                p = new Process();
                                DataOpcode = false;
                                string[] split = SetString(line);
                                p.Set_Job_Info(ConvertHexToDecimal(split[2]), ConvertHexToDecimal(split[3]), ConvertHexToDecimal(split[4]));
                                }
                            else if (line.Contains("// Data"))
                                {
                                DataOpcode = true;
                                string[] split = SetString(line);
                                p.Set_Data_info(ConvertHexToDecimal(split[2]), ConvertHexToDecimal(split[3]), ConvertHexToDecimal(split[4]));
                                }
                            else if (line == "// END" || line == "//END")
                                {
                                Console.WriteLine("Finished Loading Job Number " + p.JobNumber);
                                d.AddProcess(p);
                                }
                            }
                        else
                            {
                            if (DataOpcode == false)
                                {
                                string[] hex = line.Split('x');
                                p.Add_Job_opcode(ConvertHexToBinary(hex[1]));
                                // Console.WriteLine("Process Number:"+ p.JobNumber);
                                //Console.WriteLine("Job opcode:"+ line);
                                }
                            else
                                {
                                string[] hex = line.Split('x');
                                p.Add_Data_opcode(ConvertHexToBinary(hex[1]));
                                //Console.WriteLine("Process Number:" + p.JobNumber);
                                //Console.WriteLine("Job opcode:" + line);
                                }

                            }
                        }

                    }
                catch (Exception e)
                    {
                    Console.WriteLine(e);
                    }
                finally
                    {
                    sr.Close();
                    }
                }
            }

        private static string[] SetString(string line)
            {
            string[] words = line.Split(' ');
            return words;
            }

        public static bool LongtermScheduler(Disk d, RAM r, PCB p)
            {
            if (pc < d.TotalProcess() && r.remain_size >= d.GetProcess(pc).Opcode_Size())
                {
                while (r.changing) ;
                while (p.changing) ;
                //add to RAM
                r.Add_Process(d.GetProcess(pc));
                //create a queue to hold process id and position
                MMU q = new MMU(d.GetProcess(pc), r.Total_Process() - 1);
                //Add queue to pcb
                p.AddQueue(q);
                Console.WriteLine("Added job to RAM: " + d.GetProcess(pc++).JobNumber);
                r.changing = false;
                p.changing = false;
                return false;
                }
            else
                {
                return true;
                }
            }

        public static bool ShorttermScheduler(PCB p, Readyqueue u)
            {
            if (p.TotalQueues() > 0)
                {
                int i = 0;
                for (i = 0; i < p.TotalQueues(); i++)
                    {
                    if (p.GetQueueAt(i).state == MMU.STATE.waiting)
                        {
                        break;
                        }
                    else if (i + 1 == p.TotalQueues())
                        {
                        return true;
                        }
                    }
                p.GetQueueAt(i).ChangeState(MMU.STATE.ready);
                Console.WriteLine("Placing job " + p.GetQueueAt(i).Jobnum + " from device queue to ready queue");
                //add to ready queue
                u.AddPCB(i);
                return false;
                }
            return false;
            }
        }
    }
