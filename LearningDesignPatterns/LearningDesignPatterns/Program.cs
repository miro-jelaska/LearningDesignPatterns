namespace DesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            DesignPatterns.Patterns.Singleton.Executor.Execute();

            System.Threading.Thread.Sleep(15000);
        }
    }
}
