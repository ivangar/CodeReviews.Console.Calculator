using Calculator.ivangar.Enums;
using CalculatorLibrary;
using CalculatorLibrary.Enums;

namespace Calculator.ivangar
{
    public class Application
    {
        private bool EndApp = false;
        private readonly CalculatorEngine Calculator = new();

        private readonly Random _random = new();

        public void Run()
        {
            Menu.Intro();
            Menu.PrintMenu();

            string? optionInput = Console.ReadLine();
            MainMenuOptions menuOption;

            while (!EndApp)
            {
                while (!Menu.TryGetMenuOption(optionInput, out menuOption))
                {
                    Menu.PrintMenu(invalid: true);
                    optionInput = Console.ReadLine();
                }

                switch (menuOption)
                {
                    case MainMenuOptions.PerformOperation:
                        PerformCalculation();
                        break;
                    case MainMenuOptions.ViewOperations:
                        Calculator.PrintAllOperations();
                        break;
                    case MainMenuOptions.DeleteOperations:
                        Calculator.DeleteOperations();
                        break;
                    case MainMenuOptions.Exit:
                        EndApp = true;
                        Console.WriteLine("\nThank you for using the Calculator.");
                        break;
                }

                if (EndApp)
                    break;

                Menu.PrintMenu();
                optionInput = Console.ReadLine();
            }

            Calculator.Finish();
        }

        public void PerformCalculation()
        {
            // Ask the user to choose an operator.
            Menu.PrintOperations();
            string? op = Console.ReadLine();

            while (!Menu.ValidateOperationChoice(op))
            {
                Menu.PrintOperations(invalid: true);
                op = Console.ReadLine();
            }

            switch (op!.ToLower())
            {
                case "r":
                    SquareRootOperation(op);
                    break;
                case "p":
                    PowerOperation(op);
                    break;
                case "t":
                    TrigonometryOperation(op);
                    break;
                default:
                    BaseMathOperation(op);
                    break;
            }
        }

        public void BaseMathOperation(string operation)
        {
            double operand1 = Menu.GetOperand("Type a number, and then press Enter: ");
            double operand2 = Menu.GetOperand("Type another number, and then press Enter: ");

            try
            {
                var result = Calculator.DoOperation(operand1, operation, operand2);

                if (double.IsNaN(result))
                    Menu.PrintError("This operation will result in a mathematical error.\n");

                else Console.WriteLine("Your result: {0:0.##}\n", result);
            }
            catch (Exception e)
            {
                Menu.PrintError("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
            }
        }

        public void SquareRootOperation(string operation)
        {
            var radicand = Menu.GetOperand("Type the radicand, and then press Enter: ");

            try
            {
                var result = Calculator.DoOperation(radicand, operation);

                if (double.IsNaN(result))
                    Menu.PrintError("This operation will result in a mathematical error.\n");

                else Console.WriteLine("Your result: {0:0.##}\n", result);
            }
            catch (Exception e)
            {
                Menu.PrintError("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
            }
        }

        public void PowerOperation(string operation)
        {
            double baseOperand = Menu.GetOperand("Type the base, and then press Enter: ");
            double exponent = Menu.GetOperand("Type the exponent, and then press Enter: ");

            try
            {
                var result = Calculator.DoOperation(baseOperand, operation, exponent);

                if (double.IsNaN(result))
                    Menu.PrintError("This operation will result in a mathematical error.\n");

                else Console.WriteLine("Your result: {0:0.##}\n", result);
            }
            catch (Exception e)
            {
                Menu.PrintError("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
            }
        }

        public void TrigonometryOperation(string operation)
        {
            double degrees = Menu.GetOperand("Enter an angle in degrees°: ");
            var functionValues = Enum.GetValues<TrigonometryFunctions>();
            var randomFunction = _random.Next(functionValues.Length);
            var function = functionValues[randomFunction];

            try
            {
                var result = Calculator.DoTrigonometryOperation(degrees, function, operation);

                if (double.IsNaN(result))
                    Menu.PrintError("This operation will result in a mathematical error.\n");

                else Console.WriteLine("Your result: {0:0.##}\n", result);
            }
            catch (Exception e)
            {
                Menu.PrintError("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
            }
        }
    }
}
