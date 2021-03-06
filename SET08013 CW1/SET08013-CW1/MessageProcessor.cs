﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Windows;

namespace SET08013_CW1
{
    class MessageProcessor
    {
        private const string _badWordFilePath    = "../textwords.csv";
        private const string _universityFilePath = "../University List.csv";
        private const string _subjectsFilePath   = "../subjects.csv";
        private const string _validFilePath      = "../Valid Messages.txt";
        private const string _quarantineFilePath = "../Quarantine Messages.txt";
        private const string _jsonFilePath       = "../../json.txt";
        private       string _inputMessage;
        private List<string> _validMessages      = new List<string>();
        
        public void InputMessage(string message)
        {
            _inputMessage = message;
            if (IsValidMessage())
            {
                AppendMessageToFile(message, _validFilePath);
            }
            else
            {
                AppendMessageToFile(message, _quarantineFilePath);
            }
        }

        public void ProcessValidMessages()
        {
            ReadValidMessages();
            if (File.Exists(@_jsonFilePath))
            {
                File.Delete(@_jsonFilePath);
            }
            foreach (string message in _validMessages)
            {
                SearchMessage(message);
            }
        }

        public List<Message> GetApplications()
        {
            List<Message> applications = new List<Message>();
            try
            {
                StreamReader reader = new StreamReader(File.OpenRead(@_jsonFilePath));

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] jsons = line.Split('|');
                    foreach (string jsonString in jsons)
                    {
                        applications.Add(JsonHelper.JsonDeserialize<Message>(jsonString));
                    }
                }
                reader.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show("Could not find file '" + _jsonFilePath + "'.");
            }
            return applications;
        }

        public List<string> GetQuarantinedMessages()
        {
            List<string> messages = new List<string>();
            try
            {
                StreamReader reader = new StreamReader(File.OpenRead(@_quarantineFilePath));

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] text = line.Split('|');
                    foreach (string s in text)
                    {
                        messages.Add(s);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not find file '" + _quarantineFilePath + "'.");
            }
            return messages;
        }

        private bool IsValidMessage()
        {
            StreamReader reader = new StreamReader(File.OpenRead(@_badWordFilePath));

            while(!reader.EndOfStream)
            {
                string   line  = reader.ReadLine();
                string[] words = line.Split(',');
                string   regex = @"\b" + words[0].ToLower() + @"\b";  //Match entire word only.

                if (Regex.IsMatch(_inputMessage.ToLower(), regex))
                {
                    return false;
                }
            }
            return true;
        }

        private void AppendMessageToFile(string message, string filePath)
        {
            message = message.Replace("|", "");

            if (!File.Exists(filePath))
            {
                File.AppendAllText(filePath, message);
            }
            else
            {
                File.AppendAllText(filePath, "|" + message);
            }
        }

        private void ReadValidMessages()
        {
            try
            {
                StreamReader reader = new StreamReader(File.OpenRead(@_validFilePath));
                _validMessages.Clear();

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] text = line.Split('|');
                    foreach (string s in text)
                    {
                        _validMessages.Add(s);
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Could not find file '" + _validFilePath + "'.");
            }
        }

        private void SearchMessage(string message)
        {
            //Determine whether the message is UG or PG
            List<string> subjects      = new List<string>();
            List<string> universities  = new List<string>();
            List<string> wordsToRemove = "University of".Split(' ').ToList<string>();

            String level   = "NONE";
            string ugRegex = @"(\bug\b)|(\bu/g\b)|(\bunder graduate\b)|(\bundergraduate\b)";
            string pgRegex = @"(\bpg\b)|(\bp/g\b)|(\bpost graduate\b)|(\bpostgraduate\b)";

            if (Regex.IsMatch(message.ToLower(), ugRegex))
            {
                level = "Undergraduate";
            }
            else if (Regex.IsMatch(message.ToLower(), pgRegex))
            {
                level = "Postgraduate";
            }

            StreamReader reader = new StreamReader(File.OpenRead(@_universityFilePath));
            while (!reader.EndOfStream)
            {
                string    line      = reader.ReadLine();
                string    editLine  = StringWordsRemove(line, wordsToRemove).ToLower();
                const int threshold = 1;

                if(IsApproximateMatch(message.ToLower(), editLine, threshold))
                {
                    if (!universities.Contains(line))
                    {
                        universities.Add(line);
                    }
                } 
            }

            reader = new StreamReader(File.OpenRead(@_subjectsFilePath));
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string editLine = line.Trim().ToLower();
                const int threshold = 1;

                if (IsApproximateMatch(message.ToLower(), editLine, threshold))
                {
                    if (!subjects.Contains(line))
                    {
                        subjects.Add(line);
                    }
                }
            }

            Message mess = new Message(level, message, subjects, universities);
            string json = JsonHelper.JsonSerializer<Message>(mess);
            AppendMessageToFile(json, _jsonFilePath);
        }


        private string StringWordsRemove(string stringToClean,List<string> wordsToRemove)
        {
            string regex;

            foreach(string word in wordsToRemove)
            {
                regex = @"\b" + word + @"\b";
                stringToClean = Regex.Replace(stringToClean, regex, "");
                stringToClean = stringToClean.Replace("|", " ");
            }

            stringToClean = Regex.Replace(stringToClean, "  ", " ");
            stringToClean = stringToClean.Trim();

            return stringToClean;
        }

        private bool IsApproximateMatch(string a, string b, int threshold)
        {
            int limit = a.Length - b.Length + 1;
            if(limit <= 0)
            {
                if (Levenshtein(a, b) <= threshold)
                {
                    return true;
                }
            }

            for (int i = 0; i < limit; i++)
            {
                string aSubstring = a.Substring(i, b.Length);
                if (Levenshtein(aSubstring, b) <= threshold)
                {
                    return true;
                }
            }
            return false;
        }

        private int Levenshtein(string a, string b)
        {
            int n = a.Length;
            int m = b.Length;

            /*
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
            */

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
