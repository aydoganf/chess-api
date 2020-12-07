using System;
using System.Linq;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.Movement
{
    public class Instruction
    {
        public IStone MovingStone { get; private set; }
        public Location FromLocation { get; private set; }
        public Location Target { get; private set; }
        public Session Session { get; private set; }
        public MovementResult Result { get; private set; }
        public string RawCommand { get; private set; }

        private Instruction(IStone stone, Location target, Session session, string rawCommand)
        {
            MovingStone = stone;
            Target = target;
            Session = session;
            RawCommand = rawCommand;
            FromLocation = stone.Location;
        }

        public static Instruction CreateOne(IStone stone, Location target, Session session, string rawCommand)
        {
            return new Instruction(stone, target, session, rawCommand);
        }

        public MovementResult TryDo()
        {
            IStone willEated = default;

            var preMoveResult = PreMove();
            if (preMoveResult.IsOK == false)
            {
                return preMoveResult;
            }

            if (MovingStone.TryMove(Target, Session.Table, out willEated) != true)
            {
                return new MovementResult(false, MovingStone, null, Target, "Oraya olmaz!");
            }

            bool isOk = true;
            bool checkRemoved = false;
            string message = string.Empty;

            if (Session.Check)
            {
                if (willEated == Session.CheckStone)
                {
                    isOk = true;
                    checkRemoved = true;
                }

                // kralın önüne geçtiyse ve check-i bitirdiyse
                else
                {
                    // CurrentPlayer'ın MovingStone'u o lokasyona ghostMove yapar.
                    MovingStone.GhostMove(Target);

                    // Şuanki durumda CurrentPlayer'a sıra geçecektir.
                    var king = Session.CurrentPlayer.GetKing();
                    if (Session.CheckStone.TryMove(king.Location, Session.Table, out IStone _k))
                    {
                        // hala check-i kaldıramıyor.
                        isOk = false;
                    }
                    else
                    {
                        isOk = true;
                        checkRemoved = true;
                    }

                    MovingStone.UndoGhost();
                }
            }

            if (isOk)
            {
                message = $"<strong>{Session.CurrentPlayer.Nickname}</strong> oyuncusu <strong>{FromLocation.Name}</strong> -> <strong>{Target.Name}</strong> oynadı.";

                MovingStone.Move(Target, Session.Table, out willEated);
                return new MovementResult(true, MovingStone, willEated, Target, message, checkRemoved);
            }

            return new MovementResult(false, MovingStone, null, Target, message);
        }

        private MovementResult PreMove()
        {
            if (MovingStone is King)
            {
                return CheckKingMove();
            }
            else
            {
                var targetLocationStone = Session.Table.Stones.GetFromLocation(Target);
                MovingStone.GhostMove(Target);
                
                bool couldMove = true;
                IStone willEated = null;

                foreach (var nextPlayerStone in Session.NextPlayer.Stones)
                {
                    if (nextPlayerStone == targetLocationStone)
                    {
                        continue;
                    }

                    if (nextPlayerStone.TryMove(Session.CurrentPlayer.GetKing().Location, Session.Table, out willEated))
                    {
                        couldMove = false;
                        break;
                    }
                }

                MovingStone.UndoGhost();
                return new MovementResult(couldMove, MovingStone, willEated, Target, couldMove ? "OK" : "Protect your KING!");
            }
        }

        private MovementResult CheckKingMove()
        {
            MovingStone.GhostMove(Target);
            bool couldMove = true;
            IStone willEated = null;
            IStone eater = null;

            foreach (var nextPlayerStone in Session.NextPlayer.Stones)
            {
                if (nextPlayerStone.TryMove(MovingStone.Location, Session.Table, out willEated))
                {
                    couldMove = false;
                    eater = nextPlayerStone;
                    break;
                }
            }

            MovingStone.UndoGhost();

            return new MovementResult(couldMove, MovingStone, willEated, Target, couldMove ? "OK" : $"No way man.. {eater.Name} will kill your King!");
        }
    }
}
