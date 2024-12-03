var lines = File.ReadAllLines("TextFile1.txt");

var arrays = lines.Select(line => line.Split(new[] { ' ' }).Select(x => int.Parse(x)));
