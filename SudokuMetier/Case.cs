namespace Sudoku.Metier
{
    public class Case
    {
        private bool modifiable;
        private byte chiffre;
        private byte valeurMaxChiffre;


        internal Case(bool modifiable, byte chiffre, byte valeurMaxChiffre)
        {
            this.modifiable = modifiable;
            this.chiffre = chiffre;
            this.valeurMaxChiffre = valeurMaxChiffre;

            if (this.chiffre > this.valeurMaxChiffre)
                throw new JeuSudokuException("Valeur non valide pour la case (" +
                    this.chiffre + " est > que " + this.valeurMaxChiffre + ").");
        }




        public bool Modifable
        {
            get { return this.modifiable; }
        }


        public bool Vide
        {
            get { return this.chiffre == 0 ? true : false; }
        }


        public byte Chiffre
        {
            get { return this.chiffre; }

            internal set
            {
                if (!this.modifiable)
                    throw new JeuSudokuException("Case non modifiable");

                if (value > this.valeurMaxChiffre)
                    throw new JeuSudokuException("Valeur non valide pour la case (" +
                        value + " est > que " + this.valeurMaxChiffre + ").");

                this.chiffre = value;
            }
        }
    }
}
