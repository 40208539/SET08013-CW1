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

            Console.WriteLine("Please input message...");

            processor.ProcessMessage(Console.ReadLine());
            Console.ReadLine();
        }
    }
}
