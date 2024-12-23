var lines = File.ReadAllLines("TextFile1.txt").Select(x => x.Split('-').Order().ToArray());
var dict = new Dictionary<string, List<string>>();

void Add(string key, string val)
{
    if (dict.ContainsKey(key))
        dict[key].Add(val);
    else
        dict[key] = new[] { val }.ToList();
}

foreach (var line in lines)
{
    Add(line[0], line[1]);
    Add(line[1], line[0]);
}
foreach (var key in dict.Keys)
    dict[key] = dict[key].Order().ToList();

var CBC = 0;
clique(dict.Keys.ToList(), 0);

void clique(List<string> V, int depth)
{
    if (V.Count == 0)
    {
        if (depth > CBC)
            CBC = depth;
        return;
    }

    var i = 0;
    while (i < V.Count)
    {
        if (depth + V.Count - i <= CBC)
            return;
        i++;
        var Nvi = dict[V[i - 1]];
        var nextV = Nvi.Where(vj =>
        {
            var j = V.IndexOf(vj);
            if (j > -1)
                j++;
            return j > (i) && j <= V.Count;
        }).ToList();
        var before = CBC;
        //  clique (N(vi) | ∀vj : j > i, j ≤ |V|, depth + 1)
        clique(nextV, depth + 1);
        if (CBC > before && depth == 0)
        {
            nextV.Add(V[i - 1]);
            Console.WriteLine($"{string.Join(',', nextV.OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase))} {CBC}");
        }
    }
}

/*
function Main
 CBC := 0 // the maximum clique’s size
 clique (V, 0)
 return
end function
function clique(V, depth)
 if’|V| = 0 then
 if depth > CBC then
 New record - save it. CBC := depth
 end if
 return
 end if
 i := 0
 while i < |V| do
 if depth + |V| - i ≤ CBC then return // prune
 i := i + 1
 // form a new depth. N(vi) denotes a neighbourhood of vi.
 clique (N(vi) | ∀vj : j > i, j ≤ |V|, depth + 1)
 end while
 return
end function
*/

void First()
{
    var threes = lines.SelectMany(two =>
    {
        var thirds = lines.Where(p => p[0] == two[0] && lines.Any(ll => ll[0] == p[1] && ll[1] == two[1]))
            .Select(x => x[1]);
        return thirds.Select(x => string.Concat(new[] { two[0], x, two[1] }.Order()));
    }).ToList();
    Console.WriteLine(threes.Count(x => x[0] == 't' || x[2] == 't' || x[4] == 't'));
}

void Test()
{
    Console.WriteLine(string.Join(',', new[] { "z", "v" }.OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase)));

    var cliq = "cl,ei,fd,hc,ib,kq,kv,ky,rv,vf,wk,yx,zf".Split(',');
    foreach (var s in cliq)
    {
        var inter = dict[s].Intersect(cliq).ToList();
        inter.Add(s);

        Console.WriteLine($"{s}: {string.Join(',', inter.OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase))}");
    }
}