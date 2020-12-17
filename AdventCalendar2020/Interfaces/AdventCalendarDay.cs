using System;
using System.Diagnostics;

namespace AdventCalendar2020.Interfaces
{
    public abstract class AdventCalendarDay : IAdventCalendarDay
    {
        public abstract string DayNumber { get; }
        public abstract (string, string) ExpectedResult { get; }

        public void Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result1 = RunPuzzle1();
            stopwatch.Stop();
            var time1 = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();
            var result2 = RunPuzzle2();
            var time2 = stopwatch.ElapsedMilliseconds;
            stopwatch.Stop();

            PrintDayResults(result1, result2, time1, time2);
        }

        internal abstract string RunPuzzle1();
        internal abstract string RunPuzzle2();

        internal string[] GetInputLines()
        {
            return System.IO.File.ReadAllLines($@"inputs\day{DayNumber}.txt");
        }

        internal void PrintDayResults(string result1, string result2, long time1, long time2)
        {
            Console.WriteLine($"Day {DayNumber} - Puzzle 1: {result1}{(result1 == ExpectedResult.Item1 ? (char)0x221A : ' ')} - Elapsed: {time1} ms");
            Console.WriteLine($"Day {DayNumber} - Puzzle 2: {result2}{(result2 == ExpectedResult.Item2 ? (char)0x221A : ' ')} - Elapsed: {time2} ms");
        }
    }
}