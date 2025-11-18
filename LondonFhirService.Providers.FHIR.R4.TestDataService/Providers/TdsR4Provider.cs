// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using LondonFhirService.Providers.FHIR.R4.Abstractions;
using LondonFhirService.Providers.FHIR.R4.Abstractions.Models.Resources;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Brokers.FhirFiles;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Models.TdsHttp;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Resources;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Services.Foundations.Patients;
using Microsoft.Extensions.DependencyInjection;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Providers
{
    public sealed class TdsR4Provider : FhirProviderBase, ITdsR4Provider
    {
        private readonly TdsConfigurations configurations;
        private IPatientResource patientResource { get; set; }

        public TdsR4Provider(TdsConfigurations configurations)
        {
            this.configurations = configurations;
            IServiceProvider serviceProvider = RegisterServices(configurations);
            InitializeClients(serviceProvider);
        }

        public override string Source => this.configurations.Source;
        public override string Code => this.configurations.Code;
        public override string System => this.configurations.System;
        public override IPatientResource Patients => this.patientResource;
        public override string DisplayName => "Test Data Service";
        public override string FhirVersion => "R4";

        private void InitializeClients(IServiceProvider serviceProvider) =>
            this.patientResource = serviceProvider.GetRequiredService<IPatientResource>();

        private static IServiceProvider RegisterServices(TdsConfigurations configurations)
        {
            var serviceCollection = new ServiceCollection()
                .AddTransient<IFhirFileBroker, FhirFileBroker>()
                .AddTransient<IPatientService, PatientService>()
                .AddTransient<IPatientResource, PatientResource>()
                .AddSingleton(configurations);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
