using CalculatorLibrary.Models;
using Newtonsoft.Json;

namespace CalculatorLibrary
{
    public class CalculatorEngine
    {
        JsonWriter writer;

        private readonly List<MathOperation> _operations = new();

        public CalculatorEngine()
        {
            StreamWriter logFile = File.CreateText("calculator.json");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();
        }

        public double DoOperation(double num1, string op, double num2 = 0)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            char operation = default;
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");

            // Use a switch statement to do the math.
            switch (op)
            {
                case "a":
                    operation = '+';
                    result = num1 + num2;
                    writer.WriteValue(operation);
                    AddBasicOperation(num1, num2, operation, result);
                    break;
                case "s":
                    operation = '-';
                    result = num1 - num2;
                    writer.WriteValue(operation);
                    AddBasicOperation(num1, num2, operation, result);
                    break;
                case "m":
                    operation = '*';
                    result = num1 * num2;
                    writer.WriteValue(operation);
                    AddBasicOperation(num1, num2, operation, result);
                    break;
                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (num2 != 0)
                    {
                        operation = '/';
                        result = num1 / num2;
                        writer.WriteValue(operation);
                        AddBasicOperation(num1, num2, operation, result);
                    }
                    break;
                case "r":
                    operation = '√';
                    if (TryCalculateSquareRoot(num1, out result))
                    {
                        writer.WriteValue(operation);
                        AddSquareRootOperation(num1, operation, result);
                    }
                    break;
                case "p":
                    operation = '^';
                    if (TryCalculateExponent(num1, num2, out result))
                    {
                        writer.WriteValue(operation);
                        AddPowerOperation(num1, num2, operation, result);
                    }
                    break;
                // Return text for an incorrect option entry.
                default:
                    break;
            }

            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();

            return result;
        }

        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
            CountOperations();
            PrintAllOperations();
        }

        public void CountOperations()
        {
            var countTimes = _operations.Count == 1 ? "time" : "times";
            Console.WriteLine($"The calculator was used {_operations.Count} {countTimes}.\n");
        }

        public void PrintAllOperations()
        {
            Console.WriteLine("\nLatest Calculations:\n");

            foreach (var (index, operation) in _operations.Select((o, i) => (i, o)))
            {
                Console.WriteLine($"{index + 1}. {operation}");
            }

            Console.WriteLine("\n\n");
        }

        public void DeleteOperations()
        {
            _operations.Clear();
        }

        private void AddBasicOperation(double num1, double num2, char op, double result)
        {
            _operations.Add(new MathOperation
            {
                OperandA = num1,
                OperandB = num2,
                Operation = op,
                Result = result
            });
        }

        private void AddSquareRootOperation(double radicand, char op, double result)
        {
            _operations.Add(new SquareRoot
            {
                Radicand = radicand,
                Operation = op,
                Result = result
            });
        }

        private void AddPowerOperation(double baseNumber, double exponent, char op, double result)
        {
            _operations.Add(new Exponentiation
            {
                Base = baseNumber,
                Exponent = exponent,
                Operation = op,
                Result = result
            });
        }

        private bool TryCalculateSquareRoot(double radicand, out double result)
        {
            if (radicand < 0)
            {
                Console.WriteLine("Cannot compute square root of a negative number.");
                result = default;
                return false;
            }
            else
            {
                result = Math.Round(Math.Sqrt(radicand), 2, MidpointRounding.AwayFromZero);
                return true;
            }

        }

        private bool TryCalculateExponent(double baseNumber, double exponent, out double result)
        {
            if (exponent > 10)
            {
                Console.WriteLine("The exponent is very big, try a smaller number");
                result = default;
                return false;
            }

            else
            {
                result = Math.Pow(baseNumber, exponent);
                return true;
            }

        }
    }
}
