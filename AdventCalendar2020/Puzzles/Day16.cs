using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventCalendar2020.Puzzles
{
    public class Day16
    {
        private const string DayNumber = "16";

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
        /// --- Day 16: Ticket Translation ---
        /// As you're walking to yet another connecting flight, you realize that one of the legs of your re-routed trip coming up is on a high-speed train. However, the train ticket you were given is in a language you don't understand.You should probably figure out what it says before you get to the train station after the next flight.
        ///
        /// Unfortunately, you can't actually read the words on the ticket. You can, however, read the numbers, and so you figure out the fields these tickets must have and the valid ranges for values in those fields.
        ///
        /// You collect the rules for ticket fields, the numbers on your ticket, and the numbers on other nearby tickets for the same train service (via the airport security cameras) together into a single document you can reference(your puzzle input).
        ///
        /// The rules for ticket fields specify a list of fields that exist somewhere on the ticket and the valid ranges of values for each field.For example, a rule like class: 1-3 or 5-7 means that one of the fields in every ticket is named class and can be any value in the ranges 1-3 or 5-7 (inclusive, such that 3 and 5 are both valid in this field, but 4 is not).
        ///
        /// Each ticket is represented by a single line of comma-separated values.The values are the numbers on the ticket in the order they appear; every ticket has the same format.For example, consider this ticket:
        ///
        /// .--------------------------------------------------------.
        /// | ????: 101    ?????: 102   ??????????: 103     ???: 104 |
        /// |                                                        |
        /// | ??: 301  ??: 302             ???????: 303      ??????? |
        /// | ??: 401  ??: 402           ???? ????: 403    ????????? |
        /// '--------------------------------------------------------'
        /// Here, ? represents text in a language you don't understand. This ticket might be represented as 101,102,103,104,301,302,303,401,402,403; of course, the actual train tickets you're looking at are much more complicated.In any case, you've extracted just the numbers in such a way that the first number is always the same specific field, the second number is always a different specific field, and so on - you just don't know what each position actually means!
        ///
        ///
        /// Start by determining which tickets are completely invalid; these are tickets that contain values which aren't valid for any field. Ignore your ticket for now.
        ///
        /// For example, suppose you have the following notes:
        ///
        /// class: 1-3 or 5-7
        /// row: 6-11 or 33-44
        /// seat: 13-40 or 45-50
        ///
        /// your ticket:
        /// 7,1,14
        ///
        /// nearby tickets:
        /// 7,3,47
        /// 40,4,50
        /// 55,2,20
        /// 38,6,12
        /// It doesn't matter which position corresponds to which field; you can identify invalid nearby tickets by considering only whether tickets contain values that are not valid for any field. In this example, the values on the first nearby ticket are all valid for at least one field. This is not true of the other three nearby tickets: the values 4, 55, and 12 are are not valid for any field. Adding together all of the invalid values produces your ticket scanning error rate: 4 + 55 + 12 = 71.
        ///
        /// Consider the validity of the nearby tickets you scanned.What is your ticket scanning error rate?
        /// </summary>
        private int RunPuzzle1()
        {
            var inputLines = GetInputLines();

            var rules = new Dictionary<string, string>();
            var myTicket = new List<KeyValuePair<int, bool>>();
            var nearbyTickets = new List<List<KeyValuePair<int, bool>>>();

            var isMyTicket = false;
            var isOtherTickets = false;
            for (var i = 0; i < inputLines.Length; i++)
            {
                var line = inputLines[i];

                if (string.IsNullOrWhiteSpace(line))
                {
                    // Change type.
                    if (!isMyTicket)
                    {
                        isMyTicket = true;
                        i++;
                    }
                    else if (!isOtherTickets)
                    {
                        isOtherTickets = true;
                        i++;
                    }

                    continue;
                }

                if (!isMyTicket)
                {
                    // Rules
                    var name = line.Split(':')[0];
                    var rulesText = line.Substring(line.IndexOf(':') + 1).Trim();

                    rules.Add(name, TranslateRules(rulesText));
                }
                else if (!isOtherTickets)
                {
                    // My ticket.
                    myTicket = ParseTicket(line);
                }
                else
                {
                    // Nearby tickets.
                    nearbyTickets.Add(ParseTicket(line));
                }
            }

            return -1;
        }

        private string TranslateRules(string value)
        {
            // Take the different parts.
            var parts = value.Split(new [] { "or" }, StringSplitOptions.None).Select(x => x.Trim());

            var results = new List<string>();
            foreach (var part in parts)
            {
                var p = part.Split('-');
                var min = Convert.ToInt32(p[0]);
                var max = Convert.ToInt32(p[1]);
                results.Add(string.Join(",", Enumerable.Range(min, max - min + 1)));
            }
            

            return string.Join(",", results);
        }

        private List<KeyValuePair<int, bool>> ParseTicket(string line)
        {
            return line.Split(',').Select(x => new KeyValuePair<int, bool>(Convert.ToInt32(x), false)).ToList();
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
