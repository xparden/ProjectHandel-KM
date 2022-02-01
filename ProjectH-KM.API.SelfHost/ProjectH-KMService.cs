using itDesk.Debug;
using ProjectH_KM.DataAccess.Loader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace ProjectH_KM.API.SelfHost
{
    partial class ProjectH_KM : ServiceBase
    {
        private Task _serviceTask;
        public ProjectH_KM()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
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
                Log.Create("Pomyślnie załadowano biblioteki Soneta.", LogType.Information);

                var server = new HttpSelfHostServer(config);
                _serviceTask = server.OpenAsync();
                _serviceTask.Wait();
                Log.Create("Zakończono uruchamianie aplikacji.", LogType.Information);
                Log.Create("Server Up.", LogType.Information);
            }
            catch (Exception exception)
            {
                Log.Create($"Wystąpił błąd podczas inicjalizacji usługi Rest API., {exception.Message}", LogType.Error);
            }
        }

        protected override void OnStop()
        {
            Log.Create("Usługa zostanie zatrzymana.", LogType.Information);
        }
    }
}
