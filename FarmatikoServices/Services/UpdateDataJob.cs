using FarmatikoData;
using FarmatikoServices.FarmatikoServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FarmatikoServices.Services
{
    [DisallowConcurrentExecution]
    public class UpdateDataJob : IJob
    {
        private readonly ILogger<UpdateDataJob> _logger;
        private readonly IProcessJSONService _JSONservice;
        private readonly IServiceScopeFactory _provider;
        public UpdateDataJob(ILogger<UpdateDataJob> logger, IProcessJSONService JSONservice, IServiceScopeFactory provider)
        {
            _logger = logger;
            _JSONservice = JSONservice;
            _provider = provider;
        }
       
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("UpdateData Job started");
            using (var scope = _provider.CreateScope())
            {
                // Resolve the Scoped service
                var service = scope.ServiceProvider.GetRequiredService<FarmatikoDataContext>();
                _logger.LogInformation("UpdateData Job started");
                _JSONservice.DownloadPharmaciesExcel();
                _JSONservice.GetProcessedHealthcareWorkersFromJSON();
                _JSONservice.GetProcessedHealthFacilitiesFromJSON();
                _JSONservice.GetProcessedMedicinesFromJSON();
                
            }
            
            await Task.CompletedTask;
        }
    }
}
