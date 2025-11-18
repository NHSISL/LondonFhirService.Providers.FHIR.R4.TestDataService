// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading;
using FluentAssertions;
using Hl7.Fhir.Model;
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
            string inputFilePath = randomId;
            CancellationToken inputCancellationToken = default;
            Bundle randomBundle = CreateRandomBundle();
            Bundle expectedBundle = randomBundle;
            string outputContent = GetStringFromBundle(randomBundle);

            this.fhirFileBrokerMock.Setup(broker =>
                broker.RetrieveFhirBundleAsync(inputFilePath))
                    .ReturnsAsync(outputContent);

            // when
            Bundle actualBundle =
                await patientService.EverythingAsync(
                    id: inputId,
                    cancellationToken: inputCancellationToken);

            // then
            actualBundle.Should().BeEquivalentTo(expectedBundle);

            this.fhirFileBrokerMock.Verify(broker =>
                broker.RetrieveFhirBundleAsync(inputFilePath),
                    Times.Once);

            this.fhirFileBrokerMock.VerifyNoOtherCalls();
        }
    }
}
