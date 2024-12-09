var line = File.ReadAllText("TextFile1.txt");

var id = 0;
var mem = new List<long?>();
var files = new List<MemFile>();
for (int i = 0; i < line.Length; i++)
{
    var len = int.Parse("" + line[i]);
    if (i % 2 == 1)
        mem.AddRange(Enumerable.Range(0, len).Select(x => (long?)null));
    else
    {
        files.Add(new MemFile { Id = id, Length = len, Index = mem.Count() });
        mem.AddRange(Enumerable.Range(0, len).Select(x => (long?)id));
        id++;
    }
}

Second();

Console.WriteLine(Checksum());
long? Checksum() => mem.Select((x, i) => x * i).Sum();

void First()
{
    var right = mem.Count() - 1;
    var left = 0;

    while (true)
    {
        while (!mem[right].HasValue)
            right--;

        while (mem[left].HasValue)
            left++;

        if (left > right)
            break;

        mem[left] = mem[right];
        mem[right] = null;
    }
}

void Second()
{
    files.Reverse();
    foreach (var file in files)
    {
        for (var i = 0; i < file.Index; i++)
        {
            if (!mem[i].HasValue && mem.Skip(i).Take(file.Length).All(x => !x.HasValue))
            {
                for (var j = 0; j < file.Length; j++)
                {
                    mem[i + j] = file.Id;
                    mem[file.Index + j] = null;
                }
                break;
            }
        }
    }
}

class MemFile
{
    internal int Index;
    internal int Length;
    internal int Id;
}