namespace HW01
{
    internal class IOHandler : IIOHandler
    {
        public string? ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine(string? line)
        {
            Console.WriteLine(line);
        }

        public void Write(string? line)
        {
            Console.Write(line);
        }

        public void SetForegroundColour(ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
        }

        public ConsoleColor GetForegroundColor()
        {
            return Console.ForegroundColor;
        }

    }
}
