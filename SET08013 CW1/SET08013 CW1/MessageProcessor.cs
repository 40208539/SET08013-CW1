using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace SET08013_CW1
{
    class MessageProcessor
    {
        private const string _badWordFilePath    = "../textwords.csv";
        private const string _universityFilePath = "../University List.csv";
        private const string _subjectsFilePath   = "../subjects.csv";
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
            List<string> subjects = new List<string>();
            List<string> universities = new List<string>();
            List<string> wordsToRemove = "University of".Split(' ').ToList();

            Level level = Level.NONE;
            string ugRegex = @"(\bug\b)|(\bu/g\b)|(\bunder graduate\b)";
            string pgRegex = @"(\bpg\b)|(\bp/g\b)|(\bpost graduate\b)";

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
                string line = reader.ReadLine();
                string university = StringWordsRemove(line, wordsToRemove);
                foreach (string word in message.Split(' '))
                {
                    int distance = Levenshtein(word.ToLower(), university.ToLower());

                    if (distance < 3)
                    {
                        universities.Add(university);
                    }
                }
            }

            reader = new StreamReader(File.OpenRead(@_subjectsFilePath));
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                foreach (string word in message.Split(' '))
                {
                    int distance = Levenshtein(word.ToLower(), line.ToLower());

                    if (distance < 3)
                    {
                        if (!subjects.Contains(line))
                        {
                            subjects.Add(line);
                        }
                    }
                }
            }

            Message mess = new Message(level, message, subjects, universities);
            string json = JsonHelper.JsonSerializer<Message>(mess);
            Console.WriteLine(json);
        }

        private string StringWordsRemove(string stringToClean,List<string> wordsToRemove)
        {
            string regex;

            foreach(string word in wordsToRemove)
            {
                regex = @"\b" + word + @"\b";
                stringToClean = Regex.Replace(stringToClean, regex, "");
            }

            stringToClean = Regex.Replace(stringToClean, "  ", " ");
            stringToClean = stringToClean.Trim();

            return stringToClean;
        }

        public int Levenshtein(string a, string b)
        {
            int n = a.Length;
            int m = b.Length;

            if (n > m)
            {
                b = b.PadRight(n);
                m = n;
            }
            else if(m > n)
            {
                a = a.PadRight(m);
                n = m;
            }

            int cost = 0;
            int min1;
            int min2;
            int min3;
            int[,] d = new int[n+1,m+1];
            if (n == 0)
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
            for (int i = 0; i < m; i++)
            {
                d[0,i] = i;
            }
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    cost = (b[j - 1] == a[i - 1]) ? 0 : 1;
                    min1 = d[i - 1, j] + 1;
                    min2 = d[i, j - 1] + 1;
                    min3 = d[i - 1, j - 1] + cost;

                    d[i,j] = Math.Min(Math.Min(min1, min2), min3);

                    if (i > 1 && j > 1 && a[i - 1] == b[j - 2] && a[i - 2] == b[j - 1])
                        d[i, j] = Math.Min(d[i, j], d[i - 2, j - 2] + cost);
                }
            }
            return d[n,m];
        }
    }
}
