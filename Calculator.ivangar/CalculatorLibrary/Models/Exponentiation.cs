namespace CalculatorLibrary.Models
{
    public record Exponentiation : MathOperation
    {
        public double Base { get; set; }
        public double Exponent { get; set; }

        public override string ToString()
        {
            return $"{Base} ^ {Exponent} = {Result}";
        }
    }
}
