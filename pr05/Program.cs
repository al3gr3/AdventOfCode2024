var lines = File.ReadAllLines("TextFile1.txt");
var index = Array.IndexOf(lines, "");
var rules = lines.Take(index).Select(x => x.Split("|")).ToArray();
var patches = lines.Skip(index + 1).Select(x => x.Split(',')).ToArray();

var first = patches.Where(IsCorrect).Sum(Middle);
Console.WriteLine(first);

var second = patches.Where(p => !IsCorrect(p)).Select(Sort).Sum(Middle);
Console.WriteLine(second);

int Compare(string? x, string? y) => rules.Any(rule => x == rule[0] && y == rule[1]) ? 1 : -1;

string[] Sort(string[] sheets)
{
    Array.Sort(sheets, Compare);
    return sheets;
}

int Middle(string[] sheets) => int.Parse(sheets[(int)(sheets.Length / 2)]);

bool IsCorrect(string[] sheets) => rules.All(rule =>
{
    var index1 = Array.IndexOf(sheets, rule[0]);
    var index2 = Array.IndexOf(sheets, rule[1]);

    var isOkRule = index1 == -1 || index2 == -1 || index1 < index2;
    return isOkRule;
});
