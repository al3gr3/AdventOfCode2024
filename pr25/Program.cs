var lines = File.ReadAllLines("TextFile1.txt").ToList();
lines.Add("");
var keys = new List<List<int>>();
var locks = new List<List<int>>();

var current = new List<string>();
foreach (var line in lines)
    if (string.IsNullOrEmpty(line))
    {
        Add(current);
        current.Clear();
    }
    else
        current.Add(line);

void Add(List<string> current)
{
    var list = Enumerable.Range(0, 5).Select(i => current.Count(l => l[i] == '#') - 1).ToList();
    if (current.First().All(c => c == '#'))
        locks.Add(list);
    else
        keys.Add(list);
}

var result = 0;
foreach(var key in keys)
    foreach (var @lock in locks)
        if (key.Zip(@lock).All(x => x.First + x.Second <= 5))
            result++;

Console.WriteLine(result);