using CalculatorLibrary.Enums;

namespace CalculatorLibrary.Models
{
    public record TrigonometryOperation : MathOperation
    {
        public double Degrees { get; set; }

        // Adding Radians property in case we need to convert later.
        public double Radians
        {
            get { return Degrees * (Math.PI / 180); }
        }

        public TrigonometryFunctions Function { get; set; }

        public override string ToString() => $"{Function}({Degrees}{Operation}) = {Result}";
    }
}
