namespace DesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            DesignPatterns.Patterns.Composite.Executor.Execute();

            System.Threading.Thread.Sleep(15000);
        }
    }
}
