// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Models.Brokers.FhirFiles.Exceptions
{
    public class TestPatientNotFoundException : Xeption
    {
        public TestPatientNotFoundException(string message)
            : base(message)
        { }
    }
}
