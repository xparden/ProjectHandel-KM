using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using itDesk.Debug;
using ProjectH_KM.DataAccess.Loader;

namespace ProjectH_KM.API.SelfHost
{
    class Program
    {
        private static Task _serviceTask;
        static void Main(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    break;
                case 1:
                    switch (args[0].ToUpper())
                    {
                        case "-A":
                            try
                            {
                                Log.Create("Rozpoczynanie uruchamiania aplikacji.", LogType.Information);
                                var address = ConfigurationManager.AppSettings["Address"];
                                var port = ConfigurationManager.AppSettings["Port"];

                                var config = new HttpSelfHostConfiguration($"{address}:{port}");

                                config.EnableCors();

                                config.MapHttpAttributeRoutes();

                                config.Routes.MapHttpRoute("ActionApi", "api/{controller}/{action}",
                                    new { id = RouteParameter.Optional });
                                Log.Create("Pomyślnie przetworzono plik konfiguracyjny.", LogType.Information);
                                ModuleLoader.Load();
                                

                                var server = new HttpSelfHostServer(config);
                                _serviceTask = server.OpenAsync();
                                _serviceTask.Wait();
                                Log.Create("Zakończono uruchamianie aplikacji.", LogType.Information);
                                Log.Create("Server Up.", LogType.Information);
                                Console.ReadKey();
                            
                            }
                            catch (Exception exception)
                            {
                                Log.Create($"Wystąpił błąd podczas inicjalizacji usługi Rest API., {exception.Message}", LogType.Error);
                            }
                            break;
                        case "-I":
                            Log.Create("Rozpoczęto instalowanie usługi Windows.", LogType.Information);

                            break;
                        case "-U":
                            Log.Create("Rozpoczęto odinstalowywanie usługi Windows", LogType.Information);

                            break;
                    }
                    break;
            }
        }
    }
}
