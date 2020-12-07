using System;
using System.Collections.Generic;
using System.Text;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Movement
{
    internal enum CommandType
    {
        None,
        Exit,
        Draw,
        Undo,
        Stat,
        Play
    }

    internal struct CommandDetail 
    {
        public bool IsCorrect { get; set; }
        public string ReturnMessage { get; set; }
        public int From_X { get; set; }
        public int From_Y { get; set; }
        public int To_X { get; set; }
        public int To_Y { get; set; }
        public CommandType CommandType { get; set; }

        public CommandDetail(CommandType commandType, bool isCorrect = true, string returnMessage = "", int from_x = default, int from_y = default, int to_x = default, int to_y = default) : this()
        {
            CommandType = commandType;
            IsCorrect = isCorrect;
            ReturnMessage = returnMessage;
            From_X = from_x;
            From_Y = from_y;
            To_X = to_x;
            To_Y = to_y;
        }
    }

    internal static class CommandResolver
    {
        public static CommandDetail Resolve(string command)
        {
            string[] commandArray = command.Split(" ");

            if (commandArray.Length == 3 || commandArray.Length == 2)
            {
                if (commandArray.Length == 2 && commandArray[0].Length == 2 && commandArray[1].Length == 2)
                {
                    command = $"play {command}";
                    commandArray = command.Split(" ");
                }

                string from = commandArray[1];
                string to = commandArray[2];

                if (from.Length != 2)
                {
                    return new CommandDetail(CommandType.Play, false, "from location parameter is wrong!");
                }

                if (to.Length != 2)
                {
                    return new CommandDetail(CommandType.Play, false, "from location parameter is wrong!");
                }

                if (Table.xAxisConverter.ContainsKey(from[0].ToString()) == false || Table.xAxisConverter.ContainsKey(to[0].ToString()) == false)
                {
                    return new CommandDetail(CommandType.Play, false, "location is wrong!");
                }

                int fromX = Table.xAxisConverter[from[0].ToString()];
                int fromY = int.Parse(from[1].ToString());

                int toX = Table.xAxisConverter[to[0].ToString()];
                int toY = int.Parse(to[1].ToString());

                return new CommandDetail(CommandType.Play, true, string.Empty, fromX, fromY, toX, toY);
            }
            else
            {
                if (commandArray[0] == "draw") return new CommandDetail(CommandType.Draw);
                if (commandArray[0] == "undo") return new CommandDetail(CommandType.Undo);
                if (commandArray[0] == "stat") return new CommandDetail(CommandType.Stat);
                if (commandArray[0] == "exit" || commandArray[0] == "quit") return new CommandDetail(CommandType.Exit);

                return new CommandDetail(CommandType.None);
            }
        }
    }
}
