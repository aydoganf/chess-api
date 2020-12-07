using System.Collections.Generic;
using Thrones.Gaming.Chess.Logging;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.SessionManagement
{
    public interface ISession
    {
        Table Table { get; }

        void Start();

        ISession AddPlayers(string blackPlayerNickname, string whitePlayerNickname);

        ISession AddPlayer(string nickname, EnumStoneColor color, List<IStone> stones);

        ISession AddPlayer(Player player);

        ISession AddStartingCommands(string[] commands);

        ISession SetLogger(ILogger logger);

        ISession SetIndexer(int currentIndexer);

        SessionInformation Command(string command);

        SessionInformation GetInformation();
    }
}