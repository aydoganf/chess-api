using System.Collections.Generic;
using Thrones.Gaming.Chess.Coordinate;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.SessionManagement;

namespace Thrones.Gaming.Chess.Stones
{
    public interface IStone
    {
        EnumStoneColor Color { get; }
        string NameWithColorPrefix { get; }
        string Name { get; }
        Location Location { get; }
        Location GhostLocation { get; }
        Location StoredLocation { get; }
        Player Player { get; }
        int MoveCount { get; }

        void SetPlayer(Player player);

        bool Move(Location target, Table table, out IStone eated);
        bool TryMove(Location target, Table table, out IStone willEated);

        void GhostMove(Location target);
        void UndoGhost();
        void ForceMove(Location target);
        List<Location> GetMovementLocations(Location target, Table table);
    }
}
