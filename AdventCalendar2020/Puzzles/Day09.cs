using System;
using System.Diagnostics;
using System.Linq;

namespace AdventCalendar2020.Puzzles
{
    public class Day09
    {
        private const string DayNumber = "09";

        public void Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result1 = RunPuzzle1();
            stopwatch.Stop();
            Console.WriteLine($"Day {DayNumber} - Puzzle 1: {result1} - Elapsed: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();
            var result2 = RunPuzzle2();
            stopwatch.Stop();
            Console.WriteLine($"Day {DayNumber} - Puzzle 2: {result2} - Elapsed: {stopwatch.ElapsedMilliseconds} ms");
        }

        private string[] GetInputLines()
        {
            return System.IO.File.ReadAllLines($@"inputs\day{DayNumber}.txt");
        }

        /// <summary>
        /// --- Day 3: Toboggan Trajectory ---
        /// With the toboggan login problems resolved, you set off toward the airport.While travel by toboggan might be easy, it's certainly not safe: there's very minimal steering and the area is covered in trees.You'll need to see which angles will take you near the fewest trees.
        /// 
        /// Due to the local geology, trees in this area only grow on exact integer coordinates in a grid.You make a map (your puzzle input) of the open squares(.) and trees(#) you can see. For example:
        /// 
        /// ..##.......
        /// #...#...#..
        /// .#....#..#.
        /// ..#.#...#.#
        /// .#...##..#.
        /// ..#.##.....
        /// .#.#.#....#
        /// .#........#
        /// #.##...#...
        /// #...##....#
        /// .#..#...#.#
        /// These aren't the only trees, though; due to something you read about once involving arboreal genetics and biome stability, the same pattern repeats to the right many times:
        /// 
        /// ..##.........##.........##.........##.........##.........##.......  --->
        /// #...#...#..#...#...#..#...#...#..#...#...#..#...#...#..#...#...#..
        /// .#....#..#..#....#..#..#....#..#..#....#..#..#....#..#..#....#..#.
        /// ..#.#...#.#..#.#...#.#..#.#...#.#..#.#...#.#..#.#...#.#..#.#...#.#
        /// .#...##..#..#...##..#..#...##..#..#...##..#..#...##..#..#...##..#.
        /// ..#.##.......#.##.......#.##.......#.##.......#.##.......#.##.....  --->
        /// .#.#.#....#.#.#.#....#.#.#.#....#.#.#.#....#.#.#.#....#.#.#.#....#
        /// .#........#.#........#.#........#.#........#.#........#.#........#
        /// #.##...#...#.##...#...#.##...#...#.##...#...#.##...#...#.##...#...
        /// #...##....##...##....##...##....##...##....##...##....##...##....#
        /// .#..#...#.#.#..#...#.#.#..#...#.#.#..#...#.#.#..#...#.#.#..#...#.#  --->
        /// You start on the open square (.) in the top-left corner and need to reach the bottom(below the bottom-most row on your map).
        /// 
        /// The toboggan can only follow a few specific slopes(you opted for a cheaper model that prefers rational numbers); start by counting all the trees you would encounter for the slope right 3, down 1:
        /// 
        /// From your starting position at the top-left, check the position that is right 3 and down 1. Then, check the position that is right 3 and down 1 from there, and so on until you go past the bottom of the map.
        /// 
        /// The locations you'd check in the above example are marked here with O where there was an open square and X where there was a tree:
        /// 
        /// ..##.........##.........##.........##.........##.........##.......  --->
        /// #..O#...#..#...#...#..#...#...#..#...#...#..#...#...#..#...#...#..
        /// .#....X..#..#....#..#..#....#..#..#....#..#..#....#..#..#....#..#.
        /// ..#.#...#O#..#.#...#.#..#.#...#.#..#.#...#.#..#.#...#.#..#.#...#.#
        /// .#...##..#..X...##..#..#...##..#..#...##..#..#...##..#..#...##..#.
        /// ..#.##.......#.X#.......#.##.......#.##.......#.##.......#.##.....  --->
        /// .#.#.#....#.#.#.#.O..#.#.#.#....#.#.#.#....#.#.#.#....#.#.#.#....#
        /// .#........#.#........X.#........#.#........#.#........#.#........#
        /// #.##...#...#.##...#...#.X#...#...#.##...#...#.##...#...#.##...#...
        /// #...##....##...##....##...#X....##...##....##...##....##...##....#
        /// .#..#...#.#.#..#...#.#.#..#...X.#.#..#...#.#.#..#...#.#.#..#...#.#  --->
        /// In this example, traversing the map using this slope would cause you to encounter 7 trees.
        /// 
        /// Starting at the top-left corner of your map and following a slope of right 3 and down 1, how many trees would you encounter?
        /// </summary>
        private int RunPuzzle1()
        {
            var validPasswordCount = 0;

            var inputLines = GetInputLines();

            foreach (var line in inputLines)
            {
                var lineParts = line.Split(' ');

                var minMax = lineParts[0].Split('-');
                var min = Convert.ToInt32(minMax[0]);
                var max = Convert.ToInt32(minMax[1]);

                var letter = lineParts[1].ToCharArray()[0];

                var count = lineParts[2].Count(x => x == letter);

                if (count >= min && count <= max)
                {
                    validPasswordCount++;
                }
            }

            return validPasswordCount;
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
