// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Models.Brokers.FhirFiles.Exceptions;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Brokers.FhirFiles
{
    public class FhirFileBroker : IFhirFileBroker
    {
        private readonly string fhirTestDataPath;

        public FhirFileBroker(string testDataFolderName = "Data")
        {
            this.fhirTestDataPath = Path.Combine(AppContext.BaseDirectory, testDataFolderName);
        }

        public Bundle RetrieveFhirBundle(string nhsNumber)
        {
            Bundle fhirBundle = null;

            string jsonFilePath = Directory.GetFiles(fhirTestDataPath, $"{nhsNumber}.json").FirstOrDefault();

            if (jsonFilePath is null)
            {
                throw new TestPatientNotFoundException($"Test patient with NhsNumber {nhsNumber} not found");
            }

            string fileContent = File.ReadAllText(jsonFilePath);
            var parser = new FhirJsonParser();
            fhirBundle = parser.Parse<Bundle>(fileContent);

            return fhirBundle;
        }
    }
}
