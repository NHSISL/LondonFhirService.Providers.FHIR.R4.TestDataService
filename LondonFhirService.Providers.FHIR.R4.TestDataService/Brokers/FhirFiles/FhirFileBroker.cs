// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Hl7.Fhir.Model;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Brokers.FhirFiles
{
    public class FhirFileBroker : IFhirFileBroker
    {
        private readonly string fhirTestDataPath;

        public FhirFileBroker(string testDataFolderName = "Data")
        {
            this.fhirTestDataPath = Path.Combine(AppContext.BaseDirectory, testDataFolderName);
        }

        public List<Bundle> RetrieveAllFhirBundles()
        {
            var fhirBundles = new List<Bundle>();

            string[] jsonFiles = Directory.GetFiles(fhirTestDataPath, "*.json");

            foreach (string filePath in jsonFiles)
            {
                try
                {
                    string fileContent = File.ReadAllText(filePath);
                    var parser = new Hl7.Fhir.Serialization.FhirJsonParser();
                    Bundle bundle = parser.Parse<Bundle>(fileContent);

                    if (bundle is not null)
                    {
                        fhirBundles.Add(bundle);
                    }
                }
                catch (Exception exception)
                {
                    continue;
                }
            }

            return fhirBundles;
        }
    }
}
