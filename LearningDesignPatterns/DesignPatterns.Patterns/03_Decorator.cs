using System;
using System.Linq;

namespace DesignPatterns.Patterns.Decorator
{
    abstract class Text
    {
        public void Print()
        {
            if(this.CurrentText == null)
            {
                Console.WriteLine(" - Text: null => Nothing to print - ");
            }
            else if(this.CurrentText.Trim() == string.Empty)
            {
                Console.WriteLine(" - Text: empty or whitespace => Nothing to print - ");
            }
            else
            {
                Console.WriteLine(this.CurrentText);
            }
        }
        public string CurrentText { get; protected set; }
    }

    class TextReader : Text
    {
        public void ReadText()
        {
            Console.WriteLine("Enter some text");

            var text = Console.ReadLine();
            this.CurrentText = text;
        }
    }

    class ReverseTextDecorator : Text
    {
        public ReverseTextDecorator(Text text)
        {
            this.CurrentText =string.Join("", text.CurrentText.Reverse());
        }
    }
    class ToUpperCaseDecorator : Text
    {
        public ToUpperCaseDecorator(Text text)
        {
            this.CurrentText = text.CurrentText.ToUpper();
        }
    }

    
}
namespace DesignPatterns.Patterns.Decorator
{
    public abstract class Executor
    {
        public static void Execute()
        {
            var textReader = new TextReader();
            textReader.Print();
            textReader.ReadText();

            var text = (Text) textReader;
            text = new ReverseTextDecorator(text);
            text.Print();
            text = new ToUpperCaseDecorator(text);
            text.Print();
            text = new ReverseTextDecorator(text);
            text.Print();
        }
    }
}
