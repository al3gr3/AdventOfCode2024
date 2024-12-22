var initials = File.ReadAllLines("TextFile1.txt").Select(x => long.Parse(x)).ToArray();

Console.WriteLine(initials.Sum(Calculate2000));

var total = new Dictionary<string, long>();
foreach (var initial in initials)
    FillDict(initial);
Console.WriteLine(total.Values.Max());

long Calculate2000(long secret) => Enumerable.Range(0, 2000).Aggregate(secret, (s, _) => Calculate(s));

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

void FillDict(long secret)
{
    var lastDigit = secret % 10;
    var diffs = new List<long>();

    var dict = new Dictionary<string, long>();
    for (var i = 0; i < 2000; i++)
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
            {
                dict.Add(key, nextLastDigit);
                if (total.ContainsKey(key))
                    total[key] += nextLastDigit;
                else
                    total[key] = nextLastDigit;
            }
        }
        
        lastDigit = nextLastDigit;
    }
}