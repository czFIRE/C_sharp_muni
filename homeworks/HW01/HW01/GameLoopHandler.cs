namespace HW01
{
    internal class GameLoopHandler : IGame
    {

        public int Run()
        {
            // initialize player

            // initialize dungeons

            // 
            return 0;
        }

        public int ParsePlayerCommand()
        {
            var command = Console.ReadLine();

            if (Enum.TryParse(command, out Constants.Commands cmd))
            {
                Printer.PrintIncorectCommandError(command);
                return 1;
            }

            switch (cmd)
            {
                case Constants.Commands.inspect:
                    break;
                case Constants.Commands.fight:
                    break;
                case Constants.Commands.info:
                    break;
                case Constants.Commands.reorder:
                    break;
                case Constants.Commands.rip:
                    break;
                // new unsupported command
                default:
                    return 2;
            }

            return 0;
        }
    }
}
