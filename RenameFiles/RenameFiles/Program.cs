using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenameFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            RenameFiles renFiles = new RenameFiles();

            /* MENU */
            string input = "0";
           // renFiles.WorkingDir = @"C:\Users\Krystian\Desktop\c4d tempRender";
            while(!input.Equals("q"))
            {
                if(String.IsNullOrEmpty(renFiles.WorkingDir))
                {
                    Console.WriteLine("Specify working dir in which files will be modified:");
                    renFiles.WorkingDir = Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    if(renFiles.UseRenameSequence)
                    {
                        Console.WriteLine("Current working dir:\n{0}\n", renFiles.WorkingDir);
                        Console.WriteLine("Sequence rename menu\nDefault seq naming <Fron_Word><Numeric_Sequence><End_Word>\nWhere Front & End words are not necessary");
                        Console.WriteLine("1. Disable sequence");
                        Console.WriteLine("2. Sequence start number\t\t[{0}]", renFiles.StartSequence);
                        Console.WriteLine("3. Front word (before seq number)\t[{0}]", renFiles.NewFileName);
                        Console.WriteLine("4. End word (after seq number)\t\t[{0}]", renFiles.NewFileNameEnd);
                        Console.WriteLine("5. Sequence width e.g. if width = 4 and seq start = 2: 0002\t[{0}]", renFiles.SequenceWidth);
                        Console.WriteLine("r. Run the program");

                        switch(input = Console.ReadLine())
                        {
                            case "1":
                                renFiles.UseRenameSequence = false;
                                Console.WriteLine("Sequence enabled: " + renFiles.UseRenameSequence);
                                continue;
                            case "2":
                                Console.WriteLine("Enter a number which will start the sequence");
                                renFiles.StartSequence = 2;
                                int temp = 0;
                                Int32.TryParse(Console.ReadLine(), out temp);
                                renFiles.StartSequence = temp;
                                continue;
                            case "3":
                                Console.WriteLine("Enter front word before seq number");
                                renFiles.NewFileName = Console.ReadLine();
                                continue;
                            case "4":
                                Console.WriteLine("Enter end word before seq number");
                                renFiles.NewFileNameEnd = Console.ReadLine();
                                continue;
                            case "5":
                                Console.WriteLine("Enter sequence width");
                                int tempWidth = 0;
                                Int32.TryParse(Console.ReadLine(), out tempWidth);
                                renFiles.SequenceWidth = tempWidth;
                                continue;
                            case "r":
                                renFiles.RunRename();
                                Console.WriteLine("Press 'q' to quit or any button to continue");
                                input = Console.ReadLine();
                                continue;
                            default:
                                Console.WriteLine("Please try again or press 'q' to exit");
                                continue;
                        }
                    }
                    else
                    {
                        // MENU
                        Console.WriteLine("Current working dir:\n{0}\n", renFiles.WorkingDir);
                        Console.WriteLine("Select a number to access the menu:");
                        Console.WriteLine("1. Change working dir: " + renFiles.WorkingDir);
                        Console.WriteLine("2. Change destination dir: " + renFiles.NewDir);
                        Console.WriteLine("3. Use sequence rename (renames all files in dir to specified name with seq num)");
                        Console.WriteLine("4. Add characters to remove");
                        Console.Write("\tChars: ");
                        foreach (char item in renFiles.GetCharList())
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine("\n5. Add words to remove");
                        Console.Write("\tWords: ");
                        foreach (string item in renFiles.GetWordsList())
                        {
                            Console.Write(item + ", ");
                        }
                        Console.WriteLine("\n6. Add words to replace");
                        Console.Write("\tWords: ");
                        foreach (KeyValuePair<string, string> entry in renFiles.GetWordsToReplaceList())
                        {
                            Console.Write("[" + entry.Key + ", " + entry.Value + "], ");
                        }
                        Console.Write("\n7. Initialize default characters to remove: ~`@#$%^&*()\":;,.<>/?_-+ ");

                        if(renFiles.InitSpecialChars)
                        {
                            Console.Write("[Enabled]\n");
                        }
                        else
                        {
                            Console.Write("[Disabled]\n");
                        }

                        Console.Write("\n8. Select to change from Before to After sequence example seq: (2017)");

                        if (renFiles.RemoveAfterSeqBool)
                        {
                            Console.Write("\nRemoving after sequence e.g: \nTune-1-(2017)_Code#%45d6sa6.mp3 to:\nTune-1-(2017).mp3\n");
                        }
                        else
                        {
                            Console.Write("\nRemoving before sequence e.g: \nTune-1-(2017)_Code#%45d6sa6.mp3 to:\nTune-1-.mp3\n");
                        }
                        Console.WriteLine("\n9 Remove characters after sequence e.g: Seq: (2017) will result in: \nTune-1-(2017)_Code#%45d6sa6.mp3\nTune-1-(2017).mp3");
                        Console.WriteLine("r. Run the program");
                    }

                    switch(input = Console.ReadLine())
                    {
                        case "1":
                            Console.WriteLine("Enter new working dir:");
                            renFiles.WorkingDir = Console.ReadLine();
                            continue;
                        case "2":
                            Console.WriteLine("Enter new destination dir:");
                            renFiles.NewDir = Console.ReadLine();
                            continue;
                        case "3":
                            renFiles.UseRenameSequence = true;
                            Console.WriteLine("Sequence enabled: " + renFiles.UseRenameSequence);                           
                            continue;
                        case "4":
                            Console.WriteLine("Add character to remove");
                            char charToRemove;
                            if(!Char.TryParse(Console.ReadLine(), out charToRemove))
                            {
                                Console.WriteLine("Error. Please enter one character at the time. Press any key to continue.");
                                Console.ReadKey();
                            }
                            else
                            {
                                renFiles.AddCharactersToRemove(charToRemove);
                            }                           
                            continue;
                        case "5":
                            Console.WriteLine("Add word to remove");
                            renFiles.AddWordsToRemove(Console.ReadLine());
                            continue;
                        case "6":
                            Console.WriteLine("Add word to replace");
                            string wordToReplace;
                            string wordToReplaceWith;
                            wordToReplace = Console.ReadLine();
                            Console.WriteLine("[" + wordToReplace + "] replace with: ");
                            wordToReplaceWith = Console.ReadLine();
                            Console.WriteLine("[" + wordToReplace + "] will be replaced with: " + wordToReplaceWith);
                            renFiles.AddReplaceWords(wordToReplace, wordToReplaceWith);
                            continue;
                        case "7":
                            Console.WriteLine("Initialize default characters to remove: ~`@#$%^&*()\":;,.<>/?_-+ ");
                            if(renFiles.InitSpecialChars)
                            {
                                renFiles.InitSpecialChars = false;
                            }
                            else
                            {
                                renFiles.InitSpecialChars = true;
                            }                            
                            continue;
                        case "8":
                            if (renFiles.RemoveAfterSeqBool)
                            {
                                renFiles.RemoveAfterSeqBool = false;
                            }
                            else
                            {
                                renFiles.RemoveAfterSeqBool = true;
                            }             
                            continue;
                        case "9":
                            Console.WriteLine("Add sequence:");
                            renFiles.RemoveAfterSeq = Console.ReadLine();
                            continue;
                        case "r":
                            renFiles.RunRename();
                            Console.WriteLine("Press 'q' to quit or any button to continue");
                            input = Console.ReadLine();
                            continue;
                        default:
                            Console.WriteLine("Please try again or press 'q' to exit");
                            continue;
                    }
                }
            }
        }
    }
}
