using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Movement;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public class Rook : Stone
    {
        internal Rook(string name, bool couldMove, EnumStoneColor color, Location location, Player player) : base(name, couldMove, color, location, player)
        {
        }

        public Rook(EnumStoneColor color, int x, int y) : base(string.Empty, true, color, SessionFactory.GetTable().GetLocation(x, y), null)
        {
        }

        public override List<Location> GetMovementLocations(Location target, Table table)
        {
            List<Location> result = null;

            if (CheckMove(target, table))
            {
                result = new List<Location>();
                var span = target - Location;

                // horizontal
                if (span.YMovement == Direction.None)
                {
                    int currentY = Location.Y;
                    int currentX = Location.X;
                    for (int i = 1; i <= span.XDiff; i++)
                    {
                        currentX += span.XMovement == Direction.Forward ? 1: -1;

                        var location = table.GetLocation(currentX, currentY);
                        result.Add(location);
                    }
                }

                // vertical
                if (span.XMovement == Direction.None)
                {
                    int currentY = Location.Y;
                    int currentX = Location.X;
                    for (int i = 1; i <= span.YDiff; i++)
                    {
                        currentY += span.YMovement == Direction.Forward ? 1 : -1;

                        var location = table.GetLocation(currentX, currentY);
                        result.Add(location);
                    }
                }
            }

            return result;
        }

        public override bool TryMove(Location target, Table table, out IStone willEated)
        {
            willEated = default;
            if (CheckMove(target, table) == false)
            {
                return false;
            }

            if (MovementRules.HorizontalOrVerticalCheck(Location, target, table) == false)
            {
                return false;
            }

            willEated = table.Stones.GetFromLocation(target);
            return true;
        }
                
        protected override bool CheckMove(Location target, Table table)
        {
            // gidilecek location'da kendi taşı varsa
            if (Player.Stones.FirstOrDefault(s => s.Location == target) != null)
            {
                return false;
            }

            var span = target - Location;

            if (span.XDiff > 0 && span.YDiff > 0)
            {
                return false;
            }

            return true;
        }
    }
}
