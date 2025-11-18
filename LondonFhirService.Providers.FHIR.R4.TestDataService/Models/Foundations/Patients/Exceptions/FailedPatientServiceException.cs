// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Models.Foundations.Patients.Exceptions
{
    public class FailedPatientServiceException : Xeption
    {
        public FailedPatientServiceException(string message, Exception innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
