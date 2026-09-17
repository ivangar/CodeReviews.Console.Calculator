namespace CalculatorLibrary.Models
{
    public record MathOperation
    {
        public double OperandA { get; init; }
        public double OperandB { get; init; }
        public char Operation { get; init; }
        public double Result { get; init; }
        public override string ToString() => $"{OperandA} {Operation} {OperandB} = {Result}";
    }
}
