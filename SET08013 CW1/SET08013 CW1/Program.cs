using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SET08013_CW1
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;
            MessageProcessor processor = new MessageProcessor();
            do
            {
                Console.Clear();
                ShowMenu();

                cki = Console.ReadKey();
                switch (cki.KeyChar.ToString())
                {
                    case "1":
                        Console.Clear();
                        processor.InputMessage(Console.ReadLine());
                        break;
                    case "2":
                        Console.Clear();
                        processor.ProcessValidMessages();
                        Console.ReadLine();
                        break;
                }
            }
            while (cki.Key != ConsoleKey.Escape || cki.Key.ToString() != "3");
        }

        static void ShowMenu()
        {
            Console.WriteLine("****************************");
            Console.WriteLine("* NOOGLE MESSAGE PROCESSOR *");
            Console.WriteLine("****************************");
            Console.WriteLine("1. Input message");
            Console.WriteLine("2. Process valid messages");
            Console.WriteLine("3. Exit");
        }
    }
}
