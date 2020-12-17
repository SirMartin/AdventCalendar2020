using System;
using System.Collections.Generic;
using System.Linq;
using AdventCalendar2020.Interfaces;

namespace AdventCalendar2020.Puzzles
{
    public class Day17 : AdventCalendarDay
    {
        public override string DayNumber =>  "17";
        public override (string, string) ExpectedResult => ("372", "1896");

        /// <summary>
        /// --- Day 17: Conway Cubes ---
        /// As your flight slowly drifts through the sky, the Elves at the Mythical Information Bureau at the North Pole contact you.They'd like some help debugging a malfunctioning experimental energy source aboard one of their super-secret imaging satellites.
        /// 
        /// The experimental energy source is based on cutting-edge technology: a set of Conway Cubes contained in a pocket dimension! When you hear it's having problems, you can't help but agree to take a look.
        /// 
        /// The pocket dimension contains an infinite 3-dimensional grid. At every integer 3-dimensional coordinate (x, y, z), there exists a single cube which is either active or inactive.
        /// 
        /// In the initial state of the pocket dimension, almost all cubes start inactive.The only exception to this is a small flat region of cubes (your puzzle input); the cubes in this region start in the specified active(#) or inactive (.) state.
        /// 
        /// The energy source then proceeds to boot up by executing six cycles.
        /// 
        /// 
        /// Each cube only ever considers its neighbors: any of the 26 other cubes where any of their coordinates differ by at most 1. For example, given the cube at x= 1, y= 2, z= 3, its neighbors include the cube at x= 2, y= 2, z= 2, the cube at x = 0, y= 2, z= 3, and so on.
        /// 
        /// During a cycle, all cubes simultaneously change their state according to the following rules:
        /// 
        /// 
        /// If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active. Otherwise, the cube becomes inactive.
        /// If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active. Otherwise, the cube remains inactive.
        /// The engineers responsible for this experimental energy source would like you to simulate the pocket dimension and determine what the configuration of cubes should be at the end of the six-cycle boot process.
        /// 
        /// For example, consider the following initial state:
        /// 
        /// .#.
        /// ..#
        /// ###
        /// Even though the pocket dimension is 3-dimensional, this initial state represents a small 2-dimensional slice of it. (In particular, this initial state defines a 3x3x1 region of the 3-dimensional space.)
        /// 
        /// Simulating a few cycles from this initial state produces the following configurations, where the result of each cycle is shown layer-by-layer at each given z coordinate (and the frame of view follows the active cells in each cycle):
        /// 
        /// Before any cycles:
        /// 
        /// z=0
        /// .#.
        /// ..#
        /// ###
        /// 
        /// 
        /// After 1 cycle:
        /// 
        /// z=-1
        /// #..
        /// ..#
        /// .#.
        /// 
        /// z=0
        /// #.#
        /// .##
        /// .#.
        /// 
        /// z=1
        /// #..
        /// ..#
        /// .#.
        /// 
        /// 
        /// After 2 cycles:
        /// 
        /// z=-2
        /// .....
        /// .....
        /// ..#..
        /// .....
        /// .....
        /// 
        /// z=-1
        /// ..#..
        /// .#..#
        /// ....#
        /// .#...
        /// .....
        /// 
        /// z=0
        /// ##...
        /// ##...
        /// #....
        /// ....#
        /// .###.
        /// 
        /// z=1
        /// ..#..
        /// .#..#
        /// ....#
        /// .#...
        /// .....
        /// 
        /// z=2
        /// .....
        /// .....
        /// ..#..
        /// .....
        /// .....
        /// 
        /// 
        /// After 3 cycles:
        /// 
        /// z=-2
        /// .......
        /// .......
        /// ..##...
        /// ..###..
        /// .......
        /// .......
        /// .......
        /// 
        /// z=-1
        /// ..#....
        /// ...#...
        /// #......
        /// .....##
        /// .#...#.
        /// ..#.#..
        /// ...#...
        /// 
        /// z=0
        /// ...#...
        /// .......
        /// #......
        /// .......
        /// .....##
        /// .##.#..
        /// ...#...
        /// 
        /// z=1
        /// ..#....
        /// ...#...
        /// #......
        /// .....##
        /// .#...#.
        /// ..#.#..
        /// ...#...
        /// 
        /// z=2
        /// .......
        /// .......
        /// ..##...
        /// ..###..
        /// .......
        /// .......
        /// .......
        /// After the full six-cycle boot process completes, 112 cubes are left in the active state.
        /// 
        /// Starting with your given initial configuration, simulate six cycles. How many cubes are left in the active state after the sixth cycle?
        /// </summary>
        internal override string RunPuzzle1()
        {
            var cube = new Cube3D(GetInputLines());

            for (var i = 0; i < 6; i++)
            {
                cube.Expand();

                //cube.Print();
            }

            return cube.Actives.Count.ToString();
        }

