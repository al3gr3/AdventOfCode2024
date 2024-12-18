﻿var directions = new[]
{
    new Point { Y = -1, X = 0 },
    new Point { Y = 0, X = 1 },
    new Point { Y = 1, X = 0 },
    new Point { Y = 0, X = -1 },
};

var lines = File.ReadAllLines("TextFile1.txt");

var pos = FindStart();

First(pos);

Second(pos);

void Second(Point pos)
{
    var second = 0;
    for (int x = 0; x < lines[0].Length; x++)
        for (int y = 0; y < lines.Length; y++)
            if (lines[y][x] == '.')
            {
                lines[y] = lines[y][..x] + '#' + lines[y][(x + 1)..];
                if (IsWithCycle(pos))
                    second++;
                lines[y] = lines[y][..x] + '.' + lines[y][(x + 1)..];
            }

    Console.WriteLine(second);
}

bool InBounds(Point p) => 0 <= p.X && p.X < lines[0].Length && 0 <= p.Y && p.Y < lines.Length;

Point FindStart()
{
    for (int x = 0; x < lines[0].Length; x++)
        for (int y = 0; y < lines.Length; y++)
            if (lines[y][x] == '^')
                return new Point { X = x, Y = y };

    throw new Exception();
}

void First(Point pos)
{
    var visited = Enumerable.Range(0, lines.Length).Select(y => Enumerable.Range(0, lines[0].Length).Select(x => -1).ToArray()).ToArray();
    Walk(pos, visited);

    var first = visited.Sum(l => l.Count(c => c > -1));
    Console.WriteLine(first);
}

bool IsWithCycle(Point pos)
{
    var visited = Enumerable.Range(0, lines.Length).Select(y => Enumerable.Range(0, lines[0].Length).Select(x => -1).ToArray()).ToArray();
    return Walk(pos, visited);
}

bool Walk(Point pos, int[][] visited)
{
    var dirIndex = 0;
    while (true)
    {
        if (visited[pos.Y][pos.X] == -1)
            visited[pos.Y][pos.X] = dirIndex;
        var newPos = pos.AddClone(directions[dirIndex]);
        if (InBounds(newPos))
        {
            if (visited[newPos.Y][newPos.X] == dirIndex)
                return true;
            if (lines[newPos.Y][newPos.X] == '#')
                dirIndex = (dirIndex + 1) % 4;
            else
                pos = newPos;
        }
        else
            break;
    }

    return false;
}

class Point
{
    internal int X;
    internal int Y;

    internal Point Add(Point point)
    {
        this.X += point.X;
        this.Y += point.Y;
        return this;
    }

    internal Point AddClone(Point point) => this.Clone().Add(point);

    internal Point Clone() => new Point { X = this.X, Y = this.Y };

    internal Point Multiply(int i)
    {
        this.X *= i;
        this.Y *= i;
        return this;
    }

    internal Point MultiplyClone(int i) => this.Clone().Multiply(i);
}