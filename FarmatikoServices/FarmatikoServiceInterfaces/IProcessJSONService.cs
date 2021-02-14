using FarmatikoData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FarmatikoServices.FarmatikoServiceInterfaces
{
    public interface IProcessJSONService
    {
        void GetProcessedHealthFacilitiesFromJSON();
        void GetProcessedHealthcareWorkersFromJSON();
        void GetProcessedMedicinesFromJSON();
        void DownloadPharmaciesExcel();
    }
}
