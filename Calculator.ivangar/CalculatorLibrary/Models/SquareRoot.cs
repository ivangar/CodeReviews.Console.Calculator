namespace CalculatorLibrary.Models
{
    public record SquareRoot : MathOperation
    {
        public double Radicand { get; set; }

        public override string ToString() => $"{Operation} {Radicand} = {Result:0.##}";
    }
}
