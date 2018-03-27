using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sudoku.Persistance
{
    public abstract class JsonDAO
    {
        protected string cheminRepertoire;


        protected JsonDAO(string cheminRepertoire)
        {
            this.cheminRepertoire = cheminRepertoire;

            if (!Directory.Exists(cheminRepertoire))
                throw new JeuSudokuPersistanceException("Le chemin du répertoire pour le JsonDAO n'est pas valide.");
        }



    }
}
