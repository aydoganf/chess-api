using ChessPlaying.API.Model.Request;
using ChessPlaying.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Thrones.Gaming.Chess.SessionManagement;
using System.Collections.Generic;
using System.Linq;

namespace ChessPlaying.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChessController : ControllerBase
    {
        private readonly IChessDbService _chessDbService;

        public ChessController(IChessDbService chessDbService)
        {
            _chessDbService = chessDbService;
        }

        [HttpPost("create")]
        public SessionInformation Create([FromBody] CreateSessionRequest request)
        {
            var name = Guid.NewGuid().ToString();

            var session = SessionFactory
                .CreateOne<ChessAPISession>(name)
                .AddPlayers(request.Players[0].Nickname, request.Players[1].Nickname);

            var sessionInfo = Newtonsoft.Json.JsonConvert.SerializeObject(session.GetInformation());

            _chessDbService.CreateSession(name, sessionInfo);

            return session.GetInformation();
        }

        [HttpGet("ping")]
        public ActionResult<string> Ping()
        {
            return "Hi there!";
        }

        [HttpGet]
        public ActionResult<SessionInformation> GetSession([FromHeader(Name = "SessionName")] string sessionName)
        {
            if (string.IsNullOrEmpty(sessionName))
            {
                return Unauthorized();
            }

            var sessionInfo = _chessDbService.GetSession(sessionName).SessionInfo;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SessionInformation>(sessionInfo);
        }


        [HttpPost]
        public SessionInformation Command([FromBody] CommandRequest request, [FromHeader(Name = "SessionName")] string sessionName)
        {
            var session = _chessDbService.GetSession(sessionName);

            var chessSession = SessionFactory.CreateFrom(session.SessionInfo);
            var sessionInfo = chessSession.Command(request.Command);

            session.SessionInfo = Newtonsoft.Json.JsonConvert.SerializeObject(sessionInfo);
            _chessDbService.UpdateSession(session);

            return sessionInfo;
        }

        [HttpGet("sessions")]
        public IEnumerable<object> GetSessions()
        {
            List<object> sessions = new List<object>();
            var sessionNames = _chessDbService.GetSessions();

            if (sessionNames == null)
            {
                return sessions;
            }

            foreach (var sessionName in sessionNames)
            {
                var session = _chessDbService.GetSession(sessionName.ToString());
                var chessSession = SessionFactory.CreateFrom(session.SessionInfo);

                var players = chessSession.GetPlayerInformations();
                var title = string.Join(" vs ", players.Select(p => p.Nickname));

                sessions.Add(new
                {
                    Title = title,
                    Players = players,
                    SessionName = session.SessionName
                });
            }

            return sessions;
        }

        [HttpDelete("{sessionName}")]
        public bool DeleteSession(string sessionName)
        {
            _chessDbService.DeleteSession(sessionName);

            return true;
        }
    }
}
