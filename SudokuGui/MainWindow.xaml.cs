using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sudoku.Metier;

namespace Sudoku.Gui
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private JeuSudoku jeu;

        public MainWindow()
        {
            InitializeComponent();
            jeu = new JeuSudoku();
        }

        /// <summary>
        /// À appeler seulement en début de nouvelle partie.
        /// La méthode retire les Textbox présent et
        /// rajoute les nouveaux textbox (modifiable et non modiable).
        /// Elle ajoute également un événement aux texbox modifaibles.
        /// </summary>
        private void dessinerGrilleNouvellePartie()
        {
            int ligne, colonne;
            Border b;
            TextBox tb;
            Case c;
            for (int i = 0; i < this.grilleSudoku.Children.Count; i++)
            {
                // On récupère la case du jeu
                ligne = (i / 9);
                colonne = (i % 9);
                c = jeu.Grille.Cases[ligne, colonne];

                // Construction de la Textbox
                tb = new TextBox();
                tb.Name = "_" + ligne + "_" + colonne;
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.BorderThickness = new Thickness(0);
                tb.Width = 20;
                tb.FontSize = 23;
                tb.CaretBrush = Brushes.Black;

                b = (Border)this.grilleSudoku.Children[i];
                if (c.Modifable)
                {
                    b.Background = new SolidColorBrush(Colors.Beige);
                    tb.Background = new SolidColorBrush(Colors.Beige);
                    tb.KeyUp += new KeyEventHandler(modifierChiffre);
                }
                else
                {
                    b.Background = new SolidColorBrush(Colors.Gold);
                    tb.Background = new SolidColorBrush(Colors.Gold);
                    tb.IsEnabled = false;
                }

                b.Child = tb;
            }

            majTextBoxGrille();
        }


        /// <summary>
        /// Cette méthode met à jour le contenu des
        /// textbox pour reflèter l'état interne de
        /// la grille.
        /// </summary>
        private void majTextBoxGrille()
        {
            int ligne, colonne;
            TextBox tb;
            Case c;
            for (int i = 0; i < this.grilleSudoku.Children.Count; i++)
            {
                ligne = (i / 9);
                colonne = (i % 9);
                c = jeu.Grille.Cases[ligne, colonne];

                tb = (TextBox)((Border)this.grilleSudoku.Children[i]).Child;

                tb.Text = c.Vide ? "" : "" + c.Chiffre;
            }
        }


        /// <summary>
        /// Méthode gérant l'événement lorsqu'une entrée est
        /// faite dans une des cases (textbox) de la grille
        /// </summary>
        private void modifierChiffre(object sender, KeyEventArgs args)
        {
            if (args.Key == Key.Tab)
                return;

            if (!jeu.PartieTerminee)
            {
                TextBox txtBox = (TextBox)sender;
                try
                {
                    byte chiffre;
                    if (txtBox.Text.Equals(""))
                    {
                        chiffre = 0;
                    }
                    else
                    {
                        chiffre = Convert.ToByte(txtBox.Text);
                    }

                    try
                    {
                        string[] separateur = { "_" };
                        string[] indices = txtBox.Name.Split(separateur, StringSplitOptions.RemoveEmptyEntries);
                        jeu.modifierCase(Convert.ToByte(indices[0]), Convert.ToByte(indices[1]), chiffre);
                    }
                    catch (JeuSudokuException jse)
                    {
                        MessageBox.Show(jse.Message);
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Les lettres ne sont pas permises!");
                }
                catch (OverflowException)
                {
                    // Puisque nous convertissons en byte, si le texte représente
                    // un nombre négatif ou quelque chose de plus grand que 255,
                    // alors OverflowException est lancée.
                    MessageBox.Show("Il faut entrer un chiffre valide!");
                }

                majTextBoxGrille();
            }
            else
            {
                MessageBox.Show("Aucune partie en cours. Veuillez débuter une nouvelle partie.");
            }
        }


        private void valider_Click(object sender, RoutedEventArgs e)
        {
            if (!jeu.PartieTerminee)
            {
                if (jeu.validerGrille())
                {
                    MessageBox.Show("Super! Partie terminée");
                    this.msgEtatPartie.Text = "Partie terminée.";
                }
                else
                {
                    MessageBox.Show("La grille est non valide. Veuillez corriger vos erreurs et revalidez.");
                }
            }
            else
            {
                MessageBox.Show("Impossible de valider : aucune partie en cours...");
            }
        }


        private void resoudre_Click(object sender, RoutedEventArgs e)
        {
            if (!jeu.PartieTerminee)
            {
                if (MessageBox.Show("Voulez-vous vraiment résoudre la grille?",
                    "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    this.jeu.resoudre();
                    this.majTextBoxGrille();
                    this.msgEtatPartie.Text = "Partie terminée.";
                }
            }
            else
            {
                MessageBox.Show("Impossible de résoudre : aucune partie en cours...");
            }
        }

        private void sauver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.jeu.sauver();
                MessageBox.Show("Partie sauvegardée.");
            }
            catch (JeuSudokuException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void reprendre_Click(object sender, RoutedEventArgs e)
        {
            DialogueReprendrePartie dialogue = new DialogueReprendrePartie(jeu.obtenirPartiesSauvegardees());
            dialogue.Owner = this;
            dialogue.ShowDialog();

            if (dialogue.Id != -1)
            {
                jeu.reprendrePartie(dialogue.Id);
                dessinerGrilleNouvellePartie();

            }
        }

        private void nouvellePartie_Click(object sender, RoutedEventArgs e)
        {
            if (jeu.PartieTerminee)
            {
                jeu.nouvellePartie();
                dessinerGrilleNouvellePartie();
                this.msgEtatPartie.Text = "Partie en cours...";
            }
            else
            {
                if (MessageBox.Show("Vous avez une partie en cours. Voulez-vous vraiment en débuter une nouvelle ?",
                    "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    jeu.nouvellePartie();
                    dessinerGrilleNouvellePartie();
                    this.msgEtatPartie.Text = "Partie en cours...";
                }
            }
        }


        private void quitter_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Voulez-vous vraiment quitter l'application?",
                    "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}