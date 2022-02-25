// See https://aka.ms/new-console-template for more information
using HW01;

Console.WriteLine("Hello, World!");

//Console.WriteLine("Ahoj {0}", string.Join(", ", Constants.KnownCommands));

//Console.WriteLine("Ahoj {0}", string.Join(", ", Enum.GetNames(typeof(Constants.Commands))));

//Console.WriteLine(Enum.GetName(typeof(Constants.Commands), Constants.Commands.rip));

var entity = new Adventurer(Entities.AdventurerList[0]);

Printer.PrintEntityWithLevels(entity);

Console.WriteLine(Entities.AdventurerList[0]);

Printer.LevelUpMessage(entity);

Random rnd = new Random();

var my_list = Entities.AdventurerList.OrderBy(x => rnd.Next()).Take(3).ToList();

Console.WriteLine(my_list[0]);

var player = new Player(Entities.AdventurerList);

foreach (var i in player.Adventurers)
{
    Console.WriteLine(i);
}

player.ReorderAdventurers();

foreach (var i in player.Adventurers)
{
    Console.WriteLine(i);
}