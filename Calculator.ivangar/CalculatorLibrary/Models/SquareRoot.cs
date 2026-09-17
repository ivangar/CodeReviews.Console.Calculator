namespace CalculatorLibrary.Models
{
    public record SquareRoot : MathOperation
    {
        public double Radicand { get; set; }

        public override string ToString()
        {
            return $"{Operation} {Radicand} = {Result}";
        }
    }
}
