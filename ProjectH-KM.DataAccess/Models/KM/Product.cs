using ProjectH_KM.DataAccess.Models.IKM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectH_KM.DataAccess.Models.KM
{
    public class Product : IRecord
    {
        public String Id { get; set; }
        public String Guid { get; set; }
        public String ModuleId { get; set; }
        public String Kod { get; set; }
        public String Nazwa { get; set; }
        public String EAN { get; set; }
        public String DefinicjaStawki { get; set; }
    }
}
