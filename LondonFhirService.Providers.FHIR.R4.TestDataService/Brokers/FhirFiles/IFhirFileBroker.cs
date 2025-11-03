// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using Hl7.Fhir.Model;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Brokers.FhirFiles
{
    public interface IFhirFileBroker
    {
        List<Bundle> RetrieveAllFhirBundles();
    }
}
