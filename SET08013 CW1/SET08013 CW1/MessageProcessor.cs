using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SET08013_CW1
{
    class MessageProcessor
    {
        private const string _badWordFilePath    = "../textwords.csv";
        private const string _universityFilePath = "../University List.csv";
        private       string _message;
        
        public void ProcessMessage(string message)
        {
            _message = message.ToLower();
            if(!ContainsBadWords())
            {
                //TODO:- Write good message to valid file
                Console.WriteLine("NO BAD WORDS");
            }
            else
            {
                //TODO:- Write bad message to quarantine file
                Console.WriteLine("BAD WORD DETECTED");
            }
        }

        private bool ContainsBadWords()
        {
            StreamReader reader = new StreamReader(File.OpenRead(@_badWordFilePath));

            while(!reader.EndOfStream)
            {
                string   line  = reader.ReadLine();
                string[] words = line.Split(',');
                string regex = "\\b" + words[0].ToLower() + "\\b";  //Match entire word only.

                if (Regex.IsMatch(_message, regex))
                {
                        return true;
                }
            }
            return false;
        }
    }
}
