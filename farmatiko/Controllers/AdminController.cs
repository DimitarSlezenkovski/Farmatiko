using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmatikoData.DTOs;
using FarmatikoData.Models;
using FarmatikoServices.FarmatikoServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Farmatiko.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IService _service;
        private readonly IPHService _phservice;
        public AdminController(IAdminService adminService, IService service, IPHService phservice)
        {
            _adminService = adminService;
            _service = service;
            _phservice = phservice;
        }

        //GET
        [HttpGet]
        [Route("api/pharmacyhead")]
        public async Task<IEnumerable<PharmacyHead>> GetPharmacyHeads()
        {
            return await _adminService.GetPharmacyHeads();
        }

        [HttpGet]
        [Route("api/pharmacyhead/requests")]
        public async Task<IEnumerable<RequestPharmacyHead>> GetClaimingRequests()
        {
            return await _adminService.GetClaimingRequests();
        }


        //POST
        [HttpPost]
        [Route("api/pharmacyhead/add")]
        public async Task<IActionResult> AddPharmacyHead([FromBody]PharmacyHead pharmacyHead)
        {
            await _service.AddPharmacyHead(pharmacyHead);
            return Ok("Pharmacy added.");
        }

        [HttpDelete]
        [Route("api/pharmacyhead/delete/{Id}")] 
        public async Task<IActionResult> RemovePharmacyHead([FromRoute] int Id)
        {
            await _service.RemovePharmacyHead(Id);
            return Ok();
        }
        [HttpDelete]
        [Route("api/pharmacyhead/requests/{Id}")]
        public IActionResult RejectRequest([FromRoute] int Id)
        {
            bool Success = _adminService.RejectRequest(Id);
            return Ok(Success);
        }
        [HttpPost]
        [Route("api/pharmacyhead/{Id}")]
        public async Task<IActionResult> ApproveRequest([FromRoute]int Id, [FromBody]PharmacyHeadDto pharmacyHead)
        {
            await _phservice.UpdatePharmacyHead(pharmacyHead);
            return Ok();
        }

    }
}
