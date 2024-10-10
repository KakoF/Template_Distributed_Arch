﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;
using System.Net;

namespace API_EntityFramework.Extensions
{
	public static class AppExtension
	{
		public static void UsePrometheus(this WebApplication app)
		{
			//Preciso setar essa conf de options, para quando algum serviço externo ficar Unhealthy, não quebrar integração com Prometheus
			app.UseHealthChecksPrometheusExporter("/metrics", options => options.ResultStatusCodes[HealthStatus.Unhealthy] = (int)HttpStatusCode.OK);
			app.UseHttpMetrics();
		}
	
	}
}