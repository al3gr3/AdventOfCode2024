using System.Text.RegularExpressions;

var line = File.ReadAllText("TextFile1.txt");

Console.WriteLine(First(line));
Console.WriteLine(Second(line));

int First(string line) => new Regex(@"mul\((\d+),(\d+)\)").Matches(line).Cast<Match>().Sum(ParseMulRegex);

int ParseMulRegex(Match match) => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);

int Second(string line)
{
    var regex = new Regex(@"mul\((\d+),(\d+)\)|do\(\)|don't\(\)");
    var result = 0;
    var isEnabled = true;
    foreach (Match match in regex.Matches(line))
    {
        if (match.Value == @"do()")
            isEnabled = true;
        else if (match.Value == @"don't()")
            isEnabled = false;
        else if (isEnabled)
            result += ParseMulRegex(match);
    }
    return result;
}