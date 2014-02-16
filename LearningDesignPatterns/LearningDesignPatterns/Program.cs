namespace DesignPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            Patterns.Observer.Executor.Execute();

            System.Threading.Thread.Sleep(15000);
        }
    }
}
