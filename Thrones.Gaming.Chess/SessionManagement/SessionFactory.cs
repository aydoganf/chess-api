using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Thrones.Gaming.Chess.Players;
using Thrones.Gaming.Chess.Stones;

namespace Thrones.Gaming.Chess.SessionManagement
{
    public static class SessionFactory
    {
        private static ISession session;

        public static ISession CreateOne<TSession>(string name) where TSession : Session
        {
            return CreateOne(typeof(TSession), name);
        }

        public static ISession CreateOne(Type type, string name)
        {
            var newSession = (Session)Activator.CreateInstance(type);

            newSession.SetName(name);
            session = newSession;

            return session;
        }

        public static ISession CreateFrom(string sessionInfo)
        {
            var sessionInformation = JsonConvert.DeserializeObject<SessionInformation>(sessionInfo);

            var newSession = CreateOne(Assembly.GetCallingAssembly().GetType(sessionInformation.FullTypeName), sessionInformation.Name);
            

            foreach (var player in sessionInformation.Players)
            {
                EnumStoneColor color = sessionInformation.Players.First() == player ? EnumStoneColor.Black : EnumStoneColor.White;

                var builder = PlayerFactory.CreateOne(player.Nickname, color);

                foreach (var stone in sessionInformation.Table.Stones.Where(s => s.GetStoneColor() == color))
                {
                    builder.AddStone(stone);
                }

                builder.Build(newSession);
            }

            newSession.SetIndexer(sessionInformation.CurrentIndexer);
            return newSession;
        }

        public static Table GetTable() => session.Table;
    }
}
