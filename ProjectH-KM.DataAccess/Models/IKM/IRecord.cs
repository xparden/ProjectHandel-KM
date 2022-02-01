using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectH_KM.DataAccess.Models.IKM
{
    public interface IRecord
    {
        String Id { get; set; }
        String ModuleId { get; set; }
    }
}
