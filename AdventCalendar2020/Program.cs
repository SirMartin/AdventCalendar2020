using System;
using System.Diagnostics;
using AdventCalendar2020.Puzzles;

namespace AdventCalendar2020
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //new Day01().Run();
            //new Day02().Run();
            //new Day03().Run();
            //new Day04().Run();
            //new Day05().Run();
            //new Day06().Run();
            //new Day07().Run();
            //new Day08().Run();
            //new Day09().Run();
            //new Day10().Run();
            //new Day11().Run();
            //new Day12().Run();
            //new Day13().Run();
            //new Day14().Run();
            //new Day15().Run();
            //new Day16().Run();
            new Day17().Run();
            //new Day18().Run();
            //new Day19().Run();
            //new Day20().Run();
            //new Day21().Run();
            //new Day22().Run();
            //new Day23().Run();
            //new Day24().Run();
            //new Day25().Run();

            stopwatch.Stop();
            Console.WriteLine($"Total time elapsed: {stopwatch.ElapsedMilliseconds} ms.");

            Console.ReadLine();
        }
    }
}
