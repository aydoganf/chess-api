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
        private string sessionPath => $@"{Environment.CurrentDirectory}\sessions";
        private string sessionsTxt => $@"{sessionPath}\sessions.txt";
        private char sessionSplitter = ',';

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

            var sessions = GetSessions();
            var prefix = ",";
            if (sessions == null) prefix = "";

            System.IO.File.AppendAllText(sessionsTxt, $"{prefix}{name}");
            return session;
        }

        public Session GetSession(string name)
        {
            var filePath = $@"{sessionPath}\{name}.txt";

            #region old

            //try
            //{

            //    Console.WriteLine($"{System.IO.Directory.GetCurrentDirectory()} is exists?");
            //    Console.WriteLine(System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory()));

            //    Console.WriteLine("-----------------------");

            //    Console.WriteLine(@$"heroku_output\sessions\ is exists?");
            //    Console.WriteLine(System.IO.Directory.Exists(@$"heroku_output\sessions\"));

            //    Console.WriteLine("-----------------------");


            //    var directory = new System.IO.DirectoryInfo(@$"{System.IO.Directory.GetCurrentDirectory()}\sessions\");
            //    Console.WriteLine(directory.FullName);
            //    Console.WriteLine(directory.Exists);
            //    Console.WriteLine(directory.Name);
            //    Console.WriteLine("-----------------------");

            //    var file = new System.IO.FileInfo(@$"{System.IO.Directory.GetCurrentDirectory()}\sessions\{name}.txt");
            //    Console.WriteLine(file.FullName);
            //    Console.WriteLine(file.Exists);
            //    Console.WriteLine(file.Name);

            //    Console.WriteLine("-----------------------");
            //    Console.WriteLine($"file directory exists: {file.Directory.Exists}");

            //    if (file.Directory.Exists)
            //    {
            //        foreach (var item in file.Directory.GetFiles())
            //        {
            //            Console.WriteLine(item.FullName);
            //        }
            //    }

            //    Console.WriteLine("-----------------------");

            //    Console.WriteLine(@$"heroku_output\sessions\ is exists?");
            //    Console.WriteLine(System.IO.Directory.Exists(@$"heroku_output\sessions\"));

            //    //var stream = System.IO.File.Open(@$"{System.IO.Directory.GetCurrentDirectory()}\sessions\{name}.txt", System.IO.FileMode.Open);
            //    //Console.WriteLine("file opened");
            //    //stream.Close();
            //    //Console.WriteLine("file closed");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}

            #endregion
            var sessionInfo = System.IO.File.ReadAllText(filePath);
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
            var sessions = System.IO.File.ReadAllText(sessionsTxt);

            return string.IsNullOrEmpty(sessions) ? null : sessions.Split(sessionSplitter);
        }

        public void DeleteSession(string name)
        {
            var sessions = System.IO.File.ReadAllText(sessionsTxt);

            var list = sessions.Split(sessionSplitter).ToList();
            list.Remove(name);

            var newSessions = string.Join(sessionSplitter, list);
            System.IO.File.WriteAllText(sessionsTxt, newSessions);

            System.IO.File.Delete($@"{sessionPath}\{name}.txt");
        }

        public void Initialize()
        {
            System.IO.Directory.CreateDirectory(sessionPath);
            System.IO.File
                .Create(sessionsTxt)
                .Close();
        }
    }
}
