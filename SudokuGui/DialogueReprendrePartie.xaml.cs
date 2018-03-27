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
using System.Windows.Shapes;

namespace Sudoku.Gui
{
    /// <summary>
    /// Logique d'interaction pour DialogueReprendrePartie.xaml
    /// </summary>
    public partial class DialogueReprendrePartie : Window
    {
        private long id = -1;

        public DialogueReprendrePartie(List<Tuple<long, string>> parties)
        {
            InitializeComponent();

            foreach (Tuple<long, string> partie in parties)
            {
                listeParties.Items.Add(partie);
            }
        }
        private void reprendre_Click(object sender, RoutedEventArgs e)
        {
            Tuple<long, string> partie = (Tuple<long, string>)listeParties.SelectedItem;

            if (partie != null)
            {
                this.id = partie.Item1;
            }

            this.Close();
        }


        private void annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        public long Id
        {
            get { return this.id; }
        }
    }
}
