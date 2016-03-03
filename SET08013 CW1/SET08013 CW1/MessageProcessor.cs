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
        private List<string> _validMessages      = new List<string>();
        
        public void InputMessage(string message)
        {
            _inputMessage = message;
            WriteMessageToFile(IsValidMessage());
        }

        public void ProcessValidMessages()
        {
            ReadValidMessages();
            foreach (string message in _validMessages)
            {
                SearchMessage(message);
            }
        }

        private bool IsValidMessage()
        {
            StreamReader reader = new StreamReader(File.OpenRead(@_badWordFilePath));

            while(!reader.EndOfStream)
            {
                string   line  = reader.ReadLine();
                string[] words = line.Split(',');
                string   regex = "\b" + words[0].ToLower() + "\b";  //Match entire word only.

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
            _inputMessage = result.ToString();
        }

        private void ReadValidMessages()
        {
            StreamReader reader = new StreamReader(File.OpenRead(@_validFilePath));

            while (!reader.EndOfStream)
            {
                string   line = reader.ReadLine();
                string[] text = line.Split(',');
                foreach(string s in text)
                {
                    _validMessages.Add(s);
                }
            }
        }

        private void SearchMessage(string message)
        {
            //Determine whether the message is UG or PG
           
            string ugRegex = "(\bug\b|\bu/g\b|\bunder graduate\b)";
            string pgRegex = "(\bpg\b|\bp/g\b|\bpost graduate\b)";

            if(Regex.IsMatch(message, ugRegex))
            {

            }
            else if(Regex.IsMatch(message, pgRegex))
            {

            }
        }
    }
}
