using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Persistance
{
    public class GrilleResoluTO
    {

        private long id;

        private List<byte> cases;

        public long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }


        public List<byte> Cases
        {
            get { return this.cases; }
            set { this.cases = value; }

        }


    }
}
