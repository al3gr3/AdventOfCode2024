var lines = File.ReadAllLines("TextFile1.txt");

var arrays = lines.Select(line => line.Split(new[] { ' ' }).Select(x => int.Parse(x)));

Console.WriteLine(arrays.Count(x => IsSafe(x)));
Console.WriteLine(arrays.Count(x => IsSafeDampener(x)));

bool IsSafeDampener(IEnumerable<int> array) => array.Select((x, i) => array.Take(i).Concat(array.Skip(i + 1))).Any(x => IsSafe(x));

bool IsSafe(IEnumerable<int> array)
{
    var diffs = array.Reverse().Skip(1).Reverse().Zip(array.Skip(1)).Select(x => x.First - x.Second).ToList();

    var possibleDiffs = diffs.First() > 0 ? new[] { 1, 2, 3 } : new[] { -1, -2, -3 };
    return diffs.All(x => possibleDiffs.Contains(x));
}