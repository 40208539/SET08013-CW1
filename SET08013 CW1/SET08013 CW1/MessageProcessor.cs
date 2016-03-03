using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                if (_message.Contains(" "+words[0].ToLower()+" "))        //If the input message contains the word from the first column of the CSV
                {
                        return true;
                }
            }
            return false;
        }
    }
}
