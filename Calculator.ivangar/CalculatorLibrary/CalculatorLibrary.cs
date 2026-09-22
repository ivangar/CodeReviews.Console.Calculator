using CalculatorLibrary.Enums;
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
            LogOperationObject(num1, num2);

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

            LogResult(result);
            return result;
        }

        public double DoTrigonometryOperation(double degrees, TrigonometryFunctions function, string op)
        {
            double result = double.NaN;
            char operation = '°';

            if (TryCalculateTrygonometry(degrees, function, out result))
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Operand1");
                writer.WriteValue(degrees);
                writer.WritePropertyName("Operation");
                writer.WriteValue(operation);
                LogResult(result);
                AddTrigOperation(degrees, function, operation, result);
            }

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
            if (_operations.Count != 0)
            {
                Console.WriteLine("\nLatest calculations:\n");

                foreach (var (index, operation) in _operations.Select((o, i) => (i, o)))
                {
                    Console.WriteLine($"{index + 1}. {operation}");
                }
            }

            else
                Console.WriteLine("\nThere are no calculations to display:\n");
        }

        public void DeleteOperations()
        {
            Console.WriteLine("\nDeleting all current operations:\n");
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

        private void AddTrigOperation(double degrees, TrigonometryFunctions function, char op, double result)
        {
            _operations.Add(new TrigonometryOperation
            {
                Degrees = degrees,
                Function = function,
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

        private bool TryCalculateTrygonometry(double degrees, TrigonometryFunctions function, out double result)
        {
            if (degrees > 360 || degrees < -360)
            {
                Console.WriteLine("The degree value is outside of this scope, try a smaller number");
                result = default;
                return false;
            }

            else
            {
                var radians = degrees * (Math.PI / 180);

                result = function switch
                {
                    TrigonometryFunctions.Sin => Math.Round(Math.Sin(radians), 4),
                    TrigonometryFunctions.Cos => Math.Round(Math.Cos(radians), 4),
                    TrigonometryFunctions.Tan => Math.Round(Math.Tan(radians), 4),
                    _ => throw new ArgumentOutOfRangeException(nameof(function))
                };

                return true;
            }
        }

        private void LogOperationObject(double num1, double num2)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");
        }

        private void LogResult(double result)
        {
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();
        }
    }
}
