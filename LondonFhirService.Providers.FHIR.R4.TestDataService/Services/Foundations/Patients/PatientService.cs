// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Services.Foundations.Patients
{
    public class PatientService : IPatientService
    {
        public ValueTask<Bundle> EverythingAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
