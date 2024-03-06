using System;
using System.Reflection;
using System.Collections.Generic;

using static System.Console;

namespace Stonedrago.Application;

using AI;
using Model;

public class CommandLineInterface
{
    private string command = string.Empty;
    private Game game = null;
    private IMiniMax ai = null;

    public void Start()
    {
        ForegroundColor = ConsoleColor.Green;
        Clear();
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
        command += ReadLine();
        var trimCommand = command.Trim();
        if (trimCommand.EndsWith("/"))
            return true;

        if (trimCommand.ToLower() == "exit")
            return false;
        
        var data = getData(trimCommand);
        if (data.Length == 0)
            return true;
        
        var methods = typeof(CommandLineInterface).GetMethods();
        foreach (var method in methods)
        {
            if (method.Name.ToLower() != data[0].ToLower())
                continue;
            
            runMethod(method, data);
            command = string.Empty;
            return true;     
        }
        
        Start();
        ForegroundColor = ConsoleColor.Red;
        WriteLine("Unkown command.");
        Help();
        ReadKey(true);
        Start();
        command = string.Empty;
        return true;
    }

    private void runMethod(MethodInfo method, string[] data)
    {
        try
        {
            Start();
            int dataIndex = 1;
            List<object> parameters = new List<object>();
            foreach (var parameter in method.GetParameters())
            {
                if (dataIndex >= data.Length)
                    break;

                if (parameter.ParameterType == typeof(string))
                    parameters.Add(data[dataIndex++]);
                
                if (parameter.ParameterType == typeof(int))
                    parameters.Add(int.Parse(data[dataIndex++]));                
            }
            method.Invoke(this, parameters.ToArray());
        }
        catch (TargetParameterCountException)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine($"The method {method.Name} needs the follow parameters");
            foreach (var parameter in method.GetParameters())
            {
                Write($"\t-{parameter.Name} : {parameter.ParameterType}");
                if (parameter.IsOptional)
                    WriteLine($" = {parameter.DefaultValue}");
                else WriteLine($" [required]");
            }
        }
        catch (Exception ex)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine($"Unknow Error: {ex}");
        }

        ReadKey(true);
        Start();
    }

    public void Help()
    {

    }

    public void Load(string pgn)
    {
        game = Game.FromPGN(pgn);
        ForegroundColor = ConsoleColor.Magenta;
        WriteLine("Game loaded successfully.");
    }
    
    public void Print()
    {
        ForegroundColor = ConsoleColor.Magenta;
        WriteLine(game?.ToString() ?? "Empty Game.");
    }

    public void Expand(int depth)
    {
        if (game is null)
            game = new Game();

        if (ai is null)
        {
            ai = new Expectiminimax();
            ai.Load(game, new DefaultAvaliator());
        }

        ai.Expand(depth);
    }

    private string[] getData(string command)
    {
        command = command.Replace("/", "");
        command = command.Replace("\n", "");
        command = command.Replace("\r", "");

        List<string> data = new();

        string current = string.Empty;
        bool inString = false;
        foreach (var character in command)
        {
            if (inString && character != '\'')
            {
                current += character;
                continue;
            }
            
            if (inString && character == '\'')
            {
                inString = false;
                data.Add(current);
                current = string.Empty;
                continue;
            }
            
            if (!inString && character == '\'')
            {
                inString = true;
                continue;
            }

            if (character == ' ' && current != "")
            {
                data.Add(current);
                current = string.Empty;
                continue;
            }

            if (!char.IsAsciiLetterOrDigit(character))
                continue;
            
            current += character;
        }
        if (current != "")
            data.Add(current);

        return data.ToArray();
    }
}