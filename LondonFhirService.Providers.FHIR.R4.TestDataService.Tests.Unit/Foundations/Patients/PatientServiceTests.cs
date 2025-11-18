// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Brokers.FhirFiles;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Foundations.Patients;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Tests.Unit.Foundations.Patients
{
    public partial class PatientServiceTests
    {
        private readonly Mock<IFhirFileBroker> fhirFileBrokerMock;
        private readonly PatientService patientService;

        public PatientServiceTests()
        {
            this.fhirFileBrokerMock = new Mock<IFhirFileBroker>();
            this.patientService = new PatientService(
                fhirFileBroker: this.fhirFileBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static Patient CreateRandomPatient()
        {
            var patient = new Patient();

            HumanName humanName = new HumanName
            {
                ElementId = GetRandomString(),
                Family = GetRandomString(),
                Given = new List<string> { GetRandomString() },
                Prefix = new List<string> { "Mr" },
                Use = HumanName.NameUse.Usual
            };

            patient.Name = new List<HumanName> { humanName };
            patient.Gender = AdministrativeGender.Male;

            return patient;
        }

        private Bundle CreateRandomBundle()
        {
            var bundle = new Bundle
            {
                Type = Bundle.BundleType.Searchset,
                Total = 1,
                Timestamp = DateTimeOffset.UtcNow
            };

            Patient patient = CreateRandomPatient();

            bundle.Entry = new List<Bundle.EntryComponent> {
                new Bundle.EntryComponent
                {
                    FullUrl = $"https://api.service.nhs.uk/personal-demographics/FHIR/STU3/Patient/{patient.Id}",
                    Search = new Bundle.SearchComponent { Score = 1 },
                    Resource = patient
                }
            };

            bundle.Meta = new Meta
            {
                LastUpdated = DateTimeOffset.UtcNow,
                Source = GetRandomString()
            };

            return bundle;
        }

        private string GetStringFromBundle(Bundle bundle)
        {
            var fhirJsonSerializer = new FhirJsonSerializer();
            string serializedBundle = fhirJsonSerializer.SerializeToString(bundle);

            return serializedBundle;
        }

        public static TheoryData<Exception> DependencyValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Exception>
            {
                new ArgumentNullException(
                    message: randomMessage,
                    innerException),

                 new ArgumentException(
                    message: randomMessage,
                    innerException),

                  new PathTooLongException(
                    message: randomMessage,
                    innerException),

                  new FileNotFoundException(
                    message: randomMessage,
                    innerException),

                  new DirectoryNotFoundException(
                    message: randomMessage,
                    innerException),

                  new NotSupportedException(
                    message: randomMessage,
                    innerException),

                  new JsonException(
                    message: randomMessage,
                    innerException),
            };
        }

        public static TheoryData<Exception> DependencyExceptions()
        {
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;
            var innerException = new Xeption(exceptionMessage);

            return new TheoryData<Exception>
            {
                new DecoderFallbackException(
                    message: randomMessage,
                    innerException),

                new UnauthorizedAccessException(
                    message: randomMessage,
                    innerException),

                new IOException(
                    message: randomMessage,
                    innerException),

                new InvalidOperationException(
                    message: randomMessage,
                    innerException),
            };
        }
    }
}
