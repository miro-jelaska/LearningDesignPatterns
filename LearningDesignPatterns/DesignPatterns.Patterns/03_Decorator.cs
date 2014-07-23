using System;
using System.Linq;

namespace DesignPatterns.Patterns.Decorator
{
    abstract class Text
    {
        public void Print()
        {
            if(this.Content == null)
            {
                Console.WriteLine(">> Text: null => Nothing to print");
            }
            else if(this.Content.Trim() == string.Empty)
            {
                Console.WriteLine(">> Text: empty or whitespace => Nothing to print");
            }
            else
            {
                Console.WriteLine(this.Content);
            }
        }
        public string Content { get; protected set; }
    }

    class TextReader : Text
    {
        public void ReadText()
        {
            Console.WriteLine("Enter some text");

            var text = Console.ReadLine();
            this.Content = text;
        }
    }

    class ReverseTextDecorator : Text
    {
        public ReverseTextDecorator(Text text)
        {
            if(text != null)
            {
                this.Content = string.Join("", text.Content.Reverse());
            }
        }
    }
    class ToUpperCaseDecorator : Text
    {
        public ToUpperCaseDecorator(Text text)
        {
            if(text != null)
            {
                this.Content = text.Content.ToUpper();
            }
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
