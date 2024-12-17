var lines = File.ReadAllLines("TextFile1.txt");
var regA = long.Parse(lines[0].Split(' ').Last());
var regB = long.Parse(lines[1].Split(' ').Last());
var regC = long.Parse(lines[2].Split(' ').Last());
var output = new List<int>();
var instructions = new[] { adv, bxl, bst, jnz, bxc, @out, bdv, cdv, };
var program = lines[4].Split(' ').Last().Split(',').Select(int.Parse).ToArray();

var instructionPointer = 0;

Console.WriteLine(Go(regA));
Console.WriteLine(Second());

long Second()
{
    var programStr = string.Join(',', program);
    var queue = new Queue<long>();
    queue.Enqueue(0);
    while (queue.Any())
    {
        var next = queue.Dequeue();
        for (var i = 0; i < 8; i++)
        {
            var p = next * 8 + i;
            var s = Go(p);
            if (programStr == s)
                return p;

            if (programStr.EndsWith(s))
                queue.Enqueue(p);
        }
    }
    throw new Exception();
}

string  Go(long a)
{
    regA = a;
    regB = 0;
    regC = 0;
    output.Clear();
    instructionPointer = 0;
    while (instructionPointer < program.Length)
    {
        var instructionIndex = program[instructionPointer];
        instructions[instructionIndex](program[instructionPointer + 1]);
        instructionPointer += 2;
    }

    var res = string.Join(',', output);
    return res;
}

long Combo(int operand) => new[] { 0, 1, 2, 3, regA, regB, regC }[operand];
long DivRegA(int operand) => regA / (long)Math.Pow(2, Combo(operand));

// 0
void adv(int operand)
{
    regA = DivRegA(operand);
}

// 1
void bxl(int operand)
{
    regB ^= operand;
}

// 2
void bst(int operand)
{
    regB = Combo(operand) % 8;
}

// 3
void jnz(int operand)
{
    if (regA != 0)
        instructionPointer = operand - 2;
}

// 4
void bxc(int _)
{
    regB ^= regC;
}

// 5
void @out(int operand)
{
    output.Add((int)(Combo(operand) % 8));
}

// 6
void bdv(int operand)
{
    regB = DivRegA(operand);
}

// 7
void cdv(int operand)
{
    regC = DivRegA(operand);
}