using System.Collections.Generic;
using System.Linq;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Movement;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public class Queen : Stone
    {
        internal Queen(string name, bool couldMove, EnumStoneColor color, Location location, Player player) : base(name, couldMove, color, location, player)
        {
        }

        public Queen(EnumStoneColor color, int x, int y) : base(string.Empty, true, color, SessionFactory.GetTable().GetLocation(x, y), null)
        {
        }

        public override List<Location> GetMovementLocations(Location target, Table table)
        {
            List<Location> result = null;

            if (CheckMove(target, table))
            {
                result = new List<Location>();

                int currentX = Location.X;
                int currentY = Location.Y;
                var span = target - Location;

                // çapraz gidiş
                if (span.XDiff == span.YDiff)
                {
                    for (int i = 1; i <= span.XDiff; i++)
                    {
                        currentX += span.XMovement == Direction.Forward ? 1 : -1;
                        currentY += span.YMovement == Direction.Forward ? 1 : -1;

                        result.Add(table.GetLocation(currentX, currentY));
                    }
                }

                // yatay-dikey gidiş
                else
                {
                    // dikey
                    if (span.XDiff == 0)
                    {
                        for (int i = 1; i <= span.YDiff; i++)
                        {
                            currentY += span.YMovement == Direction.Forward ? 1 : -1;

                            result.Add(table.GetLocation(currentX, currentY));
                        }
                    }

                    // yatay
                    else
                    {
                        for (int i = 1; i <= span.XDiff; i++)
                        {
                            currentX += span.XMovement == Direction.Forward ? 1 : -1;

                            result.Add(table.GetLocation(currentX, currentY));
                        }
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

            var targetLocationStone = table.Stones.GetFromLocation(target);
            var span = target - Location;

            // çapraz gidiş
            if (span.XDiff == span.YDiff)
            {
                var result = MovementRules.DiagonalCheck(Location, target, table);
                if (result == false)
                {
                    return result;
                }

                if (targetLocationStone != null)
                {
                    willEated = targetLocationStone;
                }
            }

            // yatay || dikey gidiş
            if (span.XDiff == 0 || span.YDiff == 0)
            {
                var result = MovementRules.HorizontalOrVerticalCheck(Location, target, table);
                if (result == false)
                {
                    return result;
                }

                if (targetLocationStone != null)
                {
                    willEated = targetLocationStone;
                }
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

            var span = target - Location;

            if (span.XDiff == 0 && span.YDiff == 0)
            {
                return false;
            }

            if (span.XDiff != 0 && span.YDiff != 0 && span.XDiff != span.YDiff)
            {
                return false;
            }

            return true;
        }
    }
}
