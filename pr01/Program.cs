var lines = File.ReadAllLines("TextFile1.txt");

var left = new List<int>();
var right = new List<int>();

foreach (var line in lines)
{
    var splits = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    left.Add(int.Parse(splits[0]));
    right.Add(int.Parse(splits[1]));
}
left.Sort();
right.Sort();

Console.WriteLine(First(left, right));
Console.WriteLine(Second(left, right));

int Second(List<int> left, List<int> right) => left.Sum(x => x * right.Count(y => y == x));

int First(List<int> left, List<int> right) => left.Zip(right).Sum(x => Math.Abs(x.First - x.Second));