        //TODO: Needs a big refactoring because it takes almost 5 minutes.
        /// <summary>
        /// --- Part Two ---
        /// For some reason, your simulated results don't match what the experimental energy source engineers expected. Apparently, the pocket dimension actually has four spatial dimensions, not three.
        /// 
        /// The pocket dimension contains an infinite 4-dimensional grid.At every integer 4-dimensional coordinate (x, y, z, w), there exists a single cube (really, a hypercube) which is still either active or inactive.
        /// 
        /// Each cube only ever considers its neighbors: any of the 80 other cubes where any of their coordinates differ by at most 1. For example, given the cube at x= 1, y= 2, z= 3, w= 4, its neighbors include the cube at x= 2, y= 2, z= 3, w= 3, the cube at x = 0, y= 2, z= 3, w= 4, and so on.
        /// 
        /// The initial state of the pocket dimension still consists of a small flat region of cubes. Furthermore, the same rules for cycle updating still apply: during each cycle, consider the number of active neighbors of each cube.
        /// 
        /// For example, consider the same initial state as in the example above.Even though the pocket dimension is 4-dimensional, this initial state represents a small 2-dimensional slice of it. (In particular, this initial state defines a 3x3x1x1 region of the 4-dimensional space.)
        /// 
        /// Simulating a few cycles from this initial state produces the following configurations, where the result of each cycle is shown layer-by-layer at each given z and w coordinate:
        /// 
        /// Before any cycles:
        /// 
        /// z= 0, w= 0
        /// .#.
        /// ..#
        /// ###
        /// 
        /// 
        /// After 1 cycle:
        /// 
        /// z= -1, w= -1
        /// #..
        /// ..#
        /// .#.
        /// 
        /// z= 0, w= -1
        /// #..
        /// ..#
        /// .#.
        /// 
        /// z= 1, w= -1
        /// #..
        /// ..#
        /// .#.
        /// 
        /// z= -1, w= 0
        /// #..
        /// ..#
        /// .#.
        /// 
        /// z= 0, w= 0
        /// #.#
        /// .##
        /// .#.
        /// 
        /// z = 1, w= 0
        /// #..
        /// ..#
        /// .#.
        /// 
        /// z= -1, w= 1
        /// #..
        /// ..#
        /// .#.
        /// 
        /// z= 0, w= 1
        /// #..
        /// ..#
        /// .#.
        /// 
        /// z= 1, w= 1
        /// #..
        /// ..#
        /// .#.
        /// 
        /// 
        /// After 2 cycles:
        /// 
        /// z= -2, w= -2.....
        /// .....
        /// ..#..
        /// .....
        /// .....
        /// 
        /// z= -1, w= -2.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= 0, w= -2
        /// ###..
        /// ##.##
        /// #...#
        /// .#..#
        /// .###.
        /// 
        /// z = 1, w= -2.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= 2, w= -2.....
        /// .....
        /// ..#..
        /// .....
        /// .....
        /// 
        /// z= -2, w= -1.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= -1, w= -1.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= 0, w= -1.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= 1, w= -1.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= 2, w= -1.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= -2, w= 0
        /// ###..
        /// ##.##
        /// #...#
        /// .#..#
        /// .###.
        /// 
        /// z = -1, w= 0.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= 0, w= 0.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= 1, w= 0.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= 2, w= 0
        /// ###..
        /// ##.##
        /// #...#
        /// .#..#
        /// .###.
        /// 
        /// z = -2, w= 1.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= -1, w= 1.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= 0, w= 1.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= 1, w= 1.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= 2, w= 1.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= -2, w= 2.....
        /// .....
        /// ..#..
        /// .....
        /// .....
        /// 
        /// z= -1, w= 2.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= 0, w= 2
        /// ###..
        /// ##.##
        /// #...#
        /// .#..#
        /// .###.
        /// 
        /// z = 1, w= 2.....
        /// .....
        /// .....
        /// .....
        /// .....
        /// 
        /// z= 2, w= 2.....
        /// .....
        /// ..#..
        /// .....
        /// .....
        /// After the full six-cycle boot process completes, 848 cubes are left in the active state.
        /// 
        /// Starting with your given initial configuration, simulate six cycles in a 4-dimensional space. How many cubes are left in the active state after the sixth cycle?
        /// </summary>
        internal override string RunPuzzle2()
        {
            var cube = new Cube4D(GetInputLines());

            for (var i = 0; i < 6; i++)
            {
                cube.Expand();
            }

            return cube.Actives.Count.ToString();
        }
    }

