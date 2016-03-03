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
            MessageProcessor processor = new MessageProcessor();

            Console.WriteLine("****************************");
            Console.WriteLine("* NOOGLE MESSAGE PROCESSOR *");
            Console.WriteLine("****************************");
            Console.WriteLine("1. Input message");
            Console.WriteLine("2. Process valid messages");
            Console.WriteLine("3. Exit");
            processor.ProcessMessage(Console.ReadLine());
            Console.ReadLine();
        }
    }
}
