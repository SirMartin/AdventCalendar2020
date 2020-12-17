using System.Collections.Generic;
using System.Linq;
using AdventCalendar2020.Interfaces;

namespace AdventCalendar2020.Puzzles
{
    public class Day06 : AdventCalendarDay
    {
        public override string DayNumber =>  "06";
        public override (string, string) ExpectedResult => ("6549", "3466");

        /// <summary>
        /// --- Day 6: Custom Customs ---
        /// As your flight approaches the regional airport where you'll switch to a much larger plane, customs declaration forms are distributed to the passengers.

        /// The form asks a series of 26 yes-or-no questions marked a through z.All you need to do is identify the questions for which anyone in your group answers "yes". Since your group is just you, this doesn't take very long.

        /// However, the person sitting next to you seems to be experiencing a language barrier and asks if you can help.For each of the people in their group, you write down the questions for which they answer "yes", one per line.For example:

        ///  abcx
        ///  abcy
        ///  abcz
        /// In this group, there are 6 questions to which anyone answered "yes": a, b, c, x, y, and z. (Duplicate answers to the same question don't count extra; each question counts at most once.)

        /// Another group asks for your help, then another, and eventually you've collected answers from every group on the plane (your puzzle input). Each group's answers are separated by a blank line, and within each group, each person's answers are on a single line. For example:

        ///  abc

        ///  a
        ///  b
        ///  c

        ///  ab
        ///  ac

        ///  a
        ///  a
        ///  a
        ///  a

        ///  b
        /// 
        /// This list represents answers from five groups:

        /// The first group contains one person who answered "yes" to 3 questions: a, b, and c.
        /// The second group contains three people; combined, they answered "yes" to 3 questions: a, b, and c.
        /// The third group contains two people; combined, they answered "yes" to 3 questions: a, b, and c.
        /// The fourth group contains four people; combined, they answered "yes" to only 1 question, a.
        /// The last group contains one person who answered "yes" to only 1 question, b.
        /// In this example, the sum of these counts is 3 + 3 + 3 + 1 + 1 = 11.

        /// For each group, count the number of questions to which anyone answered "yes". What is the sum of those counts?
        /// </summary>
        internal override string RunPuzzle1()
        {
            var numberOfYes = 0;

            var inputLines = GetInputLines();

            var groupAnswers = new List<char>();
            foreach (var line in inputLines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    // New group, clean variables.
                    numberOfYes += groupAnswers.Count;
                    groupAnswers = new List<char>();
                    continue;
                }

                foreach (var answer in line.ToCharArray())
                {
                    if (!groupAnswers.Contains(answer))
                    {
                        // Add to the list.
                        groupAnswers.Add(answer);
                    }
                }
            }

            // Count the last group.
            numberOfYes += groupAnswers.Count;

            return numberOfYes.ToString();
        }

        /// <summary>
        /// --- Part Two ---
        /// As you finish the last group's customs declaration, you notice that you misread one word in the instructions:
        ///
        /// You don't need to identify the questions to which anyone answered "yes"; you need to identify the questions to which everyone answered "yes"!
        ///
        /// Using the same example as above:
        ///
        /// abc
        ///
        /// a
        /// b
        /// c
        ///
        /// ab
        /// ac
        ///
        /// a
        /// a
        /// a
        /// a
        ///
        /// b
        /// This list represents answers from five groups:
        ///
        /// In the first group, everyone (all 1 person) answered "yes" to 3 questions: a, b, and c.
        /// In the second group, there is no question to which everyone answered "yes".
        /// In the third group, everyone answered yes to only 1 question, a.Since some people did not answer "yes" to b or c, they don't count.
        /// In the fourth group, everyone answered yes to only 1 question, a.
        /// In the fifth group, everyone (all 1 person) answered "yes" to 1 question, b.
        /// In this example, the sum of these counts is 3 + 0 + 1 + 1 + 1 = 6.
        ///
        /// For each group, count the number of questions to which everyone answered "yes". What is the sum of those counts?
        /// </summary>
        internal override string RunPuzzle2()
        {
            var numberOfYes = 0;

            var inputLines = GetInputLines();

            var groupAnswers = new List<char>();
            var firstPersonInGroup = true;
            foreach (var line in inputLines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    // New group, clean variables.
                    numberOfYes += groupAnswers.Count;
                    groupAnswers.Clear();
                    firstPersonInGroup = true;
                    continue;
                }

                // First person of group.
                if (firstPersonInGroup)
                {
                    firstPersonInGroup = false;
                    groupAnswers.AddRange(line.ToCharArray());
                    continue;
                }

                // Rest of persons of the group.
                var filteredGroupAnswers = new List<char>();
                foreach (var answer in groupAnswers)
                {
                    if (line.ToCharArray().Contains(answer))
                    {
                        filteredGroupAnswers.Add(answer);
                    }
                }

                groupAnswers = new List<char>(filteredGroupAnswers);
            }

            // Count the last group.
            numberOfYes += groupAnswers.Count;

            return numberOfYes.ToString();
        }
    }
}