    public class Cube3D
    {
        public Cube3D(string[] inputLines)
        {
            StartDimensions = new Tuple<int, int, int>(inputLines.Length, inputLines[0].Length, 1);

            Actives = new List<Point3D>();

            for (var y = 0; y < inputLines.Length; y++)
            {
                var line = inputLines[y];
                for (var x = 0; x < inputLines[y].Length; x++)
                {
                    if (line[x] == '#')
                    {
                        Actives.Add(new Point3D(x, y, 0));
                    }
                }
            }
        }

        public Tuple<int, int, int> StartDimensions { get; set; }

        public int Cycles { get; set; }

        public List<Point3D> Actives { get; set; }

        public void Expand()
        {
            var newActives = new List<Point3D>();

            var minX = Actives.Select(p => p.X).Min();
            var maxX = Actives.Select(p => p.X).Max();
            var minY = Actives.Select(p => p.Y).Min();
            var maxY = Actives.Select(p => p.Y).Max();
            var minZ = Actives.Select(p => p.Z).Min();
            var maxZ = Actives.Select(p => p.Z).Max();

            for (var z = minZ - 1; z <= maxZ + 1; z++)
            {
                for (var y = minY - 1; y <= maxY + 1; y++)
                {
                    for (var x = minX - 1; x <= maxX + 1; x++)
                    {
                        if (CalculateNewState(x, y, z))
                        {
                            newActives.Add(new Point3D(x, y, z));
                        }
                    }
                }
            }

            Actives = new List<Point3D>(newActives);

            Cycles++;
        }

        private bool CalculateNewState(int x, int y, int z)
        {
            var point = new Point3D(x, y, z);

            var activeNeighbors = GetActiveNeighbors3D(x, y, z);
            if (Actives.Contains(point))
            {
                //If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active. Otherwise, the cube becomes inactive.
                if (activeNeighbors == 2 || activeNeighbors == 3)
                {
                    return true;
                }

                return false;
            }
            else
            {
                //If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active. Otherwise, the cube remains inactive.
                if (activeNeighbors == 3)
                {
                    return true;
                }

                return false;
            }
        }

