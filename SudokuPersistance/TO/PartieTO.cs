using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Persistance
{
    public class PartieTO
    {

        private long id;
        private DateTime date;
        private List<Tuple<byte, bool>> cases;

        public long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public DateTime Date
        {
            get { return this.date; }
            set { this.date = value; }
        }

        public List<Tuple<byte, bool>> Cases
        {
            get { return this.cases; }
            set { this.cases = value; }

        }


    }
}
