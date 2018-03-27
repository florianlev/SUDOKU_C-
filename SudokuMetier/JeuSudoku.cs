using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sudoku.Persistance;

namespace Sudoku.Metier
{
    public class JeuSudoku
    {
        private bool partieTerminee;
        private Grille grille;
        private Random generateur;
        private PartieTO partieSauvee;
        private int choix;

        private IGrilleDepartDAO daoGrilleDepart;
        private IPartieDAO daoPartie;
        private IGrilleResoluDAO daoGrilleResolu;

        private List<GrilleDepartTO> grilleDepart;
        private List<GrilleResoluTO> grilleResolu;
        private List<PartieTO> partiesTO;
        private List<Tuple<long, string>> listeParties;



        public JeuSudoku()
        {
            generateur = new Random();
            daoGrilleDepart = new JsonGrilleDepartDAO(@"grilles");
            daoPartie = new JsonPartieDAO(@"parties");
            daoGrilleResolu = new JsonGrilleResoluDAO(@"solutions");
            this.partieSauvee = null;
            this.partieTerminee = true;

        }


        public void nouvellePartie()
        {
            choix = 0;
            choix = generateur.Next(1, 5);
            List<GrilleDepartTO> grillesDepart = daoGrilleDepart.getAllTransfertObject();
            GrilleDepartTO g = grillesDepart[choix];
            grille = new Grille(g.Cases);

            this.partieTerminee = false;
            this.partieSauvee = null;
        }



        public void modifierCase(byte i, byte j, byte chiffre)
        {
            if (!this.partieTerminee)
                grille.modifierCase(i, j, chiffre);
        }


        public bool validerGrille()
        {
            if (!this.partieTerminee)
                this.partieTerminee = grille.estValide();

            if (this.partieTerminee && partieSauvee != null)
            {
                daoPartie.deleteTransfertObject(partieSauvee.Id);
                this.partieSauvee = null;
            }

            return this.partieTerminee;
        }


        public void resoudre()
        {
            if (!this.partieTerminee)
            {
                this.partieTerminee = true;
                List<GrilleResoluTO> grilleResolu = daoGrilleResolu.getAllTransfertObject();
                //GrilleResoluTO.g = grilleResolu[choix];
                grille.resoudre();
            }
        }


        public Grille Grille
        {
            get { return this.grille; }
        }


        public bool PartieTerminee
        {
            get { return this.partieTerminee; }
        }

        public void sauver()
        {
            if (partieTerminee)
            {
                throw new JeuSudokuException("La partie est terminée, elle ne peut être sauvée.");

            }
            if (this.partieSauvee == null)
            {
                this.partieSauvee = new PartieTO();
                this.partieSauvee.Date = DateTime.Now;
                partieSauvee.Cases = grille.getTuplesCases();
                daoPartie.insertTransfertObject(this.partieSauvee);


            }
            else
            {
            
                //implementer le reste
                this.partieSauvee.Date = DateTime.Now;
                this.partieSauvee.Cases = grille.getTuplesCases();
                daoPartie.updateTransfertObject(this.partieSauvee);
            }
        }

        public void reprendrePartie(long idPartie)
        {
            //this.partieSauvee = daoPartie.getTransfertObject(idPartie);

            Grille grille = new Grille(daoPartie.getTransfertObject(idPartie).Cases);
            

            this.partieTerminee = false;
        }

        public List<Tuple<long, string>> obtenirPartiesSauvegardees()
        {
            List<Tuple<long, string>> listeParties = new List<Tuple<long, string>>();

            List<PartieTO> partiesTO = daoPartie.getAllTransfertObject();

            foreach (PartieTO partie in partiesTO)
            {
                listeParties.Add(new Tuple<long, string>(partie.Id, partie.Date.ToString()));
            }

            return listeParties;
        }

    }
}
