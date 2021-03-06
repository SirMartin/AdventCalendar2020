﻿using System;
using System.Collections.Generic;
using System.Linq;
using AdventCalendar2020.Interfaces;

namespace AdventCalendar2020.Puzzles
{
    public class Day07 : AdventCalendarDay
    {
        private string[] _inputLines;

        public Day07()
        {
            _inputLines = null;
        }

        public Day07(string[] inputLines)
        {
            _inputLines = inputLines;
        }

        public override string DayNumber =>  "07";
        public override (string, string) ExpectedResult => ("372", "8015");


        public new string[] GetInputLines()
        {
            if (_inputLines != null)
            {
                return _inputLines;
            }

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
        internal override string RunPuzzle1()
        {
            const string myBag = "shiny gold";

            var colorsToIterate = GetColorsCanCarryMyBag(new List<string>() { myBag }, new List<string>());

            var totalColorsCanCarryMyBag = new List<string>() { };
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

            return totalColorsCanCarryMyBag.Count.ToString();
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
        /// --- Part Two ---
        /// It's getting pretty expensive to fly these days - not because of ticket prices, but because of the ridiculous number of bags you need to buy!
        /// 
        /// Consider again your shiny gold bag and the rules from the above example:
        /// 
        /// faded blue bags contain 0 other bags.
        /// dotted black bags contain 0 other bags.
        /// vibrant plum bags contain 11 other bags: 5 faded blue bags and 6 dotted black bags.
        /// dark olive bags contain 7 other bags: 3 faded blue bags and 4 dotted black bags.
        ///
        /// So, a single shiny gold bag must contain 1 dark olive bag (and the 7 bags within it) plus 2 vibrant plum bags(and the 11 bags within each of those) : 1 + 1*7 + 2 + 2*11 = 32 bags!
        /// 
        /// Of course, the actual rules have a small chance of going several levels deeper than this example; be sure to count all of the bags, even if the nesting becomes topologically impractical!
        /// 
        /// Here's another example:
        /// 
        /// shiny gold bags contain 2 dark red bags.
        /// dark red bags contain 2 dark orange bags.
        /// dark orange bags contain 2 dark yellow bags.
        /// dark yellow bags contain 2 dark green bags.
        /// dark green bags contain 2 dark blue bags.
        /// dark blue bags contain 2 dark violet bags.
        /// dark violet bags contain no other bags.
        ///
        /// In this example, a single shiny gold bag must contain 126 other bags.
        /// How many individual bags are required inside your single shiny gold bag?
        /// </summary>
        internal override string RunPuzzle2()
        {
            var myBag = GetMyBag("shiny gold");
            return myBag.ContentNumber.ToString();
        }

        #region Bad Approaches
        internal int GetNumberOfBagsInsideTheBag(string colorToFind)
        {
            var inputLines = GetInputLines();

            var colorLine = inputLines.First(x => x.StartsWith(colorToFind));
            var content = colorLine.Substring(colorLine.IndexOf("contain") + "contain".Length).Trim().Replace(".", "");
            var bags = content.Split(',').Select(x => x.Trim());

            var num = 0;
            foreach (var bag in bags)
            {
                if (bag == "no other bags")
                {
                    return 0;
                }

                var keyValueSeparator = bag.IndexOf(' ');
                var value = Convert.ToInt32(bag.Substring(0, keyValueSeparator).Trim());
                var key = bag.Substring(keyValueSeparator).Replace("bags", string.Empty).Replace("bag", string.Empty).Trim();

                var result = GetNumberOfBagsInsideTheBag(key);

                if (result == 0)
                {
                    num += value;
                    continue;
                }

                return value * result;
            }

            return num;
        }

        private int GetNumberOfBagsFromThisBag(string colorCarried, int previousAmount)
        {
            var inputLines = GetInputLines();

            var totalBags = 0;

            foreach (var line in inputLines)
            {
                if (line.StartsWith(colorCarried))
                {
                    var content = line.Substring(line.IndexOf("contain") + "contain".Length).Trim().Replace(".", "");
                    var bags = content.Split(',');

                    var numberOfVags = 0;
                    foreach (var bag in bags)
                    {
                        var theBag = bag.Trim();
                        if (theBag == "no other bags")
                        {
                            return 1;
                        }

                        var keyValueSeparator = theBag.IndexOf(' ');
                        var value = Convert.ToInt32(theBag.Substring(0, keyValueSeparator).Trim());
                        var key = theBag.Substring(keyValueSeparator).Replace("bags", string.Empty).Replace("bag", string.Empty).Trim();

                        // Get the inside ones.
                        var result = GetNumberOfBagsFromThisBag(key, value);
                        numberOfVags += value * result;
                    }

                    totalBags += previousAmount + (previousAmount == 0 ? 1 : previousAmount * numberOfVags);
                }
            }

            return totalBags;
        }
        #endregion

        public Bag GetMyBag(string color)
        {
            var myBag = new Bag(color, true);
            myBag.Content = GetBagContent(myBag.Color);
            return myBag;
        }

        private List<Bag> GetBagContent(string color)
        {
            var bagList = new List<Bag>();

            var inputLines = GetInputLines();
            var colorLine = inputLines.First(x => x.StartsWith(color));
            var content = colorLine.Substring(colorLine.IndexOf("contain") + "contain".Length).Trim().Replace(".", "");
            var bags = content.Split(',').Select(x => x.Trim());
            foreach (var bag in bags)
            {
                if (bag == "no other bags")
                {
                    return null;
                }

                var keyValueSeparator = bag.IndexOf(' ');
                var value = Convert.ToInt32(bag.Substring(0, keyValueSeparator).Trim());
                var key = bag.Substring(keyValueSeparator).Replace("bags", string.Empty).Replace("bag", string.Empty).Trim();

                var theBag = new Bag(key, value);
                theBag.Content = GetBagContent(key);
                bagList.Add(theBag);
            }

            return bagList;
        }
    }

    public class Bag
    {
        public Bag(string color, bool isMainBag = false)
        {
            Color = color;
            IsMainBag = isMainBag;
        }

        public Bag(string color, int number) : this(color, false)
        {
            Number = number;
        }

        public int Number { get; set; }
        public string Color { get; set; }
        public List<Bag> Content { get; set; }

        public bool IsMainBag { get; set; }

        public int ContentNumber
        {
            get
            {
                var contentTotal = 0;
                if (Content != null)
                {
                    contentTotal = Content.Sum(x => x.ContentNumber);
                }

                if (IsMainBag)
                {
                    return contentTotal;
                }

                return Number + Number * contentTotal;
            }
        }
    }
}
