using System.Text.RegularExpressions;

var line = File.ReadAllText("TextFile1.txt");

Console.WriteLine(First(line));
Console.WriteLine(Second(line));

int First(string line)
{
    var regex = new Regex(@"mul\(\d+,\d+\)");
    var result = 0;
    foreach (Match match in regex.Matches(line))
        result += ParseMul(match.Value);
    return result;
}

int ParseMul(string value)
{
    var splits = value.Split(new[] { "mul(", ",", ")" }, StringSplitOptions.RemoveEmptyEntries);
    return int.Parse(splits[0]) * int.Parse(splits[1]);
}

int Second(string line)
{
    var regex = new Regex(@"mul\(\d+,\d+\)|do\(\)|don't\(\)");
    var result = 0;
    var isOn = true;
    foreach (Match match in regex.Matches(line))
    {
        if (match.Value == @"do()")
            isOn = true;
        else if (match.Value == @"don't()")
            isOn = false;
        else if (isOn)
            result += ParseMul(match.Value);
    }
        
    return result;
}