namespace DesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            DesignPatterns.Patterns.State.Executor.Execute();

            System.Threading.Thread.Sleep(15000);
        }
    }
}
