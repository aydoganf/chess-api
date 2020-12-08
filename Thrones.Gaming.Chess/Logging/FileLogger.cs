using System.Collections.Generic;
using System.Linq;
using Thrones.Gaming.Chess.Movement;

namespace Thrones.Gaming.Chess.Logging
{
    public class FileLogger : ILogger
    {
        private readonly string filePath;
        private string fileName;

        private string fileFullPath => $"{filePath}\\{fileName}";

        public FileLogger(string filePath)
        {
            this.filePath = filePath;
        }

        public IEnumerable<string> ReadInstructions()
        {
            return System.IO.File.ReadAllLines(fileFullPath);
        }

        public void SaveInstructions(IEnumerable<Instruction> instructions)
        {
            System.IO.File.AppendAllLines(fileFullPath, instructions.Select(i => i.Log));
        }

        public void SetFileName(string fileName)
        {
            this.fileName = fileName;
        }
    }
}
