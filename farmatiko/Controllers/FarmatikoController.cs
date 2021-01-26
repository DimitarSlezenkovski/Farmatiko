using System.Collections.Generic;
using System.Threading.Tasks;
using FarmatikoData.Models;
using FarmatikoServices.FarmatikoServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Farmatiko.Controllers
{
    [ApiController]
    public class FarmatikoController : Controller
    {
        private readonly IService _service;
        private readonly IProcessJSONService _JSONservice;
        public FarmatikoController(IService service, IProcessJSONService JSONservice)
        {
            _service = service;
            _JSONservice = JSONservice;
        }
        // Workers
        //Get
        [HttpGet]
        [Route("api/getData")]
        public void InsertData()
        {
            //_JSONservice.DownloadPharmaciesExcel();
            //_JSONservice.GetProcessedHealthcareWorkersFromJSON();
            //_JSONservice.GetProcessedHealthFacilitiesFromJSON();
            //_JSONservice.GetProcessedMedicinesFromJSON();
            //_JSONservice.GetProcessedPandemicsFromJSONApi();
        }
        [HttpGet]
        [Route("api/workers")]
        public async Task<IEnumerable<HealthcareWorker>> GetWorkers()
        {
            var Workers =  await _service.GetAllWorkers();
            return Workers;
        }
        [HttpGet]
        [Route("api/workers/search/{query}")]
        public async Task<IEnumerable<HealthcareWorker>> SearchWorkers([FromRoute]string query)
        {
            return await _service.SearchWorkers(query);
        }
        [HttpGet]
        [Route("api/workers/{id}")]
        public async Task<HealthcareWorker> GetWorker([FromRoute] int Id)
        {
            return await _service.GetWorker(Id);
        }
        //Post


        //Facilities
        //Get
        [HttpGet]
        [Route("api/facilities")]
        public async Task<IEnumerable<HealthFacility>> GetFacilities()
        {
            return await _service.GetFacilities();
        }
        [HttpGet]
        [Route("api/facilities/search/{query}")]
        public async Task<IEnumerable<HealthFacility>> SearchFacilities([FromRoute] string query)
        {
            return await _service.SearchFacilities(query);
        }
        [HttpGet]
        [Route("api/facilities/{id}")]
        public async Task<HealthFacility> GetFacility([FromRoute] int id)
        {
            return await _service.GetFacility(id);
        }
        //Post

        //Medicine
        //Get
        [HttpGet]
        [Route("api/medicines")]
        public async Task<IEnumerable<Medicine>> GetMedicines()
        {
            return await _service.GetMedicines();
        }
        [HttpGet]
        [Route("api/medicines/search/{query}")]
        public async Task<IEnumerable<Medicine>> SearchMedicines([FromRoute] string query)
        {
            return await _service.SearchMedicines(query);
        }
        [HttpGet]
        [Route("api/medicines/{Id}")]
        public async Task<Medicine> GetMedicine([FromRoute] int Id)
        {
            return await _service.GetMedicine(Id);
        }
        //Pandemic
        [HttpGet]
        [Route("api/pandemic")]
        public async Task<Pandemic> GetPandemic()
        {
            return await _service.GetPandemic();
        }
        //Pharmacy
        //GET
        [HttpGet]
        [Route("api/pharmacy")]
        public async Task<IEnumerable<Pharmacy>> GetPharmacies()
        {
            return await _service.GetPharmacies();
        }
        [HttpGet]
        [Route("api/pharmacy/search/{Query}")]
        public async Task<IEnumerable<Pharmacy>> SearchPharmacies([FromRoute] string Query)
        {
            return await _service.SearchPharmacies(Query);
        }
        [HttpGet]
        [Route("api/pharmacy/{Id}")]
        public async Task<Pharmacy> GetPharmacy([FromRoute] int Id)
        {
            return await _service.GetPharmacy(Id);
        }

    }
}
