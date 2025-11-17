// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Brokers.FhirFiles
{
    public interface IFhirFileBroker
    {
        ValueTask<string> RetrieveFhirBundleAsync(string filePath);
    }
}
