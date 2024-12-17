var lines = File.ReadAllLines("TextFile1.txt");
var regA = int.Parse(lines[0].Split(' ').Last());
var regB = int.Parse(lines[1].Split(' ').Last());
var regC = int.Parse(lines[2].Split(' ').Last());
var instructionPointer = 0;
List<string> output = new List<string>();

var instructions = new Action<int>[]
{
    adv, bxl, bst, jnz, bxc, @out, bdv, cdv,
};

var program = lines.Last().Split(' ').Last().Split(',').Select(int.Parse).ToArray();

while (instructionPointer < program.Length)
{
    var instructionIndex = program[instructionPointer];
    instructions[instructionIndex](program[instructionPointer + 1]);
    if (!(instructionIndex == 3 && regA != 0))
        instructionPointer += 2;
}

Console.WriteLine(string.Join(',', output));
foreach (var x in new[] { regA, regB, regC })
{ 
    Console.WriteLine(x); 
}   

void adv(int operand)
{
    regA /= (int)Math.Pow(2, Combo(operand)); 
}

void bxl(int operand)
{
    regB ^= operand;
}

void bst(int operand)
{
    var val = Combo(operand);
    regB = val % 8;
}

int Combo(int operand) => operand < 4 ? operand : new[] { regA, regB, regC }[operand - 4];

void jnz(int operand)
{
    if (regA == 0)
        return;

    instructionPointer = operand;
}

void bxc(int _)
{
    regB ^= regC;
}

void @out(int operand)
{
    output.Add((Combo(operand) % 8).ToString());
}

void bdv(int operand)
{
    regB = regA / (int)Math.Pow(2, Combo(operand));
}

void cdv(int operand)
{
    regC = regA / (int)Math.Pow(2, Combo(operand));
}