using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DesignPatterns.Patterns.Decorator
{
    interface ITextReader
    {
        string Read();
    }

    class ConsoleTextReader : ITextReader
    {
        public string Read()
        {
            Console.WriteLine("Enter some text");

            return Console.ReadLine();
        }
    }
    class DummyTextReader : ITextReader
    {
        public string Read()
        {
            return "Just use a goto statement.";
        }
    }

    class ReverseTextDecorator : ITextReader
    {
        public ReverseTextDecorator(ITextReader textReader)
        {
            Contract.Requires(textReader != null);

            _textReader = textReader;
        }

        private readonly ITextReader _textReader;


        public string Read()
        {
            var text = _textReader.Read();
            return string.Join("", text.Reverse());
        }
    }
    class ToUpperCaseDecorator : ITextReader
    {
        public ToUpperCaseDecorator(ITextReader textReader)
        {
            Contract.Requires(textReader != null);

            _textReader = textReader;
        }

        private readonly ITextReader _textReader;


        public string Read()
        {
            var text = _textReader.Read();
            return text.ToUpper();
        }
    }
}
namespace DesignPatterns.Patterns.Decorator
{
    public abstract class Executor
    {
        public static void Execute()
        {
            var textReader = new DummyTextReader();
            Console.WriteLine(textReader.Read());

            var text = (ITextReader) textReader;
            text = new ReverseTextDecorator(text);
            Console.WriteLine(text.Read());
            text = new ToUpperCaseDecorator(text);
            Console.WriteLine(text.Read());
            text = new ReverseTextDecorator(text);
            Console.WriteLine(text.Read());
        }
    }
}
