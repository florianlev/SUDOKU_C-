using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Persistance
{
    public interface IDAO<T>
    {
        T getTransfertObject(long id);
        List<T> getAllTransfertObject();
        void insertTransfertObject(T to);
        void updateTransfertObject(T to);
        void deleteTransfertObject(long id);

    }
}
