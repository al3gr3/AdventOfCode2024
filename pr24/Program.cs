var lines = File.ReadAllLines("TextFile1.txt");
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
        // x00: 1
        var splits = line.Split(new[] { ": "}, StringSplitOptions.RemoveEmptyEntries);
        dict[splits[0]] = int.Parse(splits[1]);
    }
    else
    {
        // ntg XOR fgs -> mjb
        var splits = line.Split(new[] { " ", "->" }, StringSplitOptions.RemoveEmptyEntries);
        gates.Add(new Gate
        {
            Wires = new[] { splits[0], splits[2] },
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

while (!dict.Keys.Where(k => k.StartsWith('z')).All(k => dict[k].HasValue))
{
    foreach (var gate in gates)
    {
        if (dict[gate.Wires[0]].HasValue && dict[gate.Wires[1]].HasValue)
            dict[gate.Output] = gate.Do(dict[gate.Wires[0]].Value, dict[gate.Wires[1]].Value);
    }
}

var result = dict.Keys.Where(k => k.StartsWith('z')).Order().Reverse().Aggregate(0L, (s, n) => s * 2 + dict[n].Value);

Console.WriteLine(result);// 1482885350 is too low
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