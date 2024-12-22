var initials = File.ReadAllLines("TextFile1.txt").Select(x => long.Parse(x)).ToArray();

Console.WriteLine(initials.Sum(Calculate2000));

var dicts = initials.Select(PrepareDict).ToArray();

long result = 0;
for (var i1 = -9; i1 < 10; i1++)
    for (var i2 = -9; i2 < 10; i2++)
        for (var i3 = -9; i3 < 10; i3++)
            for (var i4 = -9; i4 < 10; i4++)
            {
                var key = $"{i1}{i2}{i3}{i4}";
                var bananas = dicts.Sum(d => d.ContainsKey(key) ? d[key] : 0);
                result = Math.Max(bananas, result);
            }

Console.WriteLine(result);

long Calculate2000(long secret)
{
    for (int i = 0; i < 2000; i++)
        secret = Calculate(secret);
    return secret;
}

static long Calculate(long secret)
{
    secret ^= 64 * secret;
    secret %= 16777216;

    secret ^= secret / 32;
    secret %= 16777216;

    secret ^= 2048 * secret;
    secret %= 16777216;
    return secret;
}

Dictionary<string, long> PrepareDict(long secret)
{
    var lastDigit = secret % 10;
    var diffs = new List<long>();

    var dict = new Dictionary<string, long>();
    for (int i = 0; i < 2000; i++)
    {
        secret = Calculate(secret);
        
        var nextLastDigit = secret % 10;
        
        if (diffs.Count == 4)
            diffs.RemoveAt(0);
        
        diffs.Add(nextLastDigit - lastDigit);

        if (diffs.Count == 4)
        {
            var key = string.Concat(diffs.Select(x => x.ToString()));
            if (!dict.ContainsKey(key))
                dict.Add(key, nextLastDigit);
        }
        
        lastDigit = nextLastDigit;
    }
    return dict;
}