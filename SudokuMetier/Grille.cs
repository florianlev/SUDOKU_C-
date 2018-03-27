using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Metier
{
    public class Grille
    {

        private const byte DIMENSION = 9;
        private Case[,] cases;
        private List<Tuple<byte, bool>> tuplesCases;

        internal Grille(List<byte> cases)
        {
            this.cases = new Case[DIMENSION, DIMENSION];
            byte chiffre;
            byte k = 0;
            tuplesCases = new List<Tuple<byte, bool>>();
            for (int i = 0; i < DIMENSION; i++)
            {
                for (int j = 0; j < DIMENSION; j++)
                {
                    chiffre = cases[k];
                    this.cases[i, j] = new Case(chiffre == 0 ? true : false, chiffre, DIMENSION);
                    k++;
                }
            }
        }

        internal Grille(List<Tuple<byte, bool>> cases)
        {
            this.cases = new Case[DIMENSION, DIMENSION];
            byte chiffre;
            byte k = 0;
            tuplesCases = new List<Tuple<byte, bool>>();
            for (int i = 0; i < DIMENSION; i++)
            {
                for (int j = 0; j < DIMENSION; j++)
                {
                    chiffre = cases[k].Item1;
                    this.cases[i, j] = new Case(cases[i+j].Item2, chiffre, DIMENSION);
                    k++;
                }
            }
        }

        
        public Case[,] Cases
        {
            get { return this.cases; }
        }


        internal void modifierCase(byte i, byte j, byte chiffre)
        {
            if (i >= DIMENSION || j >= DIMENSION)
                throw new JeuSudokuException("Coordonnées de case non valide.");

            this.cases[i, j].Chiffre = chiffre;
        }


        internal bool estValide()
        {
            // Nous vérifions que toutes les cases possèdent une valeur
            foreach (Case c in this.cases)
            {
                if (c.Vide)
                    return false;
            }

            // Utilisation d'un ensemble pour la validation des doublons
            HashSet<byte> set = new HashSet<byte>();

            // On regarde s'il y a des doublons sur les lignes
            for (int i = 0; i < DIMENSION; i++)
            {
                set.Clear();
                for (int j = 0; j < DIMENSION; j++)
                {
                    if (set.Add(this.cases[i, j].Chiffre) == false)
                        return false;
                }
            }

            // On regarde s'il y a des doublons sur les colonnes
            for (int j = 0; j < DIMENSION; j++)
            {
                set.Clear();
                for (int i = 0; i < DIMENSION; i++)
                {
                    if (set.Add(this.cases[i, j].Chiffre) == false)
                        return false;
                }
            }

            // On regarde s'il y a des doublons dans les "sous-grille"
            // Parcours de sous-grille en sous-grille
            for (int i = 0; i < DIMENSION; i += 3)
            {
                for (int j = 0; j < DIMENSION; j += 3)
                {
                    // Parcours d'une sous-grille
                    set.Clear();
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            if (set.Add(this.cases[i + k, j + l].Chiffre) == false)
                                return false;
                        }
                    }
                }
            }

            return true;
        }


        internal void resoudre()
        {
            // On remet à l'état initial la grille
            foreach (Case c in this.cases)
                if (c.Modifable)
                    c.Chiffre = 0;

            if (!resoudre(0))
                throw new JeuSudokuException("La grille ne possède pas de solution!");

            if (!estValide())
                throw new JeuSudokuException("L'algorithme de résolution fournie une solution non valide... WTF!");
        }


        /// <summary>
        /// Inspiré de : https://openclassrooms.com/courses/le-backtracking-par-l-exemple-resoudre-un-sudoku
        /// </summary>
        private bool resoudre(int position)
        {
            // On a résolu la grille, on arrête
            if (position == DIMENSION * DIMENSION)
                return true;

            // On transforme la position "linéaire" en i,j (multidimensions)
            int i = position / DIMENSION;
            int j = position % DIMENSION;

            // Si la case n'est pas vide, on passe à la prochaine
            // On est face à une case "non modifiable".
            if (!this.cases[i, j].Vide)
                return resoudre(position + 1);

            // Nous sommes faces à une case vide, donc il faut trouver un chiffre possible.
            // C'est ici que la "magie" du backtracking prend place. Pour une case donnée,
            // nous trouvons la première possibilité de chiffre valide. Ensuite, nous 
            // enchaînons avec la prochaine case (resoudre(position++)). Si l'algorithme
            // n'arrive pas à trouver un chiffre valide à la prochaine case avec l'état
            // actuel de la grille, alors nous revenons à l'appel d'avant et nous tentons
            // de trouver le prochain chiffre valide et rappelons encore resoudre(position++).
            // Ce pattern ce répète jusqu'à la résolution de la grille complète ou bien après
            // avoir essayé toute les possibilités et finalement retourner false pour indiquer
            // que la grille n'est pas résolvable...
            for (byte chiffre = 1; chiffre <= DIMENSION; chiffre++)
            {
                if (absentSurLigne(i, chiffre) && absentSurColonne(j, chiffre)
                    && absentDansSousGrille(i, j, chiffre))
                {
                    this.cases[i, j].Chiffre = chiffre;

                    // On passe à la prochaine case
                    if (resoudre(position + 1))
                        return true;
                }
            }

            // On a rien trouvé, on remet la case à "vide"
            this.cases[i, j].Chiffre = 0;

            return false;
        }


        private bool absentSurLigne(int i, byte chiffre)
        {
            for (int j = 0; j < DIMENSION; j++)
                if (this.cases[i, j].Chiffre == chiffre)
                    return false;
            return true;
        }


        private bool absentSurColonne(int j, byte chiffre)
        {
            for (int i = 0; i < DIMENSION; i++)
                if (this.cases[i, j].Chiffre == chiffre)
                    return false;
            return true;
        }


        private bool absentDansSousGrille(int i, int j, byte chiffre)
        {
            // On trouve les i/j de départ de la sous-grille puisque i/j 
            // en paramètre n'est pas nécessairement la première case de
            // la sous-grille
            int id = i - (i % 3);
            int jd = j - (j % 3);

            // Parcours de la sous-grille à la recherche du chiffre
            for (i = id; i < id + 3; i++)
                for (j = jd; j < jd + 3; j++)
                    if (this.cases[i, j].Chiffre == chiffre)
                        return false;

            return true;
        }

        internal List <Tuple<byte,bool>> getTuplesCases()
        {
            tuplesCases.Clear();
            

            for (int i = 0; i < DIMENSION; i++)
            {
                for (int j = 0; j < DIMENSION; j++)
                {
                    tuplesCases.Add(new Tuple<byte, bool>(cases[i, j].Chiffre, cases[i, j].Modifable));
                }
            }
            
            return tuplesCases;
            //get { return this.tuplesCases; } // a corriger
        }
    }
}