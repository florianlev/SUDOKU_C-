using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Persistance
{
    public class JeuSudokuPersistanceException : Exception
    {
        internal JeuSudokuPersistanceException(string msg) : base(msg)
        {

        }
    }
}
