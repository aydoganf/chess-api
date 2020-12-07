using System.Collections.Generic;
using System.Linq;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public class Knight : Stone
    {
        internal Knight(string name, bool couldMove, EnumStoneColor color, Location location, Player player) : base(name, couldMove, color, location, player)
        {
        }

        public Knight(EnumStoneColor color, int x, int y) : base(string.Empty, true, color, SessionFactory.GetTable().GetLocation(x, y), null)
        {
        }

        public override List<Location> GetMovementLocations(Location target, Table table)
        {
            return null;
        }

        public override bool TryMove(Location target, Table table, out IStone willEated)
        {
            willEated = default;
            if (CheckMove(target, table) == false)
            {
                return false;
            }

            var targetLocationStone = table.Stones.FirstOrDefault(s => s.Location == target);
            if (targetLocationStone != null)
            {
                willEated = targetLocationStone;
            }
            return true;
        }

        protected override bool CheckMove(Location target, Table table)
        {
            // gidilecek location'da kendi taşı varsa
            if (Player.Stones.FirstOrDefault(s => s.Location == target) != null)
            {
                return false;
            }

            if (target.X < 0 || target.Y < 0)
            {
                return false;
            }

            var span = target - Location;
            if ((span.XDiff == 1 && span.YDiff == 2) || (span.XDiff == 2 && span.YDiff == 1))
            {
                return true;
            }

            return false;
        }
    }
}
