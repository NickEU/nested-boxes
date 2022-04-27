using fancy_boxes.src.Interfaces;
using System;

namespace fancy_boxes.src.Implementations
{
    public class ConsoleOutput : IOutput
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}