        private int GetActiveNeighbors3D(int px, int py, int pz)
        {
            var originalPoint = new Point3D(px, py, pz);

            var count = 0;
            for (var z = pz - 1; z <= pz + 1; z++)
            {
                for (var y = py - 1; y <= py + 1; y++)
                {
                    for (var x = px - 1; x <= px + 1; x++)
                    {
                        var point = new Point3D(x, y, z);
                        if (Actives.Contains(point) && !point.Equals(originalPoint))
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        public void Print()
        {
            Console.WriteLine($"After {Cycles} cycles.");

            for (var z = 0 - Cycles; z < StartDimensions.Item3 + Cycles; z++)
            {
                Console.WriteLine($"Z=={z}");
                for (var y = 0 - Cycles; y < StartDimensions.Item2 + Cycles; y++)
                {
                    for (var x = 0 - Cycles; x < StartDimensions.Item1 + Cycles; x++)
                    {
                        var p = new Point3D(x, y, z);
                        Console.Write(Actives.Contains(p) ? "#" : ".");
                    }
                    Console.WriteLine("\r\n");
                }
            }
        }
    }

    public class Point3D
    {
        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override bool Equals(object obj)
        {
            var p = (Point3D)obj;
            return p.X == X && p.Y == Y && p.Z == Z;
        }

        public override string ToString()
        {
            return $"X:{X} Y:{Y} Z:{Z}";
        }
    }

    public class Cube4D
    {
        public Cube4D(string[] inputLines)
        {
            StartDimensions = new Tuple<int, int, int, int>(inputLines.Length, inputLines[0].Length, 1, 1);

            Actives = new List<Point4D>();

            for (var y = 0; y < inputLines.Length; y++)
            {
                var line = inputLines[y];
                for (var x = 0; x < inputLines[y].Length; x++)
                {
                    if (line[x] == '#')
                    {
                        Actives.Add(new Point4D(x, y, 0, 0));
                    }
                }
            }
        }

        public Tuple<int, int, int, int> StartDimensions { get; set; }

        public int Cycles { get; set; }

        public List<Point4D> Actives { get; set; }

        public void Expand()
        {
            var newActives = new List<Point4D>();

            var minX = Actives.Select(p => p.X).Min();
            var maxX = Actives.Select(p => p.X).Max();
            var minY = Actives.Select(p => p.Y).Min();
            var maxY = Actives.Select(p => p.Y).Max();
            var minZ = Actives.Select(p => p.Z).Min();
            var maxZ = Actives.Select(p => p.Z).Max();
            var minW = Actives.Select(p => p.W).Min();
            var maxW = Actives.Select(p => p.W).Max();

            for (var w = minW - 1; w <= maxW + 1; w++)
            {
                for (var z = minZ - 1; z <= maxZ + 1; z++)
                {
                    for (var y = minY - 1; y <= maxY + 1; y++)
                    {
                        for (var x = minX - 1; x <= maxX + 1; x++)
                        {
                            if (CalculateNewState(x, y, z, w))
                            {
                                newActives.Add(new Point4D(x, y, z, w));
                            }
                        }
                    }
                }
            }

            Actives = new List<Point4D>(newActives);

            Cycles++;
        }

        private bool CalculateNewState(int x, int y, int z, int w)
        {
            var point = new Point4D(x, y, z, w);

            var activeNeighbors = GetActiveNeighbors(x, y, z, w);
            if (Actives.Contains(point))
            {
                //If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active. Otherwise, the cube becomes inactive.
                if (activeNeighbors == 2 || activeNeighbors == 3)
                {
                    return true;
                }

                return false;
            }
            else
            {
                //If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active. Otherwise, the cube remains inactive.
                if (activeNeighbors == 3)
                {
                    return true;
                }

                return false;
            }
        }

        private int GetActiveNeighbors(int px, int py, int pz, int pw)
        {
            var originalPoint = new Point4D(px, py, pz, pw);

            var count = 0;
            for (var w = pw - 1; w <= pw + 1; w++)
            {
                for (var z = pz - 1; z <= pz + 1; z++)
                {
                    for (var y = py - 1; y <= py + 1; y++)
                    {
                        for (var x = px - 1; x <= px + 1; x++)
                        {
                            var point = new Point4D(x, y, z, w);
                            if (Actives.Contains(point) && !point.Equals(originalPoint))
                            {
                                count++;
                            }
                        }
                    }
                }
            }

            return count;
        }
    }

    public class Point4D : Point3D
    {
        public Point4D(int x, int y, int z, int w) : base(x, y, z)
        {
            W = w;
        }

        public int W { get; set; }

        public override bool Equals(object obj)
        {
            var p = (Point4D)obj;
            return p.X == X && p.Y == Y && p.Z == Z && p.W == W;
        }

        public override string ToString()
        {
            return $"{base.ToString()} W:{W}";
        }
    }
}
