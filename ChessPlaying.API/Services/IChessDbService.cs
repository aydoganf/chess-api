using ChessPlaying.API.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessPlaying.API.Services
{
    public interface IChessDbService
    {
        Session CreateSession(string name, string sessionInfo);
        Session GetSession(string name);
        Session UpdateSession(Session session);
        IEnumerable<object> GetSessions();

        void DeleteSession(string name);
    }
}
