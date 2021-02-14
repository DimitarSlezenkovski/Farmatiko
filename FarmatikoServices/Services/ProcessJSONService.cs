using FarmatikoData.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Linq;
using FarmatikoServices.FarmatikoServiceInterfaces;
using RestSharp;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using GemBox.Spreadsheet;
using System.Text;
using FarmatikoData.FarmatikoRepoInterfaces;

namespace FarmatikoServices.Services
{
    public class ProcessJSONService : IProcessJSONService
    {

        private readonly ILogger _logger;
        private readonly IUpdateDataRepo _repo;
        private readonly IService _service;
        public ProcessJSONService(ILogger logger, IUpdateDataRepo repo, IService service)
        {
            _logger = logger;
            _repo = repo;
            _service = service;
        }
        //Excel reader
        private bool ReadPharmaciesFromExcel(string Path)
        {
            string path = Directory.GetCurrentDirectory() + @"\ExcellDocs\1.xlsx";

            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo != null)
            {
                using (var package = new ExcelPackage(fileInfo))
                {
                    var Sheet = package.Workbook.Worksheets.First();
                    //var table = Sheet.Tables.First();
                    int rowCount = Sheet.Dimension.End.Row;
                    for (int i = 2; i < rowCount; ++i)
                    {
                        //Console.WriteLine();
                        Pharmacy pharmacy = new Pharmacy()
                        {
                            Name = Sheet.Cells[i, 2].Value.ToString(),
                            Address = Sheet.Cells[i, 3].Value.ToString(),
                            Location = Sheet.Cells[i, 4].Value.ToString(),
                            WorkAllTime = false
                        };
                        _service.AddPharmacy(pharmacy);
                    }
                    return true;
                }

            }
            
            return false;

        }
        public void DownloadPharmaciesExcel()
        {
            try
            {
                string pathToSave1 = Directory.GetCurrentDirectory() + @"\ExcellDocs\1.xlsx";

                string pathToSave2 = Directory.GetCurrentDirectory() + @"\ExcellDocs\2.xlsx";
                var client = new WebClient();
                string url1 = "http://data.gov.mk/dataset/d84c31d9-e749-4b17-9faf-a5b4db3e7a70/resource/4806b744-f6f6-42d0-b0f0-d82e66d3b177/download/-.xlsxs";
                /*string url2 = "http://data.gov.mk/dataset/d84c31d9-e749-4b17-9faf-a5b4db3e7a70/resource/a16379b4-ec81-4de7-994d-0ee503d71b55/download/registar-na-apteki-nadvor-od-mreza-na-fzo-12.08.2020.xlsx";*/
                Uri uri1 = new Uri(url1);
                //Uri uri2 = new Uri(url2);
                client.DownloadFileAsync(uri1, pathToSave1);
                //client.DownloadFile(uri2, @pathToSave2);


                bool Success = ReadPharmaciesFromExcel(pathToSave1);
                _logger.LogInformation(Success.ToString() + "1");

                
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                throw new Exception("Cannot process Pharmacies from Excel.");
            }
        }

        //Healthfacilities
        public void GetProcessedHealthFacilitiesFromJSON()
        {
            try
            {
                var client = new WebClient();
                var json = client.DownloadString(@"http://www.otvorenipodatoci.gov.mk/datastore/dump/505db453-4de2-4761-8a81-2800f7820b06?format=json");

                var jsonResponse = JObject.Parse(json);
                var records = JArray.Parse(jsonResponse.GetValue("records").ToString());

                foreach (var rec in records)
                {
                    dynamic obj = JsonConvert.DeserializeObject(rec.ToString());
                    var Name = obj[2];
                    var Municipality = obj[6];
                    var Address = obj[9];
                    var Email = obj[10];
                    var Phone = obj[11];
                    var Type = obj[5];
                    HealthFacility healthFacility = new HealthFacility();
                    //Name, Municipality, Address, Type, Email, Phone
                    healthFacility.Name = Convert.ToString(Name);
                    healthFacility.Municipality = Convert.ToString(Municipality);
                    healthFacility.Address = Convert.ToString(Address);
                    healthFacility.Type = Convert.ToString(Type);
                    healthFacility.Email = Convert.ToString(Email);
                    healthFacility.Phone = Convert.ToString(Phone);
                    _service.AddFacility(healthFacility);

                }
                
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                throw new Exception("Cannot process health facilities from JSON." + e.Message);
            }
        }
              
        //Healthcare workers
        public void GetProcessedHealthcareWorkersFromJSON()
        {
            try
            {
                var client = new WebClient();
                var jsonW = client.DownloadString(@"http://www.otvorenipodatoci.gov.mk/datastore/dump/5b661887-685b-4189-b6bb-9b52eb8ace16?format=json");

                var jsonResponseW = JObject.Parse(jsonW);
                var recordsW = JArray.Parse(jsonResponseW.GetValue("records").ToString());

                foreach (var rec in recordsW)
                {
                    dynamic obj = JsonConvert.DeserializeObject(rec.ToString());
                    var Name = Convert.ToString(obj[4]);
                    var Branch = Convert.ToString(obj[2]);
                    var FacilityName = Convert.ToString(obj[1]);
                    var Title = Convert.ToString(obj[3]);

                    HealthFacility facility = _repo.GetFacilityJSON(Convert.ToString(FacilityName));

                    if (facility != null && facility != default)
                    {
                        HealthFacility Facility = new HealthFacility(
                           facility.Name,
                           facility.Municipality,
                           facility.Address,
                           facility.Type,
                           facility.Email,
                           facility.Phone
                           );
                        HealthcareWorker healthcareWorker = new HealthcareWorker(Name, Branch, Facility, Title);
                        _service.AddWorker(healthcareWorker);
                    }
                    else
                    {
                        HealthFacility Facility = new HealthFacility(
                           Convert.ToString(FacilityName),
                           "",
                           "",
                           Convert.ToString(Branch),
                           "",
                           ""
                           );
                        HealthcareWorker healthcareWorker = new HealthcareWorker(Name, Branch, Facility, Title);
                        _service.AddWorker(healthcareWorker);
                    }
                    

                }
                
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }
        }
        //Medicines
        public void GetProcessedMedicinesFromJSON()
        {
            try
            {
                var client = new WebClient();
                var jsonM = client.DownloadString(@"http://www.otvorenipodatoci.gov.mk/datastore/dump/ecff2aef-9c8e-4efd-a557-96df4fff9adb?format=json");

                var jsonResponseM = JObject.Parse(jsonM);
                var recordsM = JArray.Parse(jsonResponseM.GetValue("records").ToString());

                foreach (var rec in recordsM)
                {
                    dynamic obj = JsonConvert.DeserializeObject(rec.ToString());
                    var Name = obj[1];
                    var Strength = obj[7];
                    var Form = obj[6];
                    var WayOfIssuing = obj[9];
                    var Manufacturer = obj[11];
                    var Price = float.Parse(Convert.ToString(obj[17]));
                    var Packaging = obj[8];
                    string price = Convert.ToString(Price);
                    Medicine medicine = new Medicine(Convert.ToString(Name), Convert.ToString(Strength), Convert.ToString(Form), Convert.ToString(WayOfIssuing), Convert.ToString(Manufacturer), Price, Convert.ToString(Packaging));

                    _service.AddMedicines(medicine);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                throw new Exception("medicine");
            }
        }
    }
}
