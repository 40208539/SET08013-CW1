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
            string[] subjects;
            string[] universities;
            Level    level         = Level.NONE;
            string   ugRegex       = @"(\bug\b)|(\bu/g\b)|(\bunder graduate\b)";
            string   pgRegex       = @"(\bpg\b)|(\bp/g\b)|(\bpost graduate\b)";

            if (Regex.IsMatch(message.ToLower(), ugRegex))
            {
                level = Level.UG;
            }
            else if (Regex.IsMatch(message.ToLower(), pgRegex))
            {
                level = Level.PG;
            }

            StreamReader reader = new StreamReader(File.OpenRead(@_universityFilePath));
            while (!reader.EndOfStream)
            {

            }
        }

        public int Levenshtein(string a, string b)
        {
            int n = a.Length;
            int m = b.Length;
            int cost = 0;
            int min1;
            int min2;
            int min3;
            int[,] d = new int[n+1,m+1];
            if(n == 0)
            {
                return m;
            }
            if(m == 0)
            {
                return n;
            }
            for(int i = 0; i < n; i++)
            {
                d[i,0] = i;
            }
            for (int i = 0; i < n; i++)
            {
                d[0,i] = i;
            }
            for (int i = 1; i < n; i++)
            {
                for (int j = 1; j < m; j++)
                {
                    cost = (a[i] == b[j]) ? 0 : 1;
                    min1 = d[i - 1, j] + 1;
                    min2 = d[i,j - 1] + 1;
                    min3 = d[i - 1,j - 1] + cost;
                    d[i,j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }
            Console.WriteLine("done alg");
            return d[n,m];
        }
    }
}
