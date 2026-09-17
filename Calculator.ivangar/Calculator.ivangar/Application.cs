using CalculatorLibrary;

namespace Calculator.ivangar
{
    public class Application
    {
        private bool EndApp = false;
        private readonly CalculatorEngine Calculator = new();

        public void Run()
        {
            Menu.Intro();

            while (!EndApp)
            {
                // Ask the user to choose an operator.
                Menu.PrintMenu();
                string? op = Console.ReadLine();

                while (!Menu.ValidateMainOptions(op))
                {
                    Menu.PrintMenu(invalid: true);
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
                    default:
                        BaseMathOperation(op);
                        break;
                }

                Console.WriteLine("------------------------\n");


                Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
                if (Console.ReadLine() == "n") EndApp = true;

                Console.WriteLine("\n");
            }

            Calculator.Finish();
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
    }
}
