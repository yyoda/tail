using System;

namespace Tail
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                new Command(args).DoTail();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
