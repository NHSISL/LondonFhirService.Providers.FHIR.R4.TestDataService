// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using LondonFhirService.Providers.FHIR.R4.Abstractions.Models.Resources;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Brokers.FhirFiles;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Models.TdsHttp;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Providers;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Resources;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Services.Foundations.Patients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Functions;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureAppConfiguration(config =>
            {
                var env = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");

                config.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(path: "appsettings.json")
                    .AddJsonFile(
                        path: $"appsettings.{env}.json",
                        optional: true)
                    .AddJsonFile(
                        path: "appsettings.local.json",
                        optional: true,
                        reloadOnChange: true)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;

                TdsConfigurations tdsConfigurations = new TdsConfigurations
                {
                    Source = "Test Data Service",
                    Code = "TDS",
                    System = "https://testservice.com"
                };

                services.AddSingleton(tdsConfigurations);
                AddBrokers(services);
                AddFoundationServices(services);
                AddProviders(services);
            })
            .UseDefaultServiceProvider(options => options.ValidateScopes = false)
            .ConfigureFunctionsWorkerDefaults()
            .Build();

        await host.RunAsync();
    }

    private static void AddBrokers(IServiceCollection services)
    {
        services.AddTransient<IFhirFileBroker, FhirFileBroker>();
    }

    private static void AddFoundationServices(IServiceCollection services)
    {
        services.AddTransient<IPatientService, PatientService>();
    }

    private static void AddProviders(IServiceCollection services)
    {
        services.AddTransient<IPatientResource, PatientResource>();
        services.AddTransient<ITdsR4Provider, TdsR4Provider>();
    }
}