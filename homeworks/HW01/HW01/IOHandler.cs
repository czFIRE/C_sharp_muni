namespace HW01
{
    internal class IOHandler : IIOHandler
    {
        public static string? ReadLine()
        {
            return Console.ReadLine();
        }

        public static void WriteLine(string? line)
        {
            Console.WriteLine(line);
        }

        public static void Write(string? line)
        {
            Console.Write(line);
        }

        public static void SetForegroundColour(ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
        }

        public static ConsoleColor GetForegroundColor()
        {
            return Console.ForegroundColor;
        }




        string? IIOHandler.ReadLine()
        {
            return ReadLine();
        }

        void IIOHandler.Write(string? line)
        {
            Write(line);
        }

        void IIOHandler.WriteLine(string? line)
        {
            WriteLine(line);
        }

        void IIOHandler.SetForegroundColour(ConsoleColor colour)
        {
            SetForegroundColour(colour);
        }

        ConsoleColor IIOHandler.GetForegroundColor()
        {
            return GetForegroundColor();
        }
    }
}
