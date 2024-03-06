using System;
using static System.Console;

namespace Stonedrago.Application;

public class CommandLineInterface
{
    public void Start()
    {
        ForegroundColor = ConsoleColor.Green;
        WriteLine(
            """
            ██████░██████░██████░██████░██████   █████░░█████░░██████░██████░██████
            ██░░░░░░░██░░░██░░██░██░░██░██░░░░   ██░░██░██░░██░██░░██░██░░░░░██░░██
            ██████░░░██░░░██░░██░██░░██░██████   ██░░██░█████░░██████░██░███░██░░██
            ░░░░██░░░██░░░██░░██░██░░██░██░░░░   ██░░██░██░░██░██░░██░██░░██░██░░██
            ██████░░░██░░░██████░██░░██░██████   █████░░██░░██░██░░██░██████░██████
            """
        );
        WriteLine();
    }

    public bool ReciveCommand()
    {
        ForegroundColor = ConsoleColor.Yellow;
        var command = ReadLine();

        if (command == "exit")
            return false;
        
        

        return true;
    }
}