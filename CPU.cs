using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Operating_System
    {
    public class CPU
        {
        public DMA dma;
        public PCB pcb;
        public Dispatcher dis;
        public Readyqueue readyqueue;
        private Register register;
        private int opCode, type, S1_reg, S2_reg, D_reg, B_reg, Reg_1, Reg_2;
        long dataSection;
        long address;
        public int pc = 0;
        private string InputBuffer, OutputBuffer, TempBuffer;
        public bool working = false;
        Thread t;
        string opcodestring;
        public int IOcount=0;
        public CPU(Dispatcher d, PCB p)
            {
            dma = new DMA(d, p);
            pcb = p;
            readyqueue = new Readyqueue(this, pcb);
            t = new Thread(new ThreadStart(this.RunCPU));
            register = new Register();
            }

        public int GetPC()
            {
            return pc;
            }

        public void StartCPU()
            {
            t.Start();
            }

        public void StopCPU()
            {
            t.Abort();
            }

        public bool Alive()
            {
            return t.IsAlive;
            }
        public void RunCPU()
            {
            while (true)
                {
                Fetch();
                if (working == true)
                    {
                    Execute(Decode(opcodestring));
                    }
                }
            }

        public int Effective_Address(int B_reg, long address)
            {
            return register.Retrieve(B_reg) + (int)address;
            }


        private void Execute(int oc)
            {

            int pcbpos = 0;
            readyqueue.calculateWaitTime(pc);
            if (oc < 0 || oc > 26)
                {
                Console.WriteLine("Opcode is not found.");
                }
            else
                {
                Console.WriteLine("----------------Start Executing----------------");
                switch (oc)
                    {
                    case 0:
                        Console.WriteLine("--------------------RD--------------------");
                        Console.WriteLine("Reads Content of Input buffer into a accumulator.");
                        if (address > 0)
                            {
                            pcbpos = readyqueue.GetPCB_AtFront();
                            InputBuffer = dma.GetOpcode(pcbpos, WordAddress((int)address));
                            register.Assign(Ultility.ConvertBinaryToDecimal(InputBuffer), Reg_1);
                            IOcount++;
                            }
                        else
                            {
                            register.Assign(register.Retrieve(Reg_2), Reg_1);
                            IOcount++;
                            }
                        break;
                    case 1:
                        Console.WriteLine("--------------------WR--------------------");
                        Console.WriteLine("Writes the content of accumulator into Output buffer.");
                        OutputBuffer = Ultility.ConvertDecimalToBinary(register.Retrieve(Reg_1));
                        pcbpos = readyqueue.GetPCB_AtFront();
                        dma.writedata(pcbpos, WordAddress((int)address), OutputBuffer);
                        IOcount++;
                        break;
                    case 2:
                        Console.WriteLine("--------------------ST--------------------");
                        Console.WriteLine("Stores content of Register {0} into an address", D_reg);
                        address = register.Retrieve(D_reg);
                        break;
                    case 3:
                        Console.WriteLine("--------------------LW--------------------");
                        Console.WriteLine("Loads the content of an address into a register{0}", D_reg);
                        register.Assign(register.Retrieve((int)address), D_reg);
                        Console.WriteLine("Content at Register {0} now has value: {1}", D_reg, register.Retrieve(D_reg));
                        break;
                    case 4:
                        Console.WriteLine("--------------------MOV--------------------");
                        register.Assign(register.Retrieve(Reg_1), Reg_2);
                        break;
                    case 5:
                        Console.WriteLine("--------------------ADD--------------------");
                        Console.WriteLine("Adds content of two S-regs into D-reg");
                        register.Assign(register.Retrieve(S1_reg) + register.Retrieve(S2_reg), D_reg);
                        Console.WriteLine("Value of Register {0}: {1}", S1_reg, register.Retrieve(S1_reg));
                        Console.WriteLine("Value of Register {0}: {1}", S2_reg, register.Retrieve(S2_reg));
                        Console.WriteLine("Adding value...");
                        Console.WriteLine("Value of Register {0}: {1}", D_reg, register.Retrieve(D_reg));
                        break;
                    case 6:
                        Console.WriteLine("--------------------SUB--------------------");
                        Console.WriteLine("Subtracts content of two S-regs into D-reg");
                        register.Assign(register.Retrieve(Reg_1) - register.Retrieve(Reg_2), D_reg);
                        Console.WriteLine("Value of Register {0}: {1}", S1_reg, register.Retrieve(S1_reg));
                        Console.WriteLine("Value of Register {0}: {1}", S2_reg, register.Retrieve(S2_reg));
                        Console.WriteLine("Subtracting value...");
                        Console.WriteLine("Value of Register {0}: {1}", D_reg, register.Retrieve(D_reg));
                        break;
                    case 7:
                        Console.WriteLine("--------------------MUL--------------------");
                        Console.WriteLine("Multiplies content of two S-regs into D-reg");
                        register.Assign(register.Retrieve(Reg_1) * register.Retrieve(Reg_2), D_reg);
                        Console.WriteLine("Value of Register {0}: {1}", S1_reg, register.Retrieve(S1_reg));
                        Console.WriteLine("Value of Register {0}: {1}", S2_reg, register.Retrieve(S2_reg));
                        Console.WriteLine("Multiplying value...");
                        Console.WriteLine("Value of Register {0}: {1}", D_reg, register.Retrieve(D_reg));
                        break;
                    case 8:
                        Console.WriteLine("--------------------DIV--------------------");
                        Console.WriteLine("Divides content of two S-regs into D-reg");
                        if (register.Retrieve(S2_reg) == 0)
                            {
                            Console.WriteLine("Cannot divide by 0!");
                            }
                        else
                            {
                            register.Assign(register.Retrieve(Reg_1) / register.Retrieve(Reg_2), D_reg);
                            Console.WriteLine("Value of Register {0}: {1}", S1_reg, register.Retrieve(S1_reg));
                            Console.WriteLine("Value of Register {0}: {1}", S2_reg, register.Retrieve(S2_reg));
                            Console.WriteLine("Dividing value...");
                            Console.WriteLine("Value of Register {0}: {1}", D_reg, register.Retrieve(D_reg));
                            }
                        break;
                    case 9:
                        Console.WriteLine("--------------------AND--------------------");
                        Console.WriteLine("Logical AND of two S-regs into D-reg");
                        register.Assign(register.Retrieve(Reg_1) & register.Retrieve(Reg_2), D_reg);
                        Console.WriteLine("Value of Register {0}: {1}", S1_reg, register.Retrieve(S1_reg));
                        Console.WriteLine("Value of Register {0}: {1}", S2_reg, register.Retrieve(S2_reg));
                        Console.WriteLine("Performing Logical AND...");
                        Console.WriteLine("Value of Register {0}: {1}", D_reg, register.Retrieve(D_reg));
                        break;
                    case 10:
                        Console.WriteLine("--------------------OR--------------------");
                        Console.WriteLine("Logical OR of two S-regs into D-reg");
                        register.Assign(register.Retrieve(Reg_1) | register.Retrieve(Reg_2), D_reg);
                        Console.WriteLine("Value of Register {0}: {1}", S1_reg, register.Retrieve(S1_reg));
                        Console.WriteLine("Value of Register {0}: {1}", S2_reg, register.Retrieve(S2_reg));
                        Console.WriteLine("Performing Logical OR...");
                        Console.WriteLine("Value of Register {0}: {1}", D_reg, register.Retrieve(D_reg));
                        break;
                    case 11:
                        Console.WriteLine("--------------------MOVI--------------------");
                        Console.WriteLine("Transfers address/data directly into a register");
                        register.Assign(WordAddress((int)address), D_reg);
                        Console.WriteLine("Register {0} now has value: {1}", D_reg, register.Retrieve(D_reg));
                        break;
                    case 12:
                        Console.WriteLine("--------------------ADDI--------------------");
                        Console.WriteLine("Adds a data directly to the content of a register");
                        register.Assign((int)address + register.Retrieve(D_reg), D_reg);
                        Console.WriteLine("Register {0} now has value: {1}", D_reg, register.Retrieve(D_reg));
                        break;
                    case 13:
                        Console.WriteLine("--------------------MULI--------------------");
                        Console.WriteLine("Multiplies a data directly to the content of a register");
                        register.Assign((int)address * register.Retrieve(D_reg), D_reg);
                        Console.WriteLine("Register {0} now has value: {1}", D_reg, register.Retrieve(D_reg));
                        break;
                    case 14:
                        Console.WriteLine("--------------------DIVI--------------------");
                        if (register.Retrieve(D_reg) == 0)
                            {
                            Console.WriteLine("Cannot divide by 0!");
                            }
                        else
                            {
                            Console.WriteLine("Divides a data directly to the content of a register");
                            register.Assign((int)address / register.Retrieve(D_reg), D_reg);
                            Console.WriteLine("Register {0} now has value: {1}", D_reg, register.Retrieve(D_reg));
                            }
                        break;

                    case 15:
                        Console.WriteLine("--------------------LDI--------------------");
                        Console.WriteLine("Loads a data/address directly to the content of a register");
                        register.Assign((int)address, D_reg);
                        Console.WriteLine("Value at Register {0} = {1}", D_reg, register.Retrieve(D_reg));
                        break;

                    case 16:
                        Console.WriteLine("--------------------SLT--------------------");
                        Console.WriteLine("Sets the D-reg to 1 if  first S-reg is less than second B-reg, and 0 otherwise");
                        if (register.Retrieve(S1_reg) < register.Retrieve(S2_reg))
                            {
                            register.Assign(1, D_reg);
                            Console.WriteLine("Value at Register {0} = {1}", D_reg, register.Retrieve(D_reg));
                            }
                        else
                            {
                            register.Assign(0, D_reg);
                            Console.WriteLine("Value at Register {0} = {1}", D_reg, register.Retrieve(D_reg));
                            }
                        break;
                    case 17:
                        Console.WriteLine("--------------------SLTI--------------------");
                        Console.WriteLine("Sets the D-reg to 1 if  first S-reg is less than a data, and 0 otherwise");
                        if (register.Retrieve(S1_reg) < (int)address)
                            {
                            register.Assign(1, D_reg);
                            Console.WriteLine("Value at Register {0} = {1}", D_reg, register.Retrieve(D_reg));
                            }
                        else
                            {
                            register.Assign(0, D_reg);
                            Console.WriteLine("Value at Register {0} = {1}", D_reg, register.Retrieve(D_reg));
                            }
                        break;
                    case 18:
                        Console.WriteLine("--------------------HLT--------------------");
                        Console.WriteLine("Logical end of program");
                        readyqueue.RemoveFrontPCB();
                        pc = 0;
                        //StopCPU();
                        Console.WriteLine("Total I/O count for this job is: "+IOcount);
                        IOcount = 0;
                        break;
                    case 19:
                        Console.WriteLine("--------------------NOP--------------------");
                        Console.WriteLine("Moving to the next instruction");
                        break;
                    case 20:
                        Console.WriteLine("--------------------JMP--------------------");
                        Console.WriteLine("\nJumping to another location");
                        pc = (int)address / 4;
                        Console.WriteLine("\nProgram counter set to " + pc);
                        break;
                    case 21:
                        Console.WriteLine("--------------------BEQ--------------------");
                        Console.WriteLine("Branches to an address when content of B-reg = D-reg");
                        if (register.Retrieve(D_reg) == register.Retrieve(B_reg))
                            {
                            pc = (int)address / 4;
                            Console.WriteLine("Program counter set to " + pc);
                            }
                        else
                            {
                            Console.WriteLine("Program counter set to " + pc);
                            }
                        break;
                    case 22:
                        Console.WriteLine("--------------------BNE--------------------");
                        Console.WriteLine("Branches to an address when content of B-reg <> D-reg");
                        if (register.Retrieve(D_reg) != register.Retrieve(B_reg))
                            {
                            pc = (int)address / 4;
                            Console.WriteLine("Program counter set to " + pc);
                            }
                        else
                            {
                            Console.WriteLine("Program counter set to " + pc);
                            }
                        break;
                    case 23:
                        Console.WriteLine("--------------------BEZ--------------------");
                        Console.WriteLine("Branches to an address when content of B-reg =0");
                        if (register.Retrieve(B_reg) == 0)
                            {
                            pc = (int)address / 4;
                            Console.WriteLine("Program counter set to " + pc);
                            }
                        else
                            {
                            Console.WriteLine("Program counter set to " + pc);
                            }
                        break;
                    case 24:
                        Console.WriteLine("--------------------BNZ--------------------");
                        Console.WriteLine("Branches to an address when content of B-reg =0");
                        if (register.Retrieve(B_reg) != 0)
                            {
                            pc = (int)address / 4;
                            Console.WriteLine("Program counter set to " + pc);
                            }
                        else
                            {
                            Console.WriteLine("Program counter set to " + pc);
                            }
                        break;
                    case 25:
                        Console.WriteLine("--------------------BGZ--------------------");
                        Console.WriteLine("Branches to an address when content of B-reg =0");
                        if (register.Retrieve(B_reg) > 0)
                            {
                            pc = (int)address / 4;
                            Console.WriteLine("Program counter set to " + pc);
                            }
                        else
                            {
                            Console.WriteLine("Program counter set to " + pc);
                            }
                        break;
                    case 26:
                        Console.WriteLine("--------------------BLZ--------------------");
                        Console.WriteLine("Branches to an address when content of B-reg =0");
                        if (register.Retrieve(B_reg) < 0)
                            {
                            pc = (int)address / 4;
                            Console.WriteLine("Program counter set to " + pc);
                            }
                        else
                            {
                            Console.WriteLine("Program counter set to " + pc);
                            }
                        break;
                    }
                }
            }

        private int WordAddress(int address)
            {
            return address / 4;
            }

        private int Decode(string oc)
            {
            Console.WriteLine("----------------Start Decoding----------------");
            Console.WriteLine("Decode this binary instruction: " + oc);
            type = Ultility.ConvertBinaryToDecimal(oc.Substring(0, 2));
            opCode = Ultility.ConvertBinaryToDecimal(oc.Substring(2, 6));
            Console.WriteLine("Instruction Type: {0}", type);
            Console.Write("Instruction Format:");
            switch (type)
                {
                case 0:
                    Console.WriteLine("Arithmetic Instruction Format");
                    S1_reg = Ultility.ConvertBinaryToDecimal(oc.Substring(8, 4));
                    S2_reg = Ultility.ConvertBinaryToDecimal(oc.Substring(12, 4));
                    D_reg = Ultility.ConvertBinaryToDecimal(oc.Substring(16, 4));
                    dataSection = Ultility.ConvertBinaryToDecimal(oc.Substring(20, 12));
                    Console.WriteLine(" Value of Opcode: {0}", opCode);
                    Console.WriteLine(" Value of Source Register {1}: {0}", register.Retrieve(S1_reg), Reg_1);
                    Console.WriteLine(" Value of Source Register {1}: {0}", register.Retrieve(S2_reg), Reg_2);
                    Console.WriteLine(" Value of Destination Register {1}: {0}", register.Retrieve(D_reg), D_reg);
                    if (dataSection > 0)
                        {
                        Console.WriteLine(" Address Value: {0}", dataSection);
                        }
                    break;
                case 1:
                    Console.WriteLine("Conditional Branch and Immediate format");
                    B_reg = Ultility.ConvertBinaryToDecimal(oc.Substring(8, 4));
                    D_reg = Ultility.ConvertBinaryToDecimal(oc.Substring(12, 4));
                    address = Ultility.ConvertBinaryToDecimal(oc.Substring(16, 16));
                    Console.WriteLine(" Value of Opcode: {0}", opCode);
                    Console.WriteLine(" Value of Branch Register: {0}", register.Retrieve(B_reg));
                    Console.WriteLine(" Value of Destination Register: {0}", register.Retrieve(D_reg));
                    Console.WriteLine(" Address Value: {0}", address);
                    break;
                case 2:
                    Console.WriteLine("Unconditional Jump format");
                    address = Ultility.ConvertBinaryToDecimal(oc.Substring(8, 24));
                    Console.WriteLine(" Value of Opcode: {0}", opCode);
                    Console.WriteLine(" Address Value: {0}", address);
                    break;
                case 3:
                    Console.WriteLine("Input and Output instruction format");
                    Reg_1 = Ultility.ConvertBinaryToDecimal(oc.Substring(8, 4));
                    Reg_2 = Ultility.ConvertBinaryToDecimal(oc.Substring(12, 4));
                    address = Ultility.ConvertBinaryToDecimal(oc.Substring(16, 16));
                    Console.WriteLine(" Value of Opcode: {0}", opCode);
                    Console.WriteLine(" Value of Register {1}: {0}", register.Retrieve(Reg_1), Reg_1);
                    Console.WriteLine(" Value of Register {1}: {0}", register.Retrieve(Reg_2), Reg_2);
                    Console.WriteLine(" Address Value: {0}", address);
                    break;
                default:
                    Console.WriteLine("*******************************************");
                    Console.WriteLine("Error! Format type does not exist.");
                    Console.WriteLine("*******************************************");
                    break;
                }
            return opCode;
            }

        private void Fetch()
            {
            working = false;
            if (readyqueue.Total() > 0)
                {
                int frontpos = readyqueue.GetPCB_AtFront();
                if (frontpos == -1)
                    {
                    return;
                    }
                Console.WriteLine("----------------Start Fetching----------------");
                opcodestring = dma.GetOpcode(frontpos, pc++);
                working = true;
                }
            }
        }
    }
