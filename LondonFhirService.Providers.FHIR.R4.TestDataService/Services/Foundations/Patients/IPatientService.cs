// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Services.Foundations.Patients
{
    public interface IPatientService
    {
        ValueTask<Bundle> EverythingAsync(string id, CancellationToken cancellationToken = default);
    }
}
