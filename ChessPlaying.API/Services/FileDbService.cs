using ChessPlaying.API.Model.Domain;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessPlaying.API.Services
{
    public class FileDbService : IChessDbService
    {
        private string rootPath => Environment.CurrentDirectory;
        private string sessionPath => $@"{rootPath}\sessions";

        public Session CreateSession(string name, string sessionInfo)
        {
            var session = new Session()
            {
                Id = 0,
                SessionInfo = sessionInfo,
                SessionName = name
            };
            var serialized = Newtonsoft.Json.JsonConvert.SerializeObject(session);

            System.IO.File.WriteAllText($@"{sessionPath}\{name}.txt", serialized);
            return session;
        }

        public Session GetSession(string name)
        {
            var sessionInfo = System.IO.File.ReadAllText($@"{sessionPath}\{name}.txt");
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Session>(sessionInfo);
        }

        public Session UpdateSession(Session session)
        {
            var serialized = Newtonsoft.Json.JsonConvert.SerializeObject(session);

            System.IO.File.WriteAllText($@"{sessionPath}\{session.SessionName}.txt", serialized);
            return session;
        }

        public IEnumerable<object> GetSessions()
        {
            var directory = new System.IO.DirectoryInfo(sessionPath);

            return directory.GetFiles().Select(f => f.Name.Split('.')[0]);
        }

        public void DeleteSession(string name)
        {
            System.IO.File.Delete($@"{sessionPath}\{name}.txt");
        }
    }
}
