var lines = File.ReadAllLines("TextFile1.txt");
var result = Second(lines);
Console.WriteLine(result);

int Second(string[] lines)
{
    var numbers = new[]
    {
        "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
    }.Union("1234567890".Select(c => "" + c)).ToArray();

    var result = lines
        .Select(line =>
        {
            var first = numbers.Select(x => new { Index = line.IndexOf(x), Number = x })
                .Where(x => x.Index > -1)
                .OrderBy(x => x.Index).Select(x => x.Number).First();
            var last = numbers.Select(x => new { Index = line.LastIndexOf(x), Number = x })
                .Where(x => x.Index > -1)
                .OrderBy(x => x.Index).Select(x => x.Number).Last();
            if (first.Length > 1)
                first = (Array.IndexOf(numbers, first) + 1).ToString();
            if (last.Length > 1)
                last = (Array.IndexOf(numbers, last) + 1).ToString();
            return first + last;
        })
        .Select(x => int.Parse(x))
        .Sum();
    return result;
}

int First(string[] lines)
{
    var digits = "1234567890";
    var result = lines
        .Select(x => "" + x.First(c => digits.Contains(c)) + x.Last(c => digits.Contains(c)))
        .Select(x => int.Parse(x))
        .Sum();
    return result;
}
