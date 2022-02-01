using Soneta.Business;
using Soneta.Business.App;
using Soneta.Handel;
using Soneta.Towary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using itDesk.Debug;
using Soneta.Magazyny;
using Soneta.CRM;
using Soneta.Types;

namespace ProjectH_KM.DataAccess.Repository
{
    public class HandelRepository
    {
        public static bool CreateProducts(string _database, string _login, string _password)
        {
            itDesk.Debug.Log.Create("Rozpoczynanie dodawania towarów A i B.", LogType.Information);
            try
            {
                Database database = BusApplication.Instance[_database];
                Login login = database.Login(false, _login, _password);
                using (Session session = login.CreateSession(false, false))
                {
                    TowaryModule tm = TowaryModule.GetInstance(session);
                    HandelModule hm = HandelModule.GetInstance(session);

                    using (ITransaction t = session.Logout(true))
                    {
                        Towar towarA = new Towar();
                        Towar towarB = new Towar();
                        tm.Towary.AddRow(towarA);
                        tm.Towary.AddRow(towarB);
                        towarA.Kod = "KOD_TOWAR_A";
                        towarA.Nazwa = "Towar A_2";
                        towarA.EAN = "2000000000776";
                        towarB.Kod = "KOD_TOWAR_B";
                        towarB.Nazwa = "Towar B_2";
                        towarB.EAN = "2000000000777";
                        t.Commit();
                    }
                    session.Save();
                }
                itDesk.Debug.Log.Create("Dodawanie towarów A i B przebiegło pomyślnie.", LogType.Information);
                return true;
            }
            catch (Exception exception)
            {
                itDesk.Debug.Log.Create($"Wystąpił błąd podczas dodawania towarów A oraz B, {exception.Message}", LogType.Error);
                return false;
            }
        }
        public static bool CreateProductAB(string _database, string _login, string _password)
        {
            itDesk.Debug.Log.Create("Rozpoczynanie dodawania produktu AB.", LogType.Information);
            try
            {
                Database database = BusApplication.Instance[_database];
                Login login = database.Login(false, _login, _password);
                using (Session session = login.CreateSession(false, false))
                {
                    TowaryModule tm = TowaryModule.GetInstance(session);
                    HandelModule hm = HandelModule.GetInstance(session);

                    using (ITransaction t = session.Logout(true))
                    {
                        Towar towarA = tm.Towary.WgKodu["KOD_TOWAR_A"];
                        Towar towarB = tm.Towary.WgKodu["KOD_TOWAR_B"];
                        Towar towarAB = new Towar();
                        tm.Towary.AddRow(towarAB);
                        towarAB.Kod = "KOD_TOWAR_AB2";
                        towarAB.Nazwa = "Towar AB";
                        towarAB.EAN = "2000000000778";
                        towarAB.Typ = TypTowaru.Produkt;

                        ElementKompletu ea = new ElementKompletu(towarAB);
                        tm.ElemKompletow.AddRow(ea);
                        ea.Towar = towarA;

                        ElementKompletu eb = new ElementKompletu(towarAB);
                        tm.ElemKompletow.AddRow(eb);
                        eb.Towar = towarB;

                        t.Commit();
                    }
                    session.Save();
                }
                itDesk.Debug.Log.Create("Dodawanie produktu AB przebiegło pomyślnie.", LogType.Information);
                return true;
            }
            catch (Exception exception)
            {
                itDesk.Debug.Log.Create($"Wystąpił błąd podczas dodawania towarów A oraz B, {exception.Message}", LogType.Error);
                return false;
            }
        }
        public static bool CreatePW(string _database, string _login, string _password)
        {
            itDesk.Debug.Log.Create("Rozpoczynanie dodawania dokumentu PW.", LogType.Information);
            try
            {
                Database database = BusApplication.Instance[_database];
                Login login = database.Login(false, _login, _password);
                using (Session session = login.CreateSession(false, false))
                {
                    HandelModule hm = HandelModule.GetInstance(session);
                    TowaryModule tm = TowaryModule.GetInstance(session);
                    MagazynyModule mm = MagazynyModule.GetInstance(session);
                    CRMModule cm = CRMModule.GetInstance(session);

                    using (ITransaction trans = session.Logout(true))
                    {
                        DokumentHandlowy dokument = new DokumentHandlowy();
                        DefDokHandlowego definicja = hm.DefDokHandlowych.WgSymbolu["PW"];
                        if (definicja == null)
                            throw new InvalidOperationException("Nieznaleziona definicja dokumentu PW.");
                        dokument.Definicja = definicja;
                        dokument.Magazyn = mm.Magazyny.Firma;
                        hm.DokHandlowe.AddRow(dokument);
                        Towar towarA = tm.Towary.WgKodu["KOD_TOWAR_A"];
                        Towar towarB = tm.Towary.WgKodu["KOD_TOWAR_B"];
                        using (var transPozycji = session.Logout(true))
                        {
                            PozycjaDokHandlowego pozycja = new PozycjaDokHandlowego(dokument);
                            hm.PozycjeDokHan.AddRow(pozycja);
                            pozycja.Towar = towarA;
                            pozycja.Ilosc = new Quantity(10, null);
                            pozycja.Cena = new DoubleCy(12.34);
                            transPozycji.CommitUI();
                        }
                        using (var transPozycji = session.Logout(true))
                        {
                            PozycjaDokHandlowego pozycja = new PozycjaDokHandlowego(dokument);
                            hm.PozycjeDokHan.AddRow(pozycja);
                            pozycja.Towar = towarB;
                            pozycja.Ilosc = new Quantity(10, null);
                            pozycja.Cena = new DoubleCy(12.34);
                            transPozycji.CommitUI();

                        }
                        session.Events.Invoke();
                        dokument.Stan = StanDokumentuHandlowego.Zatwierdzony;
                        trans.Commit();
                    }
                    session.Save();
                }
                itDesk.Debug.Log.Create("Dodawanie dokumentu PW przebiegło pomyślnie.", LogType.Information);
                return true;
            }
            catch (Exception exception)
            {
                itDesk.Debug.Log.Create($"Wystąpił błąd podczas dodawania dokumentu PW, {exception.Message}", LogType.Error);
                return false;
            }
        }
        public static bool CreateKPL(string _database, string _login, string _password)
        {
            itDesk.Debug.Log.Create("Rozpoczynanie dodawania dokumentu KPL.", LogType.Information);
            try
            {
                Database database = BusApplication.Instance[_database];
                Login login = database.Login(false, _login, _password);
                using (Session session = login.CreateSession(false, false))
                {
                    HandelModule hm = HandelModule.GetInstance(session);
                    TowaryModule tm = TowaryModule.GetInstance(session);
                    MagazynyModule mm = MagazynyModule.GetInstance(session);
                    CRMModule cm = CRMModule.GetInstance(session);
                    using (ITransaction trans = session.Logout(true))
                    {
                        DokumentHandlowy dokument = new DokumentHandlowy();
                        DefDokHandlowego definicja = hm.DefDokHandlowych.WgSymbolu["KPL"];
                        if (definicja == null)
                            throw new InvalidOperationException("Nieznaleziona definicja dokumentu KPL.");
                        dokument.Definicja = definicja;
                        dokument.Magazyn = mm.Magazyny.Firma;
                        hm.DokHandlowe.AddRow(dokument);
                        Towar towarAB = tm.Towary.WgKodu["KOD_AB"];
                        using (var transPozycji = session.Logout(true))
                        {
                            PozycjaDokHandlowego pozycja = new PozycjaDokHandlowego(dokument);
                            hm.PozycjeDokHan.AddRow(pozycja);
                            pozycja.Towar = towarAB;
                            pozycja.Ilosc = new Quantity(1, null);
                            transPozycji.CommitUI();
                        }
                        session.Events.Invoke();
                        dokument.Stan = StanDokumentuHandlowego.Zatwierdzony;
                        trans.Commit();
                    }
                    session.Save();
                }
                itDesk.Debug.Log.Create("Dodawanie dokumentu KPL przebiegło pomyślnie.", LogType.Information);
                return true;
            }
            catch (Exception exception)
            {
                itDesk.Debug.Log.Create($"Wystąpił błąd podczas dodawania dokumentu KPL, {exception.Message}", LogType.Error);
                return false;
            }
        }
    }
}
