// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Models.Foundations.Patients.Exceptions
{
    public class PatientValidationException : Xeption
    {
        public PatientValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
