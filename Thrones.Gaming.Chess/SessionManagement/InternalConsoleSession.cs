using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrones.Gaming.Chess.Provider;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.SessionManagement
{
    internal sealed class InternalConsoleSession : Session
    {
        internal InternalConsoleSession(string name, IGameProvider gameProvider) : base(name, gameProvider)
        {

        }


        public override void WriteMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now} - [system]> {message}");
            Console.ResetColor();
        }

        public override void ShowInfo()
        {
            Console.WriteLine();
            Console.WriteLine("You can use play, draw and quit commands.");
            Console.WriteLine("command: > play [from] [to] (ex: play a3 a5)");
            Console.WriteLine();
            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine();
        }

        public override void DrawTable()
        {
            Console.Clear();

            List<IStone> allStones = new List<IStone>();
            allStones.AddRange(Players.SelectMany(i => i.Stones));

            Console.WindowHeight = 45;
            if (CurrentPlayer == Players[0])
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine($"                              {Players[0].Nickname} # duration: {Players[0].Duration.ToMinute()}                              ");
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine("       a          b          c          d          e          f          g          h      ");
            Console.WriteLine(" _________________________________________________________________________________________ ");
            for (int yAxis = 8; yAxis > 0; yAxis--)
            {
                Console.Write(" |          ");
                for (int xAxis = 1; xAxis < 8; xAxis++)
                {
                    string show = "          ";
                    Console.Write($"|{show}");
                }
                Console.Write("|");
                Console.WriteLine();

                Console.Write($"{Table.yAxis[yAxis]}|");
                for (int xAxis = 1; xAxis <= 8; xAxis++)
                {
                    var location = Table.GetLocation(xAxis, yAxis);
                    var stone = allStones.FirstOrDefault(s => s.Location == location);

                    string show = "          ";
                    if (stone != null)
                    {
                        var name = stone.NameWithColorPrefix;
                        var kalan = 10 - name.Length;
                        string empty = "";
                        for (int i = 0; i < kalan; i++)
                        {
                            empty += " ";
                        }
                        show = name + empty;

                        if (stone.Color == EnumStoneColor.Black)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                    }
                    Console.Write($"{show}");
                    Console.ResetColor();
                    Console.Write("|");
                }
                Console.Write($"{Table.yAxis[yAxis]}");
                Console.WriteLine();

                Console.Write(" |__________");
                for (int xAxis = 1; xAxis < 8; xAxis++)
                {
                    string show = "__________";
                    Console.Write($"|{show}");
                }
                Console.Write("|");
                Console.WriteLine();
            }
            Console.WriteLine("       a          b          c          d          e          f          g          h      ");

            Console.WriteLine();
            if (CurrentPlayer == Players[1])
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine($"                              {Players[1].Nickname} # duration: {Players[1].Duration.ToMinute()}                              ");
            Console.ResetColor();
        }

        public override void WriteError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now} - [system]> {error}");
            Console.ResetColor();
        }

        public override string WaitCommand()
        {
            if (Check == true)
            {
                WriteError("CHECK");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.Write($"{DateTime.Now} - [player: {CurrentPlayer.Nickname}]>");
            Console.ResetColor();
            return Console.ReadLine();
        }

        public override void DrawStatistics()
        {
            foreach (var player in Players)
            {
                Console.WriteLine();

                Console.WriteLine($"   {player.Nickname}   ");
                Console.WriteLine("______________");

                foreach (var eat in player.Eats)
                {
                    Console.WriteLine($"> {eat.Name}");
                }

                Console.WriteLine();
            }
        }

        public override void WriteEmpty()
        {
            Console.WriteLine();
        }

        public override void WriteLastCommand(string rawCommand)
        {
            throw new NotImplementedException();
        }
    }
}
