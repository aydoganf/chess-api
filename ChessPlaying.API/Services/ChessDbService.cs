using ChessPlaying.API.Model.Domain;
using ChessPlaying.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessPlaying.API.Services
{
    public class ChessDbService : IChessDbService
    {
        private readonly ChessDbContext _chessDbContext;

        public ChessDbService(ChessDbContext chessDbContext)
        {
            _chessDbContext = chessDbContext;
        }


        public Session CreateSession(string name, string sessionInfo)
        {
            var session = new Session()
            {
                SessionInfo = sessionInfo,
                SessionName = name
            };

            _chessDbContext.Sessions.Add(session);
            _chessDbContext.SaveChanges();
            return session;
        }

        public Session GetSession(string name)
        {
            return _chessDbContext.Sessions.FirstOrDefault(s => s.SessionName == name);
        }

        public Session UpdateSession(Session session)
        {
            var dbSession = GetSession(session.SessionName);
            dbSession.SessionInfo = session.SessionInfo;
            _chessDbContext.Sessions.Update(dbSession);
            _chessDbContext.SaveChanges();

            return dbSession;
        }
    }
}
