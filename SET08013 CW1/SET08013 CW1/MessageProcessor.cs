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
        
        public void ProcessMessage(string message)
        {
            _inputMessage = message.ToLower();

            writeMessageToFile(isValidMessage());
        }

        private bool isValidMessage()
        {
            StreamReader reader = new StreamReader(File.OpenRead(@_badWordFilePath));

            while(!reader.EndOfStream)
            {
                string   line  = reader.ReadLine();
                string[] words = line.Split(',');
                string   regex = "\\b" + words[0].ToLower() + "\\b";  //Match entire word only.

                if (Regex.IsMatch(_inputMessage, regex))
                {
                    return false;
                }
            }

            return true;
        }

        private void writeMessageToFile(bool valid)
        {
            cleanMessage();

            if(valid)
            {
                File.AppendAllText(@_validFilePath, _inputMessage + ",");
            }
            else
            {
                File.AppendAllText(@_quarantineFilePath, _inputMessage + ",");
            }
        }

        private void cleanMessage()
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
    }
}
