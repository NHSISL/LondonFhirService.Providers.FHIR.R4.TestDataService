// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Brokers.FhirFiles;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Foundations.Patients
{
    public partial class PatientService : IPatientService
    {
        private readonly IFhirFileBroker fhirFileBroker;

        public PatientService(IFhirFileBroker fhirFileBroker) =>
            this.fhirFileBroker = fhirFileBroker;

        public ValueTask<Bundle> EverythingAsync(string id, CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            await ValidateEverythingParams(id);
            string testDataDirectory = $"{AppContext.BaseDirectory}/Data";
            string jsonFilePath = Directory.GetFiles(testDataDirectory, $"{id}.json").FirstOrDefault();
            string fileContent = await this.fhirFileBroker.RetrieveFhirBundleAsync(id);
            var parser = new FhirJsonParser();
            Bundle fhirBundle = parser.Parse<Bundle>(fileContent);

            return fhirBundle;
        });
    }
}
