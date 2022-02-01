using ProjectH_KM.DataAccess.Models.IKM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectH_KM.DataAccess.Models.KM
{
    public class Contractor : IRecord
    {
        public String Id { get; set; }
        public String ModuleId { get; set; }
        public String Nazwa { get; set; }
        public String NIP { get; set; }
        public String Adres { get; set; }
        public String Email { get; set; }
        public String PESEL { get; set; }
        public String KodKraju { get; set; }
        public Boolean Rodzic { get; set; }
    }
}
