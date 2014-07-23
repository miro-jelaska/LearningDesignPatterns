namespace DesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            Patterns.Decorator.Executor.Execute();

            System.Threading.Thread.Sleep(15000);
        }
    }
}
