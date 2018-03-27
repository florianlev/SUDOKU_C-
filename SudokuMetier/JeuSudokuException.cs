using System;

namespace Sudoku.Metier
{
    public class JeuSudokuException:Exception
    {
        internal JeuSudokuException(string msg)
            : base(msg)
        { }
    }
}
