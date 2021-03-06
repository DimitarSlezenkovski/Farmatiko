﻿using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmatikoServices.Services
{
    public class SingletonUpdateDataJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public SingletonUpdateDataJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                return scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
            }
            
        }

        public void ReturnJob(IJob job) { }
    }
}
