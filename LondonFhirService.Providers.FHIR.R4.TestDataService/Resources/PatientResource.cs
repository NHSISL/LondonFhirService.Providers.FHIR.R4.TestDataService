// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using LondonFhirService.Providers.FHIR.R4.Abstractions.Models.Capabilities;
using LondonFhirService.Providers.FHIR.R4.Abstractions.Models.Resources;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Services.Foundations.Patients;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Resources
{
    public class PatientResource : ResourceOperationBase<Patient>, IPatientResource
    {
        private readonly IPatientService patientService;

        public PatientResource(IPatientService patientService)
        {
            this.patientService = patientService;
        }

        [FhirOperation]
        public async ValueTask<Bundle> Everything(
            string id,
            DateTimeOffset? start = null,
            DateTimeOffset? end = null,
            string typeFilter = null,
            DateTimeOffset? since = null,
            int? count = null,
            CancellationToken cancellationToken = default) =>
            await patientService.EverythingAsync(id, cancellationToken);

        public ValueTask<Bundle> Match(Parameters parameters, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
