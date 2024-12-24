var lines = File.ReadAllLines("TextFile1.txt");

static string Format2Digits(int i)
{
    var iStr = i.ToString();
    if (i < 10)
        iStr = "0" + iStr;
    return iStr;
}

var isState = true;
var dict = new Dictionary<string, int?>();
var gates = new List<Gate>();
foreach (var line in lines)
{
    if (string.IsNullOrEmpty(line))
    {
        isState = false;
        continue;
    }
    if (isState)
    {
        var splits = line.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
        dict[splits[0]] = int.Parse(splits[1]);
    }
    else
    {
        var splits = line.Split(new[] { " ", "->" }, StringSplitOptions.RemoveEmptyEntries);
        gates.Add(new Gate
        {
            Wires = new[] { splits[0], splits[2] }.Order().ToArray(),
            Operation = splits[1],
            Output = splits[3],
        });

        new[] { splits[0], splits[2], splits[3] }.ToList().ForEach(x =>
        {
            if (!dict.ContainsKey(x))
                dict[x] = null;
        });
    }
}

bool IsOkZ(int depth, string reg)
{
    var iStr = Format2Digits(depth);

    var zXor = gates.FirstOrDefault(x => x.Operation == "XOR" && x.Output == reg);
    if (zXor == null)
    {
        Console.WriteLine($"{iStr}: zXor is null");
        return false;
    }

    var xyXor = gates.FirstOrDefault(x => x.Operation == "XOR" && x.Wires[0] == $"x{iStr}" && x.Wires[1] == $"y{iStr}");
    if (xyXor == null)
    {
        Console.WriteLine($"{iStr}: xyXor is null");
        return false;
    }

    var remaining = zXor.Wires.Except(new[] { xyXor.Output }).ToArray();

    if (remaining.Length != 1)
    {
        Console.WriteLine("{iStr}: xyXor output is not in zXor.Wires");
        return false;
    }
    var xorXor = gates.FirstOrDefault(x => x.Operation == "XOR" && zXor.Wires.Contains(x.Output));

}
First();
Test("z05");

for (var i = 2; i < 40; i++)
{
    var iStr = Format2Digits(i);
    var result = IsOkZ(i, $"z{iStr}");

    
    
    if (ZXor == null)
        Console.WriteLine($"{iStr}: Zxor is null");
    var xyXor = gates.FirstOrDefault(x => x.Operation == "XOR" && x.Wires[0] == $"x{iStr}" && x.Wires[1] == $"y{iStr}");
    if (xyXor == null)
        Console.WriteLine($"{iStr}: xyXor is null");
}
static string Repeat(int amount, char c) => string.Concat(Enumerable.Range(0, amount).Select(x => c));

Console.WriteLine("");

void First()
{
    while (!dict.Keys.Where(k => k.StartsWith('z')).All(k => dict[k].HasValue))
    {
        foreach (var gate in gates)
        {
            if (dict[gate.Wires[0]].HasValue && dict[gate.Wires[1]].HasValue)
                dict[gate.Output] = gate.Do(dict[gate.Wires[0]].Value, dict[gate.Wires[1]].Value);
        }
    }
    var result = Count('z');
    Console.WriteLine(result);
}

long Count(char c) => dict.Keys.Where(k => k.StartsWith(c)).Order().Reverse().Aggregate(0L, (s, n) => s * 2 + dict[n].Value);

Dictionary<string, int> FindZs()
{
    var correct = Count('x') + Count('y');
    var i = 0;
    var correctZ = new Dictionary<string, int>();
    while (correct > 0)
    {
        var iStr = Format2Digits(i);
        correctZ[$"z{iStr}"] = (int)(correct % 2);
        correct /= 2;
        i++;
    }

    return correctZ;
}

void Test(string s)
{
    var wave = new[] { s }.ToList();
    while (wave.Any())
    {
        var nextWave = new List<string>();
        var s1 = "";

        foreach (var next in wave)
        {
            var gs = gates.Where(g => g.Output == next).ToList();

            foreach (var g in gs)
            {
                s1 += $"{g.Output}={g.Operation}({g.Wires[0]},{g.Wires[1]}) ";

                nextWave.AddRange(g.Wires);
            }
        }

        Console.WriteLine(s1);
        wave = nextWave;
    }
}

class Gate
{
    internal string[] Wires;
    internal string Output;
    internal string Operation;

    internal int Do(int a, int b)
    {
        if (Operation == "AND")
            return a & b;
        else if (Operation == "OR")
            return a | b;
        else if (Operation == "XOR")
            return a ^ b;
        throw new Exception();
    }
}