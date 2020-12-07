using System;
using System.Collections.Generic;
using System.Text;
using Thrones.Gaming.Chess.Movement;

namespace Thrones.Gaming.Chess.Logging
{
    public interface ILogger
    {
        void SetFileName(string fileName);

        void SaveInstructions(IEnumerable<Instruction> instructions);

        IEnumerable<string> ReadInstructions();
    }
}
