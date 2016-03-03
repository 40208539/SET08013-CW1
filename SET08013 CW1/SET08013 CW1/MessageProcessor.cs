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
        private const string _validFilePath      = "../Valid Messages.txt";
        private const string _quarantineFilePath = "../Quarantine Messages.txt";
        private       string _inputMessage;
        private     string[] _validMessages;
        
        public void ProcessMessage(string message)
        {
            _inputMessage = message;
            WriteMessageToFile(IsValidMessage());
            ReadMessages();
        }

        private bool IsValidMessage()
        {
            StreamReader reader = new StreamReader(File.OpenRead(@_badWordFilePath));

            while(!reader.EndOfStream)
            {
                string   line  = reader.ReadLine();
                string[] words = line.Split(',');
                string   regex = "\\b" + words[0].ToLower() + "\\b";  //Match entire word only.

                if (Regex.IsMatch(_inputMessage.ToLower(), regex))
                {
                    return false;
                }
            }

            return true;
        }

        private void WriteMessageToFile(bool valid)
        {
            CleanMessage();

            if(valid)
            {
                File.AppendAllText(@_validFilePath, _inputMessage + ",");
            }
            else
            {
                File.AppendAllText(@_quarantineFilePath, _inputMessage + ",");
            }
        }

        private void CleanMessage()
        {
            StringBuilder result = new StringBuilder(_inputMessage.Length);

            foreach (char c in _inputMessage)
            {
                if (c != ',')
                    result.Append(c);
            }
            Console.Write(result);
            _inputMessage = result.ToString();
        }

        private void ReadMessages()
        {
            StreamReader reader = new StreamReader(File.OpenRead(@_badWordFilePath));

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                _validMessages = line.Split(',');
                Console.WriteLine(_validMessages[0]);
            }
        }
    }
}
