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
            var filePath = $@"{sessionPath}\{name}.txt";

            try
            {

                Console.WriteLine($"{System.IO.Directory.GetCurrentDirectory()} is exists?");
                Console.WriteLine(System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory()));

                Console.WriteLine("-----------------------");

                Console.WriteLine(@$"heroku_output\sessions\ is exists?");
                Console.WriteLine(System.IO.Directory.Exists(@$"heroku_output\sessions\"));

                Console.WriteLine("-----------------------");


                var directory = new System.IO.DirectoryInfo(@$"{System.IO.Directory.GetCurrentDirectory()}\sessions\");
                Console.WriteLine(directory.FullName);
                Console.WriteLine(directory.Exists);
                Console.WriteLine(directory.Name);
                Console.WriteLine("-----------------------");

                var file = new System.IO.FileInfo(@$"{System.IO.Directory.GetCurrentDirectory()}\sessions\{name}.txt");
                Console.WriteLine(file.FullName);
                Console.WriteLine(file.Exists);
                Console.WriteLine(file.Name);

                Console.WriteLine("-----------------------");
                Console.WriteLine($"file directory exists: {file.Directory.Exists}");

                if (file.Directory.Exists)
                {
                    foreach (var item in file.Directory.GetFiles())
                    {
                        Console.WriteLine(item.FullName);
                    }
                }

                Console.WriteLine("-----------------------");

                Console.WriteLine(@$"heroku_output\sessions\ is exists?");
                Console.WriteLine(System.IO.Directory.Exists(@$"heroku_output\sessions\"));

                //var stream = System.IO.File.Open(@$"{System.IO.Directory.GetCurrentDirectory()}\sessions\{name}.txt", System.IO.FileMode.Open);
                //Console.WriteLine("file opened");
                //stream.Close();
                //Console.WriteLine("file closed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

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
            var directory = new System.IO.DirectoryInfo(@$"{rootPath}/sessions");
            Console.WriteLine(directory.FullName);
            Console.WriteLine(directory.Exists);
            Console.WriteLine(directory.Name);

            return directory.GetFiles().Select(f => f.Name.Split('.')[0]);
        }

        public void DeleteSession(string name)
        {
            System.IO.File.Delete($@"{sessionPath}\{name}.txt");
        }
    }
}
