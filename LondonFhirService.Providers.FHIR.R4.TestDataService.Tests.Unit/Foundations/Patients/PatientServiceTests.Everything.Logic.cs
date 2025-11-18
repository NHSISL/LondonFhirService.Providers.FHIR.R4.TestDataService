// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading;
using FluentAssertions;
using Hl7.Fhir.Model;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Foundations.Patients;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Tests.Unit.Foundations.Patients
{
    public partial class PatientServiceTests
    {
        [Fact]
        public async Task ShouldEverythingAsync()
        {
            // given
            string randomId = GetRandomString();
            string inputId = randomId;
            string randomFilePath = GetRandomString();
            string outputFilePath = randomFilePath;
            CancellationToken inputCancellationToken = default;
            Bundle randomBundle = CreateRandomBundle();
            Bundle expectedBundle = randomBundle;
            string outputContent = GetStringFromBundle(randomBundle);

            var patientServiceMock = new Mock<PatientService>(
                this.fhirFileBrokerMock.Object)
            { CallBase = true };

            patientServiceMock.Setup(service =>
                service.GetPatientFilePathAsync(inputId))
                    .ReturnsAsync(outputFilePath);

            this.fhirFileBrokerMock.Setup(broker =>
                broker.RetrieveFhirBundleAsync(outputFilePath))
                    .ReturnsAsync(outputContent);

            var patientService = patientServiceMock.Object;

            // when
            Bundle actualBundle =
                await patientService.EverythingAsync(
                    id: inputId,
                    cancellationToken: inputCancellationToken);

            // then
            actualBundle.Should().BeEquivalentTo(expectedBundle);

            patientServiceMock.Verify(service =>
                service.GetPatientFilePathAsync(inputId),
                    Times.Once);

            this.fhirFileBrokerMock.Verify(broker =>
                broker.RetrieveFhirBundleAsync(outputFilePath),
                    Times.Once);

            patientServiceMock.VerifyNoOtherCalls();
            this.fhirFileBrokerMock.VerifyNoOtherCalls();
        }
    }
}
