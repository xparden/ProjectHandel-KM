using itDesk.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectH_KM.DataAccess.Loader
{
    public class ModuleLoader
    {
        public static bool Load()
        {
            try
            {
                var loader = new Soneta.Start.Loader
                {
                    WithExtensions = true,
                    WithUI = false
                };

                loader.Load();
                Log.Create("Pomyślnie załadowano biblioteki Soneta.", LogType.Information);
                return true;
            }
            catch (Exception exception)
            {
                itDesk.Debug.Log.Create($"Wystąpił błąd podczas ładowania bibliotek Soneta, {exception.Message}", LogType.Error);
                return false;
            }
        }
    }
}
