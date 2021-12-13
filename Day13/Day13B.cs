﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day13
{
    public class Day13B : IDay
    {
        private class Coords
        {
            public int x;
            public int y;

            public Coords(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public Coords(Coords other)
            {
                x = other.x;
                y = other.y;
            }

            public override int GetHashCode() => (x.GetHashCode() << 2) ^ y.GetHashCode();
            public override bool Equals(object? obj) =>
                (obj != null) &&
                (x == ((Coords)obj).x) &&
                (y == ((Coords)obj).y);
        }

        public void Run()
        {
            // The transparent paper is marked with random dots and includes instructions on how to fold it up (your puzzle input).
            string[] input = File.ReadAllText(@"..\..\..\Day13\Day13.txt").Split("\r\n\r\n");

            // The first section is a list of dots on the transparent paper.
            List<Coords> listOfDots = input[0].Split("\r\n")
                .Select(line => line.Split(",").Select(int.Parse).ToList())
                .Select(coords => new Coords(coords[0], coords[1]))
                .ToList();

            // Then, there is a list of fold instructions. Each instruction indicates a line on the transparent paper and wants you
            // to fold the paper up (for horizontal y=... lines) or left (for vertical x=... lines).
            List<Tuple<Func<Coords, int, Coords>, int>> foldInstructions = input[1].Split("\r\n")
                .Select(line => line.Split("="))
                .Select(line => new Tuple<Func<Coords, int, Coords>, int>(FoldMethods[line[0]], int.Parse(line[1])))
                .ToList();

            // Finish folding the transparent paper according to the instructions.
            // The manual says the code is always eight capital letters.
            foldInstructions.ForEach(instruction =>
                listOfDots = listOfDots.Select(coords => instruction.Item1(coords, instruction.Item2))
                    .Distinct()
                    .ToList());

            // What code do you use to activate the infrared thermal imaging camera system?
            Console.WriteLine("Solution:");
            foreach (int y in Enumerable.Range(0, listOfDots.Select(elem => elem.y).Max() + 1))
            {
                foreach (int x in Enumerable.Range(0, listOfDots.Select(elem => elem.x).Max() + 1))
                {
                    Console.Write(listOfDots.Contains(new Coords(x, y)) ? "#" : " ");
                }
                Console.WriteLine();
            }
        }

        private static Coords FoldAlongX(Coords coords, int x)
        {
            Coords folded = new(coords);
            if (folded.x > x)
            {
                folded.x -= 2 * (folded.x - x);
            }
            return folded;
        }

        private static Coords FoldAlongY(Coords coords, int y)
        {
            Coords folded = new(coords);
            if (folded.y > y)
            {
                folded.y -= 2 * (folded.y - y);
            }
            return folded;
        }

        private static readonly Dictionary<string, Func<Coords, int, Coords>> FoldMethods = new()
        {
            { "fold along x", FoldAlongX },
            { "fold along y", FoldAlongY }
        };
    }
}