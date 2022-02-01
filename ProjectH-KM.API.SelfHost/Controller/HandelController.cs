using ProjectH_KM.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ProjectH_KM.API.SelfHost.Controller
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HandelController : ApiController
    {
        private bool _response = false;
        private string _database = ConfigurationManager.AppSettings["DatabaseName"];
        private string _login = ConfigurationManager.AppSettings["EnovaOperator"];
        private string _password = ConfigurationManager.AppSettings["EnovaPassword"];
        [ActionName("products")]
        [HttpPost]
        public HttpResponseMessage CreateProducts()
        {
            _response = HandelRepository.CreateProducts(_database, _login, _password);
            return Request.CreateResponse(HttpStatusCode.Created, _response);
        }
        [ActionName("productAB")]
        [HttpPost]
        public HttpResponseMessage CreateProductAB()
        {
            _response = HandelRepository.CreateProductAB(_database, _login, _password);
            return Request.CreateResponse(HttpStatusCode.Created, _response);
        }
        [ActionName("PW")]
        public HttpResponseMessage CreatePW()
        {
            _response = HandelRepository.CreatePW(_database, _login, _password);
            return Request.CreateResponse(HttpStatusCode.Created, _response);
        }
        [ActionName("KPL")]
        [HttpPost]
        public HttpResponseMessage CreateKPL()
        {
            _response = HandelRepository.CreateKPL(_database, _login, _password);
            return Request.CreateResponse(HttpStatusCode.Created, _response);
        }
    }
}
