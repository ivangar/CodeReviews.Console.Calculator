using System.Text.Encodings.Web;
using System.Text.Json;

namespace CalculatorLibrary.Logging
{
    public class CalculatorLogWriter : IDisposable
    {
        private Utf8JsonWriter _writer;
        private FileStream _stream;
        private readonly string _filePath;

        public CalculatorLogWriter(string filePath = "calculator.json")
        {
            _filePath = filePath;
            InitializeWriter();
        }

        public void WriteOperation(double operand1, double operand2, char symbol, double result)
        {
            _writer.WriteStartObject();
            _writer.WriteNumber("Operand1", operand1);
            _writer.WriteNumber("Operand2", operand2);
            _writer.WriteString("Operation", symbol.ToString());
            WriteResult(result);
        }

        public void WriteOperation(double operand1, char symbol, double result)
        {
            _writer.WriteStartObject();
            _writer.WriteNumber("Operand1", operand1);
            _writer.WriteString("Operation", symbol.ToString());
            WriteResult(result);
        }

        public void WriteOperationSymbol(char symbol)
        {
            _writer.WriteStringValue(symbol.ToString());
        }

        public void WriteResult(double result)
        {
            _writer.WriteNumber("Result", result);
            _writer.WriteEndObject();
        }

        public void DeleteOperations()
        {
            // Close writer AND stream
            Dispose();
            _stream.Dispose();

            // Read existing JSON to check if operations exist
            string json = File.ReadAllText(_filePath);

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("Operations", out var ops) || ops.GetArrayLength() == 0)
            {
                Console.WriteLine("No operations to delete.");
                InitializeWriter();   // reopen writer so logging continues
                return;
            }

            Console.WriteLine("Deleting all operations from log file.");

            EmptyOperations();
            InitializeWriter();
        }

        public void Finish()
        {
            _writer.WriteEndArray();
            _writer.WriteEndObject();
        }

        public void Dispose()
        {
            Finish();
            _writer.Dispose();
        }

        // Rewrite file with empty Operations array
        private void EmptyOperations()
        {
            using var stream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            var options = new JsonWriterOptions
            {
                Indented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            using var newWriter = new Utf8JsonWriter(stream, options);

            newWriter.WriteStartObject();
            newWriter.WritePropertyName("Operations");
            newWriter.WriteStartArray();
            newWriter.WriteEndArray();
            newWriter.WriteEndObject();

            newWriter.Flush();

            Dispose();

            InitializeWriter();
        }

        // Initialize writer for logging
        private void InitializeWriter()
        {
            _stream = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None);

            var options = new JsonWriterOptions
            {
                Indented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            _writer = new Utf8JsonWriter(_stream, options);

            // Move writer to the end of the Operations array
            _writer.WriteStartObject();
            _writer.WritePropertyName("Operations");
            _writer.WriteStartArray();
        }
    }
}
