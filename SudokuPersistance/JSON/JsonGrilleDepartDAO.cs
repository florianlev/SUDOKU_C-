using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Sudoku.Persistance
{
    public class JsonGrilleDepartDAO : JsonDAO, IGrilleDepartDAO
    {
        public JsonGrilleDepartDAO(string cheminRepertoire) :
            base(cheminRepertoire)
        {
        }
        public GrilleDepartTO getTransfertObject(long id)
        {
            StreamReader sr = new StreamReader(this.cheminRepertoire + @"\" + id + ".json", Encoding.Default);

            string json = sr.ReadToEnd();
            GrilleDepartTO partie = JsonConvert.DeserializeObject<GrilleDepartTO>(json);

            sr.Dispose();

            return partie;
        }


        public List<GrilleDepartTO> getAllTransfertObject()
        {
            List<GrilleDepartTO> parties = new List<GrilleDepartTO>();

            string[] fichiers = Directory.GetFiles(this.cheminRepertoire);

            StreamReader sr;
            string json;
            foreach (string f in fichiers)
            {
                sr = new StreamReader(f, Encoding.Default);
                json = sr.ReadToEnd();

                GrilleDepartTO partie = JsonConvert.DeserializeObject<GrilleDepartTO>(json);
                parties.Add(partie);

                sr.Dispose();
            }

            return parties;
        }


        public void insertTransfertObject(GrilleDepartTO to)
        {
            to.Id = DateTime.Now.Ticks;
            updateTransfertObject(to);
        }


        public void deleteTransfertObject(long id)
        {
            File.Delete(this.cheminRepertoire + @"\" + id + ".json");
        }


        public void updateTransfertObject(GrilleDepartTO to)
        {
            string json = JsonConvert.SerializeObject(to, Formatting.Indented);

            StreamWriter sw = new StreamWriter(this.cheminRepertoire + @"\" + to.Id + ".json", false, Encoding.Default);
            sw.Write(json);

            sw.Dispose();
        }
    }
}
