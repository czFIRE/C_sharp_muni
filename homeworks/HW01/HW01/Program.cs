// See https://aka.ms/new-console-template for more information
using HW01;

Console.WriteLine("Hello, World!");

//Console.WriteLine("Ahoj {0}", string.Join(", ", Constants.KnownCommands));

//Console.WriteLine("Ahoj {0}", string.Join(", ", Enum.GetNames(typeof(Constants.Commands))));

//Console.WriteLine(Enum.GetName(typeof(Constants.Commands), Constants.Commands.rip));

var entity = new Entity(Entities.EntityList[0]);

Printer.PrintEntityWithLevels(entity);

Console.WriteLine(Entities.EntityList[0]);