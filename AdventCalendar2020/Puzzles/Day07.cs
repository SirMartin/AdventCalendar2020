using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventCalendar2020.Puzzles
{
    public class Day07
    {
        private const string DayNumber = "07";

        public void Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result1 = RunPuzzle1();
            stopwatch.Stop();
            Console.WriteLine($"Day {DayNumber} - Puzzle 1: {result1} - Elapsed: {stopwatch.ElapsedMilliseconds} ms");
            //stopwatch.Restart();
            //var result2 = RunPuzzle2();
            //stopwatch.Stop();
            //Console.WriteLine($"Day {DayNumber} - Puzzle 2: {result2} - Elapsed: {stopwatch.ElapsedMilliseconds} ms");
        }

        private string[] GetInputLines()
        {
            return System.IO.File.ReadAllLines($@"inputs\day{DayNumber}.txt");
        }

        /// <summary>
        /// --- Day 7: Handy Haversacks ---
        /// You land at the regional airport in time for your next flight.In fact, it looks like you'll even have time to grab some food: all flights are currently delayed due to issues in luggage processing.
        /// 
        /// Due to recent aviation regulations, many rules(your puzzle input) are being enforced about bags and their contents; bags must be color-coded and must contain specific quantities of other color-coded bags.Apparently, nobody responsible for these regulations considered how long they would take to enforce!
        /// 
        /// 
        /// For example, consider the following rules:
        /// 
        /// 
        /// light red bags contain 1 bright white bag, 2 muted yellow bags.
        /// dark orange bags contain 3 bright white bags, 4 muted yellow bags.
        /// bright white bags contain 1 shiny gold bag.
        /// muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
        /// shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
        /// dark olive bags contain 3 faded blue bags, 4 dotted black bags.
        /// vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
        /// faded blue bags contain no other bags.
        /// dotted black bags contain no other bags.
        /// These rules specify the required contents for 9 bag types. In this example, every faded blue bag is empty, every vibrant plum bag contains 11 bags (5 faded blue and 6 dotted black), and so on.
        /// 
        /// You have a shiny gold bag.If you wanted to carry it in at least one other bag, how many different bag colors would be valid for the outermost bag? (In other words: how many colors can, eventually, contain at least one shiny gold bag?)
        /// 
        /// In the above rules, the following options would be available to you:
        /// 
        /// A bright white bag, which can hold your shiny gold bag directly.
        /// A muted yellow bag, which can hold your shiny gold bag directly, plus some other bags.
        /// A dark orange bag, which can hold bright white and muted yellow bags, either of which could then hold your shiny gold bag.
        /// A light red bag, which can hold bright white and muted yellow bags, either of which could then hold your shiny gold bag.
        /// So, in this example, the number of bag colors that can eventually contain at least one shiny gold bag is 4.
        /// 
        /// How many bag colors can eventually contain at least one shiny gold bag? (The list of rules is quite long; make sure you get all of it.)
        /// </summary>
        private int RunPuzzle1()
        {
            const string myBag = "shiny gold";

            var colorsToIterate = GetColorsCanCarryMyBag(new List<string>() { myBag }, new List<string>());

            var totalColorsCanCarryMyBag = new List<string>(){};
            totalColorsCanCarryMyBag.AddRange(colorsToIterate);

            var foundNewColors = true;
            while (foundNewColors)
            {
                var newColorsFounded = GetColorsCanCarryMyBag(colorsToIterate, totalColorsCanCarryMyBag);

                if (!newColorsFounded.Any())
                {
                    foundNewColors = false;
                }

                totalColorsCanCarryMyBag.AddRange(newColorsFounded);
                colorsToIterate = new List<string>(newColorsFounded);
            }

            return totalColorsCanCarryMyBag.Count;
        }

        private List<string> GetColorsCanCarryMyBag(List<string> colorsToSearch, List<string> colorsFoundedForNow)
        {
            var inputLines = GetInputLines();

            var colorsCanCarryMyBag = new List<string>();
            foreach (var line in inputLines)
            {
                var content = line.Substring(line.IndexOf("contain") + "contain".Length);
                foreach (var colorCarried in colorsToSearch)
                {
                    if (content.Contains(colorCarried))
                    {
                        var bagColor = line.Remove(line.IndexOf(' ', line.IndexOf(' ') + 1));

                        if (!colorsFoundedForNow.Contains(bagColor))
                        {
                            colorsCanCarryMyBag.Add(bagColor);
                        }

                        break;
                    }
                }
            }

            return colorsCanCarryMyBag;
        }

        /// <summary>
        ///    --- Part Two ---
        ///    While it appears you validated the passwords correctly, they don't seem to be what the Official Toboggan Corporate Authentication System is expecting.
        ///
        ///    The shopkeeper suddenly realizes that he just accidentally explained the password policy rules from his old job at the sled rental place down the street! The Official Toboggan Corporate Policy actually works a little differently.
        ///
        ///    Each policy actually describes two positions in the password, where 1 means the first character, 2 means the second character, and so on. (Be careful; Toboggan Corporate Policies have no concept of "index zero"!) Exactly one of these positions must contain the given letter.Other occurrences of the letter are irrelevant for the purposes of policy enforcement.
        ///
        ///    Given the same example list from above:
        ///
        ///       1-3 a: abcde is valid: position 1 contains a and position 3 does not.
        ///       1-3 b: cdefg is invalid: neither position 1 nor position 3 contains b.
        ///       2-9 c: ccccccccc is invalid: both position 2 and position 9 contain c.
        ///    How many passwords are valid according to the new interpretation of the policies?
        /// </summary>
        private int RunPuzzle2()
        {
            var validPasswordCount = 0;

            var inputLines = GetInputLines();

            foreach (var line in inputLines)
            {
                var lineParts = line.Split(' ');

                var positions = lineParts[0].Split('-');
                var pos1 = Convert.ToInt32(positions[0]);
                var pos2 = Convert.ToInt32(positions[1]);

                var letter = lineParts[1].ToCharArray()[0];

                var passwordLetters = lineParts[2].ToCharArray();

                if ((passwordLetters[pos1 - 1] == letter && passwordLetters[pos2 - 1] != letter) ||
                    (passwordLetters[pos1 - 1] != letter && passwordLetters[pos2 - 1] == letter))
                {
                    validPasswordCount++;
                }
            }

            return validPasswordCount;
        }
    }
}
