using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assembler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Assembler Code";
            ofd.Filter = "Text File|*.txt";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text File|*.txt";
            sfd.FileName = "AssemblerOutput"; // Change to object code?
            sfd.Title = "Save Text File"; // The title of the Write location dialog, 
            string output = string.Empty;

            // Dictionary for storing opcodes
            Dictionary<string, string> OPCODE = new Dictionary<string, string>()
            {
                { "LDUR", "11111000010"},
                { "STUR", "11111000000"},
                { "ADD", "10001011000"},
                { "SUB", "11001011000"},
                { "ORR", "10101010000"},
                { "AND", "10001010000"},
                { "CBZ", "10110100"},
                { "B", "000101"},
                { "MOVZ", "110100101"},
                { "0:", "00"},
                { "16:", "01"},
                { "48:", "11"},
                { "ADDI", "10010001001"}
            };


            List<string> SymbolTableLabel = new List<string>();
            List<int> SymbolTableAdress = new List<int>();

            // Read in the file for stuff
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader sr = new StreamReader(File.OpenRead(ofd.FileName));
                List<string> lines = new List<string>();
                lines = File.ReadAllLines(ofd.FileName).ToList();
                //Computable = File.ReadAllLines(ofd.FileName).ToString().Replace()

                int NumberAddress = 0x10000000;
                int NumberAddress2 = 0x00400000;
                int i = 0;
                //int j = 0;
                //int z = 0;
                List<string> MachineLanguage = new List<string> { "00000000", "00000023", "FFFFFFFF", "FFFFFFEC", "33AAF315", "4555D499" };
                // List<string> MachineLanguage2 = new List<string> { "8B030041", "CB0503F3", "Line contains an Error, moving on...", "8B030021", };


                foreach (String line in lines)
                { // reading each line, main loop, can do operations in here

                    if (line.Contains("Number"))
                    {
                        SymbolTableLabel.Add("Number");
                        SymbolTableAdress.Add(NumberAddress);
                    }

                    else if (line.Contains("Begin"))
                    {
                        SymbolTableLabel.Add("Begin");
                        SymbolTableAdress.Add(NumberAddress2);
                    }

                    else if (line.Contains("Begin"))
                    {
                        SymbolTableLabel.Add("Begin");
                        SymbolTableAdress.Add(NumberAddress2);
                    }

                    else if (line.Contains("Loop"))
                    {
                        SymbolTableLabel.Add("Loop");
                        SymbolTableAdress.Add(NumberAddress2);
                    }
                    else if (line.Contains("Save"))
                    {
                        SymbolTableLabel.Add("Save");
                        SymbolTableAdress.Add(NumberAddress2);
                    }



                    // Ignores Comments.
                    if (line.Contains("//"))
                    {
                        // need to strike // from the string for rest of the function to get to work
                        //int lineIndex = line.IndexOf("//");
                        string input = line.ToString();
                        //input = input.Replace("\t", " ");
                        int index = input.IndexOf("//");    //change to single /?
                        if (index > 0)
                            output += "\n" + line.ToString().Substring(0, index);
                    }
                    else
                    {
                        output += "\n" + line.ToString() + "\t\t";
                    }

                    
                    //SymbolTableLabel.Add(");

                    if (line.Contains("Number:"))//  .doubleword 35, "))//-20, 0x33AAF3154555D499"))
                    {
                        //SymbolTableLabel.Add("Number");
                        //SymbolTableAdress.Add(NumberAddress);
                        while (NumberAddress <= 0x10000014)
                        {
                            output += "\n\t\t\t\t\t" + NumberAddress.ToString("X") + "    " + MachineLanguage[i];   //Next need to print the Machine Language Equivalent next to it
                            NumberAddress += 4;
                            i++;
                        }
                    }


                    else{
                                string raw_instruction = line.ToString();
                                // output += raw_instruction + "Here"; Does print!, now about those operations...
                                string formatted_instruction = raw_instruction.Replace(" ", ",").Replace("]", string.Empty).Replace("[", string.Empty).Replace(".text", string.Empty).Replace("Begin:", string.Empty).Replace("Loop:", string.Empty).Replace("Save:", string.Empty).Replace("\t", ",").Replace("\n", string.Empty).Replace("\r", string.Empty);
                                //output += "Here!" + formatted_instruction + "\n";
                                List<string> instruction_list = new List<string>();
                                ////instruction_list = formatted_instruction.Split(':').ToList();
                                instruction_list = formatted_instruction.Split(',').ToList();
                                //output += "Here" + instruction_list[5]; // at one returns an error because nothing exists in 1 so would have to filter the NONE values in the list.


                                // loop concatenates the output so there isn't any spaces. 
                                List<string> actual_instruction_list = new List<string>();
                                var query = from word in instruction_list where word != "" select word;
                                foreach (var word in query)
                                {
                                    actual_instruction_list.Add(word);
                                    //Console.WriteLine(word); // need to figure out how to get values back into a list. So can finish the R instructions. 
                                }

                                // first before Add instructions?
                                if (line.Contains("ADDI"))
                                {
                                    actual_instruction_list[1] = actual_instruction_list[1].Replace("X", string.Empty);
                                    actual_instruction_list[2] = actual_instruction_list[2].Replace("X", string.Empty);
                                    actual_instruction_list[3] = actual_instruction_list[3].Replace("#", string.Empty);

                                    int fullBits;
                                    int addiIntermediate;
                                    // if negative
                                    if (actual_instruction_list[3].Contains("-"))
                                    {
                                        Console.WriteLine("Negative");
                                        actual_instruction_list[3] = actual_instruction_list[3].Replace("-", string.Empty);
                                        int.TryParse(actual_instruction_list[3], out addiIntermediate);

                                        fullBits = 0xFF;
                                        Console.WriteLine(fullBits);
                                        fullBits -= addiIntermediate;
                                        Console.WriteLine(fullBits);

                                    }
                                    // if positive
                                    else
                                    {
                                        Console.WriteLine("Postive");
                                        int.TryParse(actual_instruction_list[3], out addiIntermediate);

                                        fullBits = 0;
                                        fullBits += addiIntermediate;
                                    }

                                    int.TryParse(actual_instruction_list[1], out int rn);
                                    int.TryParse(actual_instruction_list[2], out int rd);


                                    int.TryParse(Convert.ToString(rn, 2), out rn);
                                    int.TryParse(Convert.ToString(rd, 2), out rd);
                                    int.TryParse(Convert.ToString(addiIntermediate, 2), out addiIntermediate);
                                    int.TryParse(Convert.ToString(fullBits, 2), out fullBits);
                                    Console.WriteLine(fullBits);
                                    string machine_code;
                                    if (line.Contains("-"))
                                    {
                                        machine_code = OPCODE[actual_instruction_list[0]] + "1" + fullBits.ToString().PadLeft(11, '1') + rn.ToString("D4") + rd.ToString("D5");
                                        Console.WriteLine("Negative");
                                    }
                                    else
                                    {
                                        machine_code = OPCODE[actual_instruction_list[0]] + "0" + fullBits.ToString("D11") + rn.ToString("D5") + rd.ToString("D5");
                                        Console.WriteLine("Positive");
                                    }
                                    Console.WriteLine(machine_code);
                                    string hexString = String.Format("{0:X2}", Convert.ToUInt64(machine_code, 2));  // The hex representation of the string for output

                                    Console.WriteLine(hexString);
                                    output += NumberAddress2.ToString("X8") + "    " + hexString;
                                    NumberAddress2 += 4;

                                    actual_instruction_list.Clear();

                                }
                                // Checking for D instruction
                                else if (line.Contains("LDUR") || line.Contains("STUR"))
                                {
                                    if (actual_instruction_list[1].Contains('X') && actual_instruction_list[2].Contains("X"))
                                    {   // check for correctness

                                        actual_instruction_list[1] = actual_instruction_list[1].Replace("X", string.Empty);
                                        actual_instruction_list[2] = actual_instruction_list[2].Replace("X", string.Empty);
                                        actual_instruction_list[3] = actual_instruction_list[3].Replace("#", string.Empty);

                                        int.TryParse(actual_instruction_list[1], out int rd);
                                        int.TryParse(actual_instruction_list[2], out int rn);
                                        int.TryParse(actual_instruction_list[3], out int address);

                                        int.TryParse(Convert.ToString(rn, 2), out rn);
                                        int.TryParse(Convert.ToString(rd, 2), out rd);
                                        int.TryParse(Convert.ToString(address, 2), out address);
                                        string shamt = "00";

                                        string machine_code = OPCODE[actual_instruction_list[0]] + address.ToString("D9") + shamt + rn.ToString("D5") + rd.ToString("D5");

                                        string hexString = String.Format("{0:X2}", Convert.ToUInt64(machine_code, 2));  // The hex representation of the string for output

                                        Console.WriteLine(hexString);

                                        output += NumberAddress2.ToString("X8") + "    " + hexString;
                                        NumberAddress2 += 4;

                                        foreach (string element in actual_instruction_list)
                                        {
                                            Console.WriteLine("Element: " + element);   //for checking if correct while not outputting to file. 
                                        }


                                        actual_instruction_list.Clear();

                                    }
                                }


                                // Checks to see if an R instruction.
                                else if (line.Contains("ADD") || line.Contains("SUB") || line.Contains("ORR") || line.Contains("AND"))
                                {
                                    if (actual_instruction_list[3].Contains('X') && actual_instruction_list[2].Contains("X") && actual_instruction_list[1].Contains("X"))   // check for correctness
                                    {

                                        actual_instruction_list[3] = actual_instruction_list[3].Replace("X", string.Empty);       // Gettin the numbers out of complex values
                                        actual_instruction_list[2] = actual_instruction_list[2].Replace("X", string.Empty);
                                        actual_instruction_list[1] = actual_instruction_list[1].Replace("X", string.Empty);
                                        //int rm;

                                        int.TryParse(actual_instruction_list[3], out int rm);
                                        int.TryParse(actual_instruction_list[2], out int rn);
                                        int.TryParse(actual_instruction_list[1], out int rd);

                                        int.TryParse(Convert.ToString(rm, 2), out int rmS);          // String representation of the binary representation of the value of the registers, into an int value.
                                        int.TryParse(Convert.ToString(rn, 2), out int rnS);
                                        int.TryParse(Convert.ToString(rd, 2), out int rdS);
                                        string shamt = "000000";        // unique to R instructions


                                        // Check with each line individually and try to figure out, how to decrement the list of all the values. 
                                        // Returns the entire binary string computed from the opcode and the 
                                        string machine_code = OPCODE[actual_instruction_list[0]] + rmS.ToString("D5") + shamt + rnS.ToString("D5") + rdS.ToString("D5");  // The printing of the string representatino of Binary
                                        Console.WriteLine(machine_code);    // checks if accureate

                                        string hexString = String.Format("{0:X2}", Convert.ToUInt64(machine_code, 2));  // The hex representation of the string for output

                                        Console.WriteLine(hexString);

                                        // iterate over each line, decrement the list each time,

                                        // output to file
                                        output += NumberAddress2.ToString("X8") + "    " + hexString; // the output with the address and the value stored there.
                                        NumberAddress2 += 4;

                                        /*foreach (string element in actual_instruction_list)
                                        {
                                            Console.WriteLine("Element: " + element);   for checking if correct while not outputting to file. 
                                        }*/

                                        actual_instruction_list.Clear();    // Clears the instruction list in case it decides to hold values for later.
                                    }
                                }

                                // Solves MOVZ
                                else if (line.Contains("MOVZ"))
                                {
                                    if (line.Contains("LSL 0") || line.Contains("LSL 16") || line.Contains("LSL 48"))
                                    {
                                        actual_instruction_list[1] = actual_instruction_list[1].Replace("X", string.Empty);
                                        actual_instruction_list[2] = actual_instruction_list[2].Replace("#", string.Empty);

                                        int.TryParse(actual_instruction_list[1], out int rd);
                                        int.TryParse(actual_instruction_list[2], out int MovImediate);

                                        int.TryParse(Convert.ToString(rd, 2), out rd);
                                        int.TryParse(Convert.ToString(MovImediate, 2), out MovImediate);

                                        // padd with 16bits
                                        //rd ("D5?")                      
                                        string machine_code = OPCODE[actual_instruction_list[0]] + OPCODE[actual_instruction_list[4]] + MovImediate.ToString("D16") + rd.ToString("D5");
                                        string hexString = String.Format("{0:X2}", Convert.ToUInt64(machine_code, 2));

                                        output += NumberAddress2.ToString("X8") + "    " + hexString;
                                        NumberAddress2 += 4;


                                        foreach (string element in actual_instruction_list)
                                        {
                                            Console.WriteLine("Element: " + element);   //for checking if correct while not outputting to file. 
                                        }
                                        actual_instruction_list.Clear();
                                    }
                                }

                                else if (line.Contains("CBZ")){
                            // if there is a register take it on the back
                                     int end = 0;
                                        if (actual_instruction_list[1].Contains("X"))
                                        {
                                            actual_instruction_list[1] = actual_instruction_list[1].Replace("X", string.Empty);
                                            int.TryParse(actual_instruction_list[1], out end);
                                            int.TryParse(Convert.ToString(end, 2), out end);
                                        }


                                    //int.TryParse(Convert.ToString(rd, 2), out rd);
                                    //int.TryParse(Convert.ToString(MovImediate, 2), out MovImediate);

                                    // padd with 16bits
                                    //rd ("D5?")                      
                                    string machine_code = OPCODE[actual_instruction_list[0]] + "0000" + end.ToString("D20");
                                    string hexString = String.Format("{0:X2}", Convert.ToUInt64(machine_code, 2));

                                     output += NumberAddress2.ToString("X8") + "    " + hexString;
                                    NumberAddress2 += 4;


                                    foreach (string element in actual_instruction_list)
                                    {
                                        Console.WriteLine("Element: " + element);   //for checking if correct while not outputting to file. 
                                    }
                                    actual_instruction_list.Clear();

                                }


                                // B type Instruction
                                else if (line.Contains("B")){
                                    int end = 0;

                                     // if there is a register take it on the back
                                    if (actual_instruction_list.Count() == 1)
                                    {
                                        if (actual_instruction_list[1].Contains("X"))
                                        {
                                            actual_instruction_list[1] = actual_instruction_list[1].Replace("X", string.Empty);
                                            int.TryParse(actual_instruction_list[1], out end);
                                            int.TryParse(Convert.ToString(end, 2), out end);
                                        }
                                    }


                                    //int.TryParse(Convert.ToString(rd, 2), out rd);
                                    //int.TryParse(Convert.ToString(MovImediate, 2), out MovImediate);

                                    // padd with 16bits
                                    //rd ("D5?")                      
                                    string machine_code = OPCODE[actual_instruction_list[0]] + "00000" + end.ToString("D21");
                                    string hexString = String.Format("{0:X2}", Convert.ToUInt64(machine_code, 2));

                                     output += "\t" + NumberAddress2.ToString("X8") + "    " + hexString;
                                    NumberAddress2 += 4;


                                    foreach (string element in actual_instruction_list)
                                    {
                                        Console.WriteLine("Element: " + element);   //for checking if correct while not outputting to file. 
                                    }
                                    actual_instruction_list.Clear();

                                }



                        

                                /*foreach (int element in SymbolTableAdress)
                                {
                                    output += SymbolTableLabel[element] + SymbolTableAdress[element];
                                    Console.WriteLine(SymbolTableLabel[element] + SymbolTableAdress[element]);
                                }*/


                              
                         }
                    sr.Dispose();
                }
                output += "\n\nSymbol Table:\nLabel    Address (in hex)\n";
                for (int j = 0; j != SymbolTableLabel.Count(); j++)
                { 
                    output += SymbolTableLabel[j] + "\t" +SymbolTableAdress[j].ToString("X8")+ "\n"; // print symbol Table at the end
                }
            }
            // Writing the results of the operations
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = sfd.FileName;
                BinaryWriter bw = new BinaryWriter(File.Create(path)); // defaulting to the directory of the openfilddialog
                bw.Write(output); // ***
                bw.Dispose();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
