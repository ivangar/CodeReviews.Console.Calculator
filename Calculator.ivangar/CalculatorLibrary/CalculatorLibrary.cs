using CalculatorLibrary.Enums;
using CalculatorLibrary.Logging;
using CalculatorLibrary.Models;

namespace CalculatorLibrary
{
    public class CalculatorEngine
    {
        private readonly CalculatorLogWriter _log;
        private readonly List<MathOperation> _operations = new();
        private int _operationCount = 0;
        private bool SuccessfullOperation = true;

        public CalculatorEngine()
        {
            _log = new CalculatorLogWriter();
        }

        public double DoOperation(double num1, string op, double num2 = 0)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            char operation = default;

            // Use a switch statement to do the math.
            switch (op)
            {
                case "a":
                    operation = '+';
                    result = num1 + num2;
                    AddBasicOperation(num1, num2, operation, result);
                    break;
                case "s":
                    operation = '-';
                    result = num1 - num2;
                    AddBasicOperation(num1, num2, operation, result);
                    break;
                case "m":
                    operation = '*';
                    result = num1 * num2;
                    AddBasicOperation(num1, num2, operation, result);
                    break;
                case "d":
                    operation = '/';
                    if (TryDivide(num1, num2, out result))
                    {
                        AddBasicOperation(num1, num2, operation, result);
                    }
                    else
                        SuccessfullOperation = false;
                    break;
                case "r":
                    operation = '√';
                    if (TryCalculateSquareRoot(num1, out result))
                    {
                        AddSquareRootOperation(num1, operation, result);
                    }
                    else
                        SuccessfullOperation = false;
                    break;
                case "p":
                    operation = '^';
                    if (TryCalculateExponent(num1, num2, out result))
                    {
                        AddPowerOperation(num1, num2, operation, result);
                    }
                    else
                        SuccessfullOperation = false;
                    break;
                default:
                    Console.WriteLine("This operation does not exist");
                    SuccessfullOperation = false;
                    break;
            }

            if (SuccessfullOperation)
            {
                _operationCount++;
                if (operation == '√')
                    _log.WriteOperation(num1, operation, result);
                else
                    _log.WriteOperation(num1, num2, operation, result);
            }

            return result;
        }

        public double DoTrigonometryOperation(double degrees, TrigonometryFunctions function, string op)
        {
            double result = double.NaN;
            char operation = '°';

            if (TryCalculateTrygonometry(degrees, function, out result))
            {
                _log.WriteOperation(degrees, operation, result);
                AddTrigOperation(degrees, function, operation, result);
            }

            return result;
        }

        public void Finish()
        {
            _log.Dispose();
            PrintAllOperations();
        }

        public void CountOperations()
        {
            var countTimes = _operationCount == 1 ? "time" : "times";
            Console.WriteLine($"\nThe calculator was used {_operations.Count} {countTimes}.\n");
        }

        public void PrintAllOperations()
        {
            CountOperations();

            if (_operations.Count != 0)
            {
                Console.WriteLine("Latest calculations:\n");

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
            _log.DeleteOperations();
        }

        private void AddBasicOperation(double num1, double num2, char op, double result)
        {
            var operation = new MathOperation
            {
                OperandA = num1,
                OperandB = num2,
                Operation = op,
                Result = result
            };

            _operations.Add(operation);
            Console.WriteLine($"Operation: {operation}\n");
        }

        private void AddSquareRootOperation(double radicand, char op, double result)
        {
            var operation = new SquareRoot
            {
                Radicand = radicand,
                Operation = op,
                Result = result
            };

            _operations.Add(operation);
            Console.WriteLine($"Operation: {operation}\n");
        }

        private void AddPowerOperation(double baseNumber, double exponent, char op, double result)
        {
            var operation = new Exponentiation
            {
                Base = baseNumber,
                Exponent = exponent,
                Operation = op,
                Result = result
            };

            _operations.Add(operation);
            Console.WriteLine($"Operation: {operation}\n");
        }

        private void AddTrigOperation(double degrees, TrigonometryFunctions function, char op, double result)
        {
            var operation = new TrigonometryOperation
            {
                Degrees = degrees,
                Function = function,
                Operation = op,
                Result = result
            };

            _operations.Add(operation);
            Console.WriteLine($"Operation: {operation}\n");
        }

        private bool TryDivide(double num1, double num2, out double result)
        {
            if (num2 == 0)
            {
                Console.WriteLine("Cannot divide by zero.");
                result = default;
                return false;
            }
            else
            {
                result = num1 / num2;
                return true;
            }
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
    }
}
