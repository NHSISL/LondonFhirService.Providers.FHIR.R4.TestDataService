// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Brokers.FhirFiles
{
    public class FhirFileBroker : IFhirFileBroker
    {
        public async ValueTask<string> RetrieveFhirBundleAsync(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}
