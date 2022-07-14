namespace MyApp
{
    using ConciergeDevTools;

    internal class Program
    {
        static void Main(string[] args)
        {
            ParseScrubbedData.Parse(args[0]);
        }
    }
}