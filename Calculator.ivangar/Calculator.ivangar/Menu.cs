namespace Calculator.ivangar
{
    public static class Menu
    {
        private static readonly Dictionary<char, string> _operations = new Dictionary<char, string>()
        {
            { 'a', "Add"},
            { 's', "Subtract"},
            { 'm', "Multiply"},
            { 'd', "Divide"},
            { 'r', "Square Root"},
            { 'p', "Power"},
            { 't', "Trigonometry function"},
        };

        public static void Intro()
        {
            Console.WriteLine("Console Calculator in C#\r");
            Console.WriteLine("------------------------\n");
        }

        public static void PrintMenu(bool invalid = false)
        {
            if (invalid)
                Console.WriteLine("\nError: Unrecognized input.");

            Console.WriteLine("\nChoose an operator from the following list:\n");
            PrintMenuOptions();
        }

        public static void PrintMenuOptions()
        {
            foreach (var (key, op) in _operations)
                Console.WriteLine($"\t{key} - {op}");

            Console.Write("Your option? ");
        }

        public static double GetOperand(string message)
        {
            double cleanNum1;

            Console.Write(message);
            string? numInput1 = Console.ReadLine();

            while (!double.TryParse(numInput1, out cleanNum1))
            {
                Console.Write("This is not valid input. Please enter a numeric value: ");
                numInput1 = Console.ReadLine();
            }

            return cleanNum1;
        }

        public static void PrintError(string message)
        {
            Console.WriteLine("This operation will result in a mathematical error.\n");
        }

        public static bool ValidateMainOptions(string? option)
        {
            if (string.IsNullOrEmpty(option) || string.IsNullOrWhiteSpace(option) || option.Length != 1)
                return false;

            return _operations.ContainsKey(char.ToLower(option[0]));
        }
    }
}
