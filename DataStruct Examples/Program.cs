using System;

namespace DataStruct_Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the data structure demos!");
            Console.WriteLine("Please select a demo:");
            Console.WriteLine("1. Timeline (Confluently persistent temporal data structure)");
            Console.WriteLine("2. TODO");
            int selection = Int32.Parse(Console.ReadLine());
            switch (selection) {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:

                    break;
                case 2:
                    FractionalCascading.Demo();
                    break;
            }
            Console.ReadLine();
        }
    }
}
