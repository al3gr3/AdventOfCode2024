﻿using System.Drawing;

var lines = File.ReadAllLines("TextFile1.txt");
var numeric = new[]
{
    "789",
    "456",
    "123",
    " 0A"
};
var directional = new[]
{
    " ^A",
    "<v>",
};
var result = 0;

foreach (var line in lines)
{
    //Console.WriteLine(line);
    var s1 = Dial(line, numeric);
    //Console.WriteLine(s1);
    var s2 = Dial(s1, directional);
    //Console.WriteLine(s2);
    var s3 = Dial(s2, directional);
    Console.WriteLine($"{line}: {s3}");
    //Console.WriteLine($"{s3.Length} * {int.Parse(line[..3])}");
    result += s3.Length * int.Parse(line[..3]);
}

Console.WriteLine(result);

string Dial(string line, string[] dial)
{
    var s = "";
    var pos = Find('A', dial);
    foreach(var c in line)
    {
        var nextPos = Find(c, dial);
        var diff = nextPos.AddClone(pos.MultiplyClone(-1));
        if (diff.Y < 0)
            s += Repeat(-1 * diff.Y, '^');
        if (diff.X < 0)
            s += Repeat(-1 * diff.X, '<');

        if (diff.X > 0)
            s += Repeat(diff.X, '>');
        if (diff.Y > 0)
            s += Repeat(diff.Y, 'v');

        s += "A";

        pos = nextPos;
    }
    return s;
}

string Repeat(int amount, char c) => string.Concat(Enumerable.Range(0, amount).Select(x => c));

static Point Find(char c, string[] ls)
{
    for (var i = 0; i < ls.Length; i++)
        if (ls[i].Contains(c))
            return new Point { X = ls[i].IndexOf(c), Y = i };
    throw new Exception();
}

public class Point
{
    internal int X;
    internal int Y;

    internal static Point[] OrtoDirections => new[]
    {
        new Point { Y = -1, X = 0 },
        new Point { Y = 0, X = 1 },
        new Point { Y = 1, X = 0 },
        new Point { Y = 0, X = -1 },
    };

    internal static Point[] AllDirections => new[]
    {
        new Point { Y = 1, X = 0 },
        new Point { Y = 1, X = 1 },
        new Point { Y = 0, X = 1 },
        new Point { Y = -1, X = 1 },
        new Point { Y = -1, X = 0 },
        new Point { Y = -1, X = -1 },
        new Point { Y = 0, X = -1 },
        new Point { Y = 1, X = -1 },
    };

    internal Point Add(Point point)
    {
        this.X += point.X;
        this.Y += point.Y;
        return this;
    }

    internal bool IsEqual(Point p) => this.X == p.X && this.Y == p.Y;

    internal Point AddClone(Point point) => this.Clone().Add(point);

    internal Point Clone() => new Point { X = this.X, Y = this.Y };

    internal Point Multiply(int i)
    {
        this.X *= i;
        this.Y *= i;
        return this;
    }

    internal Point MultiplyClone(int i) => this.Clone().Multiply(i);

    internal int Distance(Point u)
    {
        var diff = this.AddClone(u.MultiplyClone(-1));
        return Math.Abs(diff.X) + Math.Abs(diff.Y);
    }
}