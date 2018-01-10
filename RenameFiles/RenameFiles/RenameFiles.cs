using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * By Krystian Horoszkiewicz
 */

namespace RenameFiles
{
    class RenameFiles
    {
        List<string> words = new List<string>();                                        // Words to be removed
        List<char> characters = new List<char>();                                       // Characters to remove
        bool initChars = false;
        //List<char> specialChars = new List<char>();                                   // Build in list of special characters including '.'
        Dictionary<string, string> replaceWords = new Dictionary<string, string>();     // Key: word to replace Value: replace with
        public bool UseRenameSequence { get; set; }
        public string NewFileName { get; set; }
        public string NewFileNameEnd { get; set; }
        public int StartSequence { get; set; }
        public int SequenceWidth { get; set; }

        private string workingDir = "";
        private string newDir = "";
        public bool FirstLetterCapital { get; set; }
        private bool returnToNewDir = false;
        private bool completed = true;

        // Public access modifiers
        /// <summary>
        /// Source directory containing files to be renamed.
        /// </summary>
        public string WorkingDir
        {
            set
            {
                if (!String.IsNullOrEmpty(value) && Directory.Exists(value))
                {
                    workingDir = value;
                }
                else
                {
                    Console.WriteLine("Invalid dir specified, use absolute path e.g: C:\\Desktop\\MyFiles");
                }
            }
            get
            {
                return workingDir;
            }
        }
        /// <summary>
        /// Destination directory to which files will be moved after renaming.
        /// </summary>
        public string NewDir
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    newDir = value;
                    returnToNewDir = true;
                }
                else
                {
                    Console.WriteLine("Word is empty or null");
                }
            }
            get
            {
                return newDir;
            }
        }

        /// <summary>
        /// Makes sure list of characters contains only unique characters
        /// </summary>
        /// <param name="newChar"></param>
        private void CheckCharDuplicates(char newChar)
        {
            if(!characters.Contains(newChar))
            {
                characters.Add(newChar);
            }
            else
            {
                Console.WriteLine("Character already in the list");
            }
        }

        /// <summary>
        /// Specify if default characters should be enabled.
        /// </summary>
        public bool InitSpecialChars
        {
            set
            {
                if (value == true)
                {
                    CheckCharDuplicates('~');
                    CheckCharDuplicates('`');
                    CheckCharDuplicates('@');
                    CheckCharDuplicates('#');
                    CheckCharDuplicates('$');
                    CheckCharDuplicates('%');
                    CheckCharDuplicates('^');
                    CheckCharDuplicates('&');
                    CheckCharDuplicates('*');
                    CheckCharDuplicates('(');
                    CheckCharDuplicates(')');
                    CheckCharDuplicates('"');
                    CheckCharDuplicates(':');
                    CheckCharDuplicates(';');
                    CheckCharDuplicates(',');
                    CheckCharDuplicates('.');
                    CheckCharDuplicates('<');
                    CheckCharDuplicates('>');
                    CheckCharDuplicates('/');
                    CheckCharDuplicates('?');
                    CheckCharDuplicates('-');
                    CheckCharDuplicates('_');
                    CheckCharDuplicates('+');
                    CheckCharDuplicates('—');
                    initChars = true;
                    Console.WriteLine("Special characters added.");
                }
                else
                {
                    characters.Remove('~');
                    characters.Remove('`');
                    characters.Remove('@');
                    characters.Remove('#');
                    characters.Remove('$');
                    characters.Remove('%');
                    characters.Remove('^');
                    characters.Remove('&');
                    characters.Remove('*');
                    characters.Remove('(');
                    characters.Remove(')');
                    characters.Remove('"');
                    characters.Remove(':');
                    characters.Remove(';');
                    characters.Remove(',');
                    characters.Remove('.');
                    characters.Remove('<');
                    characters.Remove('>');
                    characters.Remove('/');
                    characters.Remove('?');
                    characters.Remove('-');
                    characters.Remove('_');
                    characters.Remove('+');
                    characters.Remove('—');
                    initChars = false;
                    Console.WriteLine("Special characters removed.");
                }
            }
            get
            {
                return initChars;
            }
        }
        /// <summary>
        /// Returns a list of characters to remove
        /// </summary>
        /// <returns></returns>
        public List<char> GetCharList()
        {
            return characters;
        }
        /// <summary>
        /// Returns a list of words to remove
        /// </summary>
        /// <returns></returns>
        public List<string> GetWordsList()
        {
            return words;
        }

        /// <summary>
        /// Returns a dictionary of words to replace, key: word which will be replaced, value: new word for the key
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetWordsToReplaceList()
        {
            return replaceWords;
        }

        // Class initializer
        /// <summary>
        /// Enter source directory containing files to be renamed if WorkingDir not set already.
        /// </summary>
        /// <param name="rootDir"></param>
        public RenameFiles(string rootDir)
        {
            workingDir = rootDir;
        }

        /// <summary>
        /// 
        /// </summary>
        public RenameFiles()
        {
            // TODO: Complete member initialization
        }
        /// <summary>
        /// Recursive scan for all files in subfolders.
        /// </summary>
        /// <param name="sDir"></param>
        private void FindAll(string sDir)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(sDir);
                FileInfo[] infos = dir.GetFiles();
                Console.WriteLine("Renaming files in " + sDir);

                foreach (FileInfo f in infos)
                {
                    if (returnToNewDir)
                    {
                        // File.Move is used to rename files or move files.
                        if (FirstLetterCapital)
                        {
                            File.Move(f.FullName, newDir + "\\" + Rename(FirstCapital(f.Name)));
                        }
                        else
                        {
                            File.Move(f.FullName, newDir + "\\" + Rename(f.Name));
                        }
                    }
                    else
                    {
                        if (FirstLetterCapital)
                        {
                            File.Move(f.FullName, sDir + "\\" + Rename(FirstCapital(f.Name)));
                        }
                        else
                        {
                            File.Move(f.FullName, sDir + "\\" + Rename(f.Name));
                        }
                    }
                }

                /*foreach (string f in Directory.GetFiles(sDir))
                {
                    Console.WriteLine(f);
                }*/
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    FindAll(d);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                completed = false;
            }
        }

        /// <summary>
        /// Takes working directory and renames all files in this directory to specified sequence determined by NewFileName, NewFileNameEnd, StartSequence
        /// </summary>
        /// <param name="sDir"></param>
        private void RenameSequence(string sDir)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(sDir);
                FileInfo[] infos = dir.GetFiles();
                
                foreach(FileInfo f in infos)
                {
                    string tempName = f.Name;
                    int indexLast = tempName.LastIndexOf('.');
                    string extension = tempName.Substring(indexLast);       // Sets file extension to whatever is after '.' eg: .png .txt

                    // 0000, 1 w> 1, 0000, 13 w> 2 
                    string seqWidth = "";
                    int currentSeqWidth = StartSequence.ToString().Length;        // 4 = 1, 12 = 2, 465 = 3

                    for (int i = 0; i < SequenceWidth - currentSeqWidth; i++ )
                    {
                        seqWidth += '0';
                    }

                    File.Move(f.FullName, sDir + "\\" + NewFileName + seqWidth + StartSequence + NewFileNameEnd + extension);
                    //Console.WriteLine("File: " + f.Name + " renamed to: " + NewFileName + seqWidth + StartSequence + NewFileNameEnd + extension);
                    StartSequence++;
                }
            }
            catch(System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Runs the program.
        /// </summary>
        public void RunRename()
        {
            if(UseRenameSequence)
            {
                RenameSequence(workingDir);
            }
            else
            {
                FindAll(workingDir);
            }
            
            if (completed)
            {
                Console.WriteLine("Renaming completed");

                if (returnToNewDir)
                {
                    Console.WriteLine("Files moved to {0}", newDir);
                }
            }

            /*DirectoryInfo d = new DirectoryInfo(workingDir);
            FileInfo[] infos = d.GetFiles();

            foreach (FileInfo f in infos)
            {
                Console.WriteLine("Renaming: " + f.Name);
                if(FirstLetterCapital)
                {
                    File.Move(f.FullName, workingDir + "\\" + Rename(FirstCapital(f.Name)));
                }
                else
                {
                    File.Move(f.FullName, workingDir + "\\" + Rename(f.Name));
                }
                Console.WriteLine("Renamed: " + f.Name + "\n");
            }*/
        }
        /// <summary>
        /// Replace words loop has the priority over remove words and remove characters
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string Rename(string name)
        {
            // Replace words
            name = ReplaceWords(name);

            // Removes words
            name = RemoveWords(name);

            // Removes characters
            name = RemoveCharacters(name);

            return name;
        }

        private string ReplaceWords(string name)
        {
            try
            {
                foreach (KeyValuePair<string, string> entry in replaceWords)
                {
                    if (name.Contains(entry.Key))
                    {
                        if (entry.Key.Equals("."))
                        {
                            int indexLast = name.LastIndexOf('.');
                            name = name.Replace(entry.Key, entry.Value);

                            StringBuilder sb = new StringBuilder(name);
                            sb[indexLast] = '.';
                            name = sb.ToString();
                        }
                        else
                        {
                            name = name.Replace(entry.Key, entry.Value);
                        }
                    }
                }
                return name;
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ReplaceWords failure " + ane.Message);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine("ReplaceWords failure " + ae.Message);
            }
            return "";
        }

        private string RemoveWords(string name)
        {
            try
            {
                foreach (string word in words)
                {
                    if (name.Contains(word))
                    {
                        int wordLength = word.Length;
                        int index = name.IndexOf(word);
                        name = name.Remove(index, wordLength);
                    }
                }
                return name;
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("RemoveWords failure " + ane.Message);
            }
            catch (ArgumentOutOfRangeException aor)
            {
                Console.WriteLine("RemoveWords failure " + aor.Message);
            }
            return "";
        }

        private string RemoveCharacters(string name)
        {
            try
            {
                foreach (char character in characters)
                {
                    if (name.Contains(character))
                    {
                        int charNum = name.Count(c => c == character);          // Counts the number of occurrences of the specific character in a word

                        while(charNum > 0)
                        {
                            if(name.Contains(character) && character != '.')    // '.' is handled in a specific way as its used to define file extensions and all '.' cannot be removed
                            {
                                int tempIndex = name.IndexOf(character);
                                name = name.Remove(tempIndex, 1);               // Removes one character at the time, tempIndex defines the closest char from the start
                            }
                            else
                            {
                                if (charNum > 1)                                // Leaves the last '.' that defines file format
                                {
                                    int tempIndex = name.IndexOf(character);
                                    name = name.Remove(tempIndex, 1);
                                }
                                if(charNum == 1)
                                {
                                    charNum = 0;                                
                                }
                            }
                            charNum--;
                        }
                    }
                }
                return name;
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("RemoveCharacters failure " + ane.Message);
            }
            catch (ArgumentOutOfRangeException aor)
            {
                Console.WriteLine("RemoveCharacters failure " + aor.Message);
            }
            catch (OverflowException oe)
            {
                Console.WriteLine("RemoveCharacters failure " + oe.Message);
            }
            return "";
        }

        /// <summary>
        /// Capitalizes the first letter in the string.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string FirstCapital(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                StringBuilder sb = new StringBuilder(name);
                string temp = name;
                temp = temp.ToUpper();
                sb[0] = temp[0];
                name = sb.ToString();

                return name;
            }
            else
            {
                Console.WriteLine("Word is empty or null");
            }
            return name;
        }
        /// <summary>
        /// Add word to replace, "word" is the word you want to replace with "wordToReplace".
        /// </summary>
        /// <param name="word"></param>
        /// <param name="wordToReplace"></param>
        public void AddReplaceWords(string word, string wordToReplace)
        {
            if (!String.IsNullOrEmpty(word))
            {
                replaceWords.Add(word, wordToReplace);
            }
            else
            {
                Console.WriteLine("Word is empty or null");
            }
        }
        /// <summary>
        /// Add word to remove. Word can also have one letter.
        /// </summary>
        /// <param name="word"></param>
        public void AddWordsToRemove(string word)
        {
            if (!String.IsNullOrEmpty(word))
            {
                words.Add(word);
            }
            else
            {
                Console.WriteLine("Word is empty or null");
            }
        }
        /// <summary>
        /// Add character to remove.
        /// </summary>
        /// <param name="character"></param>
        public void AddCharactersToRemove(char character)
        {
            if (character != ' ')
            {
                CheckCharDuplicates(character);
            }
            else
            {
                Console.WriteLine("Character is empty or null");
            }
        }
    }
}
