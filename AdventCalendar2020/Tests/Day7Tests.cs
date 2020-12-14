using AdventCalendar2020.Puzzles;
using NUnit.Framework;

namespace AdventCalendar2020.Tests
{
    [TestFixture]
    public class Day7Tests
    {
        private Day07 _day07;

        [Test]
        public void ShinyGold_NoBagsInside_Returns0()
        {
            var inputLines = System.IO.File.ReadAllLines($@"Tests\Inputs\day07_ShinyGold_NoBagsInside_Returns0.txt");
            _day07 = new Day07(inputLines);
            var myBag = _day07.GetMyBag("shiny gold");
            Assert.AreEqual(0, myBag.ContentNumber);
        }

        [Test]
        public void ShinyGold_2DifferentEmptyBagsInside_Returns2()
        {
            var inputLines = System.IO.File.ReadAllLines($@"Tests\Inputs\day07_ShinyGold_2DifferentEmptyBagsInside_Returns2.txt");
            _day07 = new Day07(inputLines);
            var myBag = _day07.GetMyBag("shiny gold");
            Assert.AreEqual(2, myBag.ContentNumber);
        }

        [Test]
        public void ShinyGold_2BagLevels_Returns3()
        {
            var inputLines = System.IO.File.ReadAllLines($@"Tests\Inputs\ShinyGold_2BagLevels_Returns3.txt");
            _day07 = new Day07(inputLines);
            var myBag = _day07.GetMyBag("shiny gold");
            Assert.AreEqual(3, myBag.ContentNumber);
        }

        [Test]
        public void ShinyGold_Example1_Returns32()
        {
            var inputLines = System.IO.File.ReadAllLines($@"Tests\Inputs\day07_BasicTest1.txt");
            _day07 = new Day07(inputLines);
            var myBag = _day07.GetMyBag("shiny gold");
            Assert.AreEqual(32, myBag.ContentNumber);
        }

        [Test]
        public void ShinyGold_Example1_Returns126()
        {
            var inputLines = System.IO.File.ReadAllLines($@"Tests\Inputs\day07_BasicTest2.txt");
            _day07 = new Day07(inputLines);
            var myBag = _day07.GetMyBag("shiny gold");
            Assert.AreEqual(126, myBag.ContentNumber);
        }
    }
}
