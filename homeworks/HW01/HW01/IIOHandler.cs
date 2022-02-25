namespace HW01
{
    internal interface IIOHandler
    {
        public void WriteLine(string? line);

        public void Write(string? line);

        public string? ReadLine();

        public void SetForegroundColour(ConsoleColor colour);
        public ConsoleColor GetForegroundColor();
    }
}
