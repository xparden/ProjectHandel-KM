using itDesk.Debug;
using ProjectH_KM.DataAccess.Models.KM;
using Soneta.Business;
using Soneta.Business.App;
using Soneta.Core;
using Soneta.CRM;
using Soneta.Handel;
using Soneta.Towary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectH_KM.DataAccess.Repository
{
    public class KMRepository
    {
        public static List<Product> GetProducts(string _database, string _login, string _password)
        {
            itDesk.Debug.Log.Create("Rozpoczęcie pobierania produktów", LogType.Information);
            try
            {
                Database database = BusApplication.Instance[_database];
                Login login = database.Login(false, _login, _password);

                List<Product> products = new List<Product>();
                var dokumentyHandlowe = new List<DokumentHandlowy>();

                using (Session session = login.CreateSession(true, false, "Getproducts"))
                {
                    var handelModule = HandelModule.GetInstance(session);

                    int countOfKontrahents = handelModule.Towary.Towary.WgKodu.Count();

                    var list = handelModule.Towary.Towary.WgKodu;

                    foreach (var item in list)
                    {
                        Product product = new Product();

                        product.Id = item.ID.ToString();
                        product.Guid = item.Guid.ToString();
                        product.Kod = item.Kod;
                        product.Nazwa = item.Nazwa;
                        product.EAN = item.EAN;
                        product.DefinicjaStawki = item.DefinicjaStawki.ToString();

                        products.Add(product);
                        // itDesk.Debug.Log.Create("Pomyślnie wyeksportowano dane towaru o id " + product.Id, LogType.Information);
                    }
                }
                itDesk.Debug.Log.Create("Pomyślnie wyeksportowano dane dotyczące towarów.", LogType.Information);
                return products;
            }
            catch (Exception exception)
            {
                itDesk.Debug.Log.Create($"Wystąpił błąd eksportowania towarów, {exception.Message}", LogType.Error);
                return new List<Product>();
            }
        }

        public static bool AddProducts(string _database, string _login, string _password, List<Product> products)
        {
            itDesk.Debug.Log.Create("Rozpoczęcie dodawania towarów", LogType.Information);
            try
            {
                Database database = BusApplication.Instance[_database];
                Login login = database.Login(false, _login, _password);
                foreach (Product product in products)
                {
                    using (Session session = login.CreateSession(false, false, "ImportProducts"))
                    {
                        var handelModule = HandelModule.GetInstance(session);
                        var crmModule = CRMModule.GetInstance(session);

                        Towar towar = null;
                        try
                        {
                            towar = handelModule.Towary.Towary.WgKodu[product.Kod];
                        }
                        catch (Exception exception)
                        {
                            itDesk.Debug.Log.Create($"Wystąpił błąd podczas dodawania produktów, {exception.Message}", LogType.Error);
                        }

                        using (ITransaction transaction = session.Logout(true))
                        {
                            if (towar == null)
                            {
                                towar = new Towar();

                                handelModule.Towary.Towary.AddRow(towar);

                            }
                            towar.Nazwa = product.Nazwa;
                            towar.EAN = product.EAN;
                            towar.Kod = product.Kod;
                            towar.DefinicjaStawki = CoreModule.GetInstance(session).DefStawekVat.WgKodu["23%"];
                            towar.DefinicjaStawkiZakupu = CoreModule.GetInstance(session).DefStawekVat.WgKodu["23%"];
                            transaction.Commit();
                        }
                        session.Save();
                    }
                }
                itDesk.Debug.Log.Create("Dodawanie towarów zakończone", LogType.Information);
                return true;
            }
            catch(Exception exception)
            {
                itDesk.Debug.Log.Create($"Wystąpił błąd dodawania towarów, {exception.Message}", LogType.Error);
                return false;
            }
        }
        public static List<Contractor> GetContractors(string _database, string _login, string _password)
        {
            itDesk.Debug.Log.Create("Rozpoczęcie operacji pobierania Kontrahentów", LogType.Information);
            try
            {
                Database database = BusApplication.Instance[_database];
                Login login = database.Login(false, _login, _password);

                List<Contractor> contractors = new List<Contractor>();
                var dokumentyHandlowe = new List<DokumentHandlowy>();
                using (Session session = login.CreateSession(true, false, "GetContractors"))
                {
                    var crmModule = CRMModule.GetInstance(session);

                    int countOfKontrahents = crmModule.Kontrahenci.WgKodu.Count();

                    var list = crmModule.Kontrahenci.WgKodu;
                    foreach (var item in list)
                    {
                        Contractor contractor = new Contractor();

                        contractor.Adres = item.Adres.ToString();
                        contractor.NIP = item.NIP;
                        contractor.Nazwa = item.Nazwa;
                        contractor.Email = item.EMAIL;
                        contractor.KodKraju = item.KodKraju;
                        contractor.Id = item.Kod;
                        contractor.PESEL = item.PESEL;

                        contractors.Add(contractor);

                       // KMKontrahent.Logger.Log("Pomyślnie wyeksportowano dane kontrahenta o id " + contractor.Id, LogType.Information);
                    }
                }
                itDesk.Debug.Log.Create("Pobieranie listy kontrahentów przebiegło pomyślnie.", LogType.Information);
                return contractors;
            }
            catch (Exception exception)
            {
                itDesk.Debug.Log.Create($"Wystąpił błąd eksportowania kontrahentó, {exception.Message}", LogType.Error);
                return new List<Contractor>();
            }
        }
        public static bool AddContractors(string _database, string _login, string _password, List<Contractor> contractors)
        {
            itDesk.Debug.Log.Create("Rozpoczęcie operacji dodawania Kontrahentów", LogType.Information);
            try
            {
                Database database = BusApplication.Instance[_database];
                Login login = database.Login(false, _login, _password);
                foreach(Contractor contractor in contractors)
                {
                    using (Session session = login.CreateSession(false, false, "ImportContractors"))
                    {
                        var handelModule = HandelModule.GetInstance(session);
                        var crmModule = CRMModule.GetInstance(session);
                        Kontrahent kontrahent = null;
                        try
                        {
                            kontrahent = crmModule.Kontrahenci.WgKodu[contractor.Id];
                        }
                        catch (Exception exception)
                        {
                            itDesk.Debug.Log.Create($"Wystąpił błąd podczas dodawania kontrahentów, {exception.Message}", LogType.Error);
                        }

                        using (ITransaction transaction = session.Logout(true))
                        {
                            if (kontrahent == null)
                            {
                                kontrahent = new Kontrahent();

                                crmModule.Kontrahenci.AddRow(kontrahent);

                            }

                            if (kontrahent.Nazwa == "!INCYDENTALNY")
                                continue;

                            kontrahent.NIP = contractor.NIP;
                            kontrahent.Nazwa = contractor.Nazwa;
                            kontrahent.EMAIL = contractor.Email;
                            kontrahent.Kod = contractor.Id;
                            kontrahent.PESEL = contractor.PESEL;
                            transaction.Commit();
                        }
                        session.Save();
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                itDesk.Debug.Log.Create($"Wystąpił błąd dodawania kontrahentów, {exception.Message}", LogType.Error);
                return false;
            }
        }
    }
}
