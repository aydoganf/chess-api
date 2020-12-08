using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Logging;
using Thrones.Gaming.Chess.Movement;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.Provider;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.SessionManagement
{
    public abstract class Session : ISession
    {
        private IGameProvider _gameProvider;

        public string Name { get; protected set; }
        public Table Table { get; protected set; }

        #region player

        public List<Player> Players { get; protected set; }
        public Player CurrentPlayer { get; protected set; }
        public Player NextPlayer { get; protected set; }
        public Player PreviousPlayer => NextPlayer;
        public int CurrentIndexer { get; set; }


        public ISession AddPlayers(string blackPlayerNickname, string whitePlayerNickname)
        {
            string[] players = new string[2] { blackPlayerNickname, whitePlayerNickname };

            for (int i = 0; i < players.Length; i++)
            {
                var color = EnumStoneColor.Black;
                if (i == 1)
                {
                    color = EnumStoneColor.White;
                }
                string nickname = players[i];
                var player = Player.CreateOne(nickname, color);

                #region Pawns

                int pawnLine = i == 0 ? 7 : 2;
                List<IStone> stones = new List<IStone>();
                foreach (var xAxis in Table.xAxis.Keys)
                {
                    var xAxisVal = Table.xAxis[xAxis];
                    var name = $"pawn#{xAxisVal}{pawnLine}";
                    var pawn = new Pawn(name, true, color, Table.GetLocation(xAxis, pawnLine), player);

                    stones.Add(pawn);
                }
                #endregion

                #region Rooks

                Rook rookLeft = default;
                Rook rookRight = default;

                if (color == EnumStoneColor.Black)
                {
                    rookLeft = new Rook("rook#a8", false, color, Table.GetLocation(1, 8), player);
                    rookRight = new Rook("rook#h8", false, color, Table.GetLocation(8, 8), player);
                }
                else
                {
                    rookLeft = new Rook("rook#a1", false, color, Table.GetLocation(1, 1), player);
                    rookRight = new Rook("rook#h1", false, color, Table.GetLocation(8, 1), player);
                }

                stones.Add(rookLeft);
                stones.Add(rookRight);

                #endregion

                #region Knights
                Knight knightLeft = default;
                Knight knightRight = default;

                if (color == EnumStoneColor.Black)
                {
                    knightLeft = new Knight("knight#b8", true, color, Table.GetLocation(2, 8), player);
                    knightRight = new Knight("knight#g8", true, color, Table.GetLocation(7, 8), player);
                }
                else
                {
                    knightLeft = new Knight("knight#b1", true, color, Table.GetLocation(2, 1), player);
                    knightRight = new Knight("knight#g1", true, color, Table.GetLocation(7, 1), player);
                }

                stones.Add(knightLeft);
                stones.Add(knightRight);
                #endregion

                #region Bishops
                Bishop bishopLeft = default;
                Bishop bishopRight = default;

                if (color == EnumStoneColor.Black)
                {
                    bishopLeft = new Bishop("bishop#c8", false, color, Table.GetLocation(3, 8), player);
                    bishopRight = new Bishop("bishop#f8", false, color, Table.GetLocation(6, 8), player);
                }
                else
                {
                    bishopLeft = new Bishop("bishop#c1", false, color, Table.GetLocation(3, 1), player);
                    bishopRight = new Bishop("bishop#f1", false, color, Table.GetLocation(6, 1), player);
                }

                stones.Add(bishopLeft);
                stones.Add(bishopRight);
                #endregion

                #region Queens
                Queen queen = default;
                if (color == EnumStoneColor.Black)
                {
                    queen = new Queen("queen", false, color, Table.GetLocation(4, 8), player);
                }
                else
                {
                    queen = new Queen("queen", false, color, Table.GetLocation(4, 1), player);
                }

                stones.Add(queen);
                #endregion

                #region Kings
                King king = default;

                if (color == EnumStoneColor.Black)
                {
                    king = new King("king", false, color, Table.GetLocation(5, 8), player);
                }
                else
                {
                    king = new King("king", false, color, Table.GetLocation(5, 1), player);
                }

                stones.Add(king);
                #endregion


                player.SetStones(stones);

                Players.Add(player);
                Table.AddStones(stones);
            }

            return this;
        }

        public ISession AddPlayer(string nickname, EnumStoneColor color, List<IStone> stones)
        {
            var player = Player.CreateOne(nickname, color);
            player.SetStones(stones);
            Players.Add(player);
            Table.AddStones(stones);
            return this;
        }

        public ISession AddPlayer(Player player)
        {
            Players.Add(player);
            Table.AddStones(player.Stones);
            return this;
        }
        #endregion

        #region check/checkmate

        public bool Check { get; protected set; }
        public bool Checkmate { get; private set; }
        public IStone CheckStone { get; set; }
        private void IsCheck(IStone stone)
        {
            King king = NextPlayer.GetKing();
            if (stone.TryMove(king.Location, Table, out IStone k))
            {
                Check = true;
                CheckStone = stone;
            }

            if (Check)
            {
                IsCheckmate(stone);
                if (Checkmate)
                {
                    Check = false;
                }
            }
        }

        private void IsCheckmate(IStone stone)
        {
            var king = NextPlayer.GetKing();

            // check yapan taşı yiyebilecek bir taş var mı?
            bool checkStoneCouldEated = false;
            IStone checkStoneEater = null;
            foreach (var nextPlayerStone in NextPlayer.Stones)
            {
                if (nextPlayerStone.TryMove(stone.Location, Table, out IStone _s))
                {
                    bool checkStoneEaterCouldMove = true;
                    // şah çeken taşı yiyebilecek olan taş bu hareketi yapabilir mi?
                    nextPlayerStone.GhostMove(stone.Location);

                    foreach (var currentPlayerStone in CurrentPlayer.Stones)
                    {
                        if (currentPlayerStone == stone)
                        {
                            // şah çeken taşı yediğini varsayıyoruz
                            continue;
                        }

                        if (currentPlayerStone.TryMove(stone.Location, Table, out IStone _k))
                        {
                            // danger
                            // nextPlayerStone could not move!!
                            checkStoneEaterCouldMove = false;
                            break;
                        }
                    }

                    nextPlayerStone.UndoGhost();

                    if (checkStoneEaterCouldMove)
                    {
                        checkStoneCouldEated = true;
                        checkStoneEater = nextPlayerStone;
                        break;
                    }
                }
            }

            if (checkStoneCouldEated)
            {
                if (checkStoneEater is King)
                {
                    // stone'u o lokasyondan geçici olarak alalım
                    // daha sonra o lokasyona bir taş gidebilir mi ona bakalım.
                    // gidemiyorsa stone'u o laskyona geri alalım
                    stone.GhostMove(null);

                    foreach (var currentPlayerStone in CurrentPlayer.Stones)
                    {
                        if (currentPlayerStone == stone)
                        {
                            continue;
                        }

                        if (currentPlayerStone.TryMove(stone.StoredLocation, Table, out IStone _k))
                        {
                            //Checkmate = true;
                            //stone.UndoGhost();
                            //return;

                            checkStoneCouldEated = false;
                            break;
                        }
                    }

                    stone.UndoGhost();
                }

                //return;
            }


            // başka bir taş kral ile check yapan taş arasına girebilir mi?

            bool someStoneBroked = false;

            List<Location> checkLocations = stone.GetMovementLocations(king.Location, Table);
            if (checkLocations != null)
            {
                foreach (var checkLocation in checkLocations)
                {
                    foreach (var nextPlayerStone in NextPlayer.Stones)
                    {
                        if (nextPlayerStone is King)
                        {
                            continue;
                        }

                        if (nextPlayerStone.TryMove(checkLocation, Table, out IStone _s))
                        {
                            someStoneBroked = true;
                            break;
                        }
                    }

                    if (someStoneBroked) break;
                }
            }


            // king kaçabilir mi?
            if (checkStoneCouldEated == false && someStoneBroked == false && king.CouldRun(Table, stone.Location) == false)
            {
                Checkmate = true;
                Check = false;
            }
        }
        #endregion

        #region movement

        private Stopwatch SessionTimer { get; set; }
        protected Dictionary<Instruction, MovementResult> MovementInstructions { get; set; } = new Dictionary<Instruction, MovementResult>();
        protected List<string> StartingCommands { get; set; } = new List<string>();
        private Instruction GetLastMovement() => MovementInstructions.Keys.LastOrDefault();

        public ISession AddStartingCommands(string[] commands)
        {
            StartingCommands = commands.ToList();

            return this;
        }
        #endregion

        #region logging

        protected ILogger Logger { get; private set; }
        private string _logFileName => $"{Name.Replace(" ", "")}-{DateTime.Now.ToString("dd.MM.yyyy-HHmmss")}.txt";

        public ISession SetLogger(ILogger logger)
        {
            Logger = logger;

            return this;
        }
        #endregion

        #region ctors

        protected Session(string name, IGameProvider gameProvider) 
        {
            Name = name;
            CreateNew();
            _gameProvider = gameProvider;
        }

        public Session()
        {
            CreateNew();
        }

        #endregion

        #region factory

        private void CreateNew()
        {
            Table = Table.CreateOne();
            Players = new List<Player>();

            SessionTimer = new Stopwatch();
            SessionTimer.Start();
        }

        public void SetName(string name)
        {
            Name = name;
        }

        #endregion

        #region utils

        public abstract void ShowInfo();
        public abstract void DrawTable();
        public abstract void WriteMessage(string message);
        public abstract void WriteError(string error);
        public abstract void WriteEmpty();
        public abstract string WaitCommand();
        public abstract void WriteLastCommand(string rawCommand);
        public abstract void DrawStatistics();
        protected void SetPlayerReturn()
        {
            int indexer = CurrentIndexer + 1;
            CurrentIndexer = indexer % 2;
            CurrentPlayer = Players[CurrentIndexer];
            NextPlayer = Players[CurrentIndexer == 0 ? 1 : 0];
        }
        #endregion


        public virtual void Start()
        {
            Logger?.SetFileName(_logFileName);
            CurrentPlayer = Players[CurrentIndexer];
            NextPlayer = Players[CurrentIndexer == 0 ? 1 : 0];
            
            DrawTable();

            string command = string.Empty;
            if (StartingCommands.Count != 0)
            {
                command = StartingCommands[0];
            }
            

            

            return;
            while (command != "quit" && command != "exit")
            {
                if (string.IsNullOrEmpty(command))
                {
                    ShowInfo();
                }
                else
                {
                    string[] commandArr = command.Split(' ');
                    if (commandArr.Length == 2 && commandArr[0].Length == 2 && commandArr[1].Length == 2)
                    {
                        command = $"play {command}";
                    }
                    
                    if (command == "draw")
                    {
                        DrawTable();
                        if (Checkmate)
                        {
                            WriteError("Checkmate !!!");
                        }
                    }

                    if (command == "undo")
                    {
                        if (Checkmate)
                        {
                            WriteError("Checkmate !!!");
                        }

                        if (Checkmate == false && MovementInstructions.Keys.Count != 0)
                        {
                            var lastInstraction = GetLastMovement();
                            var lastResult = MovementInstructions[lastInstraction];

                            if (lastResult.Eated != null)
                            {
                                CurrentPlayer.Stones.Add(lastResult.Eated);                                
                                PreviousPlayer.Eats.Remove(lastResult.Eated);
                            }

                            lastInstraction.MovingStone.ForceMove(lastInstraction.FromLocation);

                            SetPlayerReturn();

                            MovementInstructions.Remove(lastInstraction);
                            DrawTable();

                            SessionTimer.Restart();
                        }
                    }

                    if (Checkmate == false && command.StartsWith("play"))
                    {
                        var commandDetail = CommandResolver.Resolve(command);
                        if (commandDetail.IsCorrect == false)
                        {
                            WriteError(commandDetail.ReturnMessage);
                            command = WaitCommand();
                            continue;
                        }

                        var fromLocation = Table.GetLocation(commandDetail.From_X, commandDetail.From_Y);

                        var stone = Table.Stones.GetFromLocation(fromLocation); 
                        var targetLocation = Table.GetLocation(commandDetail.To_X, commandDetail.To_Y);
                        
                        if (targetLocation == null)
                        {
                            WriteError("Location is not found!");
                            command = WaitCommand();
                            continue;
                        }

                        if (stone == null)
                        {
                            WriteError("Stone is not found at given location!");
                            command = WaitCommand();
                            continue;
                        }

                        var instraction = Instruction.CreateOne(stone, targetLocation, this, command);
                        
                        var result = instraction.TryDo();
                        if (result.IsOK)
                        {
                            MovementInstructions.Add(instraction, result);

                            if (result.Eated != null)
                            {
                                CurrentPlayer.Eat(result.Eated);
                                Table.Stones.Remove(result.Eated);
                            }

                            if (result.CheckRemoved)
                            {
                                Check = false;
                            }

                            WriteMessage(result.Message);
                            IsCheck(stone);

                            SessionTimer.Stop();
                            CurrentPlayer.Duration += SessionTimer.ElapsedMilliseconds;

                            if (Checkmate)
                            {
                                DrawTable();
                                WriteEmpty();
                                WriteError("Checkmate !!!");
                            }
                            else
                            {
                                SetPlayerReturn();
                                DrawTable();

                                SessionTimer.Restart();
                            }

                            
                        }
                        else
                        {
                            WriteError(result.Message);
                        }
                    }

                    if (command == "stat")
                    {
                        DrawStatistics();
                    }
                }

                if (StartingCommands.Count == 0)
                {
                    var lastMovementInstaction = GetLastMovement();
                    if (lastMovementInstaction != null)
                    {
                        WriteLastCommand(lastMovementInstaction.Log);
                    }

                    command = WaitCommand();
                }
                else
                {
                    var current = StartingCommands.First();
                    StartingCommands.Remove(current);
                    if (StartingCommands.Count != 0)
                    {
                        command = StartingCommands[0];
                    }
                    else
                    {
                        var lastMovementInstaction = GetLastMovement();
                        if (lastMovementInstaction != null)
                        {
                            WriteLastCommand(lastMovementInstaction.Log);
                        }
                        command = WaitCommand();
                    }
                }
            }

            Logger?.SaveInstructions(MovementInstructions.Where(m => m.Value.IsOK).Select(m => m.Key));
        }

        public SessionInformation Command(string command)
        {
            var commandDetail = CommandResolver.Resolve(command);

            if (commandDetail.CommandType == CommandType.Draw)
            {
                DrawTable();
            }

            if (commandDetail.CommandType == CommandType.Undo)
            {
                if (Checkmate)
                {
                    WriteError("Checkmate !!!");
                }

                if (Checkmate == false && MovementInstructions.Keys.Count != 0)
                {
                    var lastInstraction = GetLastMovement();
                    var lastResult = MovementInstructions[lastInstraction];

                    if (lastResult.Eated != null)
                    {
                        CurrentPlayer.Stones.Add(lastResult.Eated);
                        PreviousPlayer.Eats.Remove(lastResult.Eated);
                    }

                    lastInstraction.MovingStone.ForceMove(lastInstraction.FromLocation);

                    SetPlayerReturn();

                    MovementInstructions.Remove(lastInstraction);
                    DrawTable();

                    SessionTimer.Restart();
                }
            }

            if (commandDetail.CommandType == CommandType.Stat)
            {
                DrawStatistics();
            }

            if (commandDetail.CommandType == CommandType.Play && Checkmate == false)
            {
                if (commandDetail.IsCorrect == false)
                {
                    WriteError(commandDetail.ReturnMessage);

                    MovementInstructions.Add(default, new MovementResult(false, null, null, null, commandDetail.ReturnMessage));
                    return SessionInformation;
                }

                #region check locations

                var fromLocation = Table.GetLocation(commandDetail.From_X, commandDetail.From_Y);

                var stone = Table.Stones.GetFromLocation(fromLocation);
                var targetLocation = Table.GetLocation(commandDetail.To_X, commandDetail.To_Y);

                if (targetLocation == null)
                {
                    WriteError("Location is not found!");

                    MovementInstructions.Add(default, new MovementResult(false, stone, null, null, "Location is not found"));
                    return SessionInformation;
                }

                if (stone == null)
                {
                    WriteError("Stone is not found at given location!");

                    MovementInstructions.Add(default, new MovementResult(false, null, null, targetLocation, "Stone is not found at given location!"));
                    return SessionInformation;
                }
                #endregion

                var instraction = Instruction.CreateOne(stone, targetLocation, this, command);

                var result = instraction.TryDo();
                if (result.IsOK)
                {
                    if (result.Eated != null)
                    {
                        CurrentPlayer.Eat(result.Eated);
                        Table.Stones.Remove(result.Eated);
                    }

                    if (result.CheckRemoved)
                    {
                        Check = false;
                    }

                    WriteMessage(result.Message);
                    IsCheck(stone);

                    SessionTimer.Stop();
                    CurrentPlayer.Duration += SessionTimer.ElapsedMilliseconds;

                    if (Checkmate)
                    {
                        DrawTable();
                        WriteEmpty();
                        WriteError("Checkmate !!!");
                    }
                    else
                    {
                        SetPlayerReturn();
                        DrawTable();

                        if (Check)
                        {
                            WriteError("CHECK");
                        }
                        

                        SessionTimer.Restart();
                    }
                }
                else
                {
                    WriteError(result.Message);
                }

                MovementInstructions.Add(instraction, result);
                return SessionInformation;
            }

            return SessionInformation;
        }

        public SessionInformation SessionInformation => new SessionInformation()
        {
            CurrentIndexer = CurrentIndexer,
            FullTypeName = GetType().FullName,
            Name = Name,
            Players = Players.Select(p => new PlayerInformation()
            {
                Nickname = p.Nickname,
                Duration = p.Duration
            }).ToList(),
            Table = new TableInformation()
            {
                Stones = Table.Stones.Select(s => new StoneInformation(s)).ToList()
            },
            MovementResult = BuildMovementResultInformation()
        };

        private MovementResultInformation BuildMovementResultInformation()
        {
            string command = MovementInstructions.Keys.LastOrDefault()?.RawCommand;
            var result = MovementInstructions.GetLast();

            return new MovementResultInformation(result, command, Check, Checkmate);
        }

        public ISession SetIndexer(int currentIndexer)
        {
            CurrentIndexer = currentIndexer;
            CurrentPlayer = Players[CurrentIndexer];
            NextPlayer = Players[CurrentIndexer == 0 ? 1 : 0];
            return this;
        }

        public SessionInformation GetInformation() => SessionInformation;
    }
}
