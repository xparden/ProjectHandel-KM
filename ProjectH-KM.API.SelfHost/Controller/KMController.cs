using ProjectH_KM.DataAccess.Models.KM;
using ProjectH_KM.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net;

namespace ProjectH_KM.API.SelfHost.Controller
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class KMController : ApiController
    {
        private bool _response = false;
        private string _database = ConfigurationManager.AppSettings["DatabaseName"];
        private string _login = ConfigurationManager.AppSettings["EnovaOperator"];
        private string _password = ConfigurationManager.AppSettings["EnovaPassword"];
        [ActionName("getProducts")]
        [HttpGet]
        public HttpResponseMessage GetProducts()
        {
            List<Product> productsList;
            productsList = KMRepository.GetProducts(_database, _login, _password);
            return Request.CreateResponse(HttpStatusCode.OK, productsList);
        }
        [ActionName("addProducts")]
        [HttpPost]
        public HttpResponseMessage AddProducts([FromBody] List<Product> products)
        {
            _response = KMRepository.AddProducts(_database, _login, _password, products);
            return Request.CreateResponse(HttpStatusCode.Created, _response);
        }
        [ActionName("getContractors")]
        [HttpGet]
        public HttpResponseMessage GetContractors()
        {
            List<Contractor> contractorList;
            contractorList = KMRepository.GetContractors(_database, _login, _password);
            return Request.CreateResponse(HttpStatusCode.OK, contractorList);
        }
        [ActionName("addContractors")]
        [HttpPost]
        public HttpResponseMessage AddContractors([FromBody] List<Contractor> contractors)
        {
            _response = KMRepository.AddContractors(_database, _login, _password, contractors);
            return Request.CreateResponse(HttpStatusCode.Created, _response);
        }
    }
}
