// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Models.Foundations.Patients.Exceptions
{
    public class InvalidDependencyPatientServiceException : Xeption
    {
        public InvalidDependencyPatientServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
