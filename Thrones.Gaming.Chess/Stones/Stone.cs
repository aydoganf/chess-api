using System;
using System.Collections.Generic;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public abstract class Stone : IStone
    {
        public string Name => $"{GetType().Name.ToLower()}#{Location.Name}";
        public bool CouldMove { get; protected set; }
        public EnumStoneColor Color { get; protected set; }
        public Location StoredLocation { get; set; }
        public Location Location { get; protected set; }
        public Location GhostLocation { get; protected set; }
        public Player Player { get; protected set; }
        public int MoveCount { get; protected set; }
        public string NameWithColorPrefix => $"{GetType().Name.ToLower()}#{Color.ToString().ToLower()[0]}";

        public Stone(string name, bool couldMove, EnumStoneColor color, Location location, Player player)
        {
            //Name = name;
            CouldMove = couldMove;
            Color = color;
            Location = location;
            Player = player;
            MoveCount = 0;
        }

        protected abstract bool CheckMove(Location target, Table table);

        public virtual bool Move(Location target, Table table, out IStone eated) 
        {
            if (TryMove(target, table, out eated))
            {
                Move(target);
                return true;
            }

            return false;
        }
        public abstract bool TryMove(Location target, Table table, out IStone willEated);

        protected virtual void Move(Location location)
        {
            Location = location;
            MoveCount++;
        }

        public void GhostMove(Location target)
        {
            GhostLocation = target;
            StoredLocation = Location;
            Location = GhostLocation;
        }

        public void UndoGhost()
        {
            GhostLocation = null;
            Location = StoredLocation;
        }

        public void ForceMove(Location target)
        {
            Location = target;
            MoveCount--;
        }

        public void SetPlayer(Player player)
        {
            Player = player;
        }

        public abstract List<Location> GetMovementLocations(Location target, Table table);
    }
}
