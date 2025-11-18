// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Hl7.Fhir.Model;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Foundations.Patients;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Models.Foundations.Patients.Exceptions;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Tests.Unit.Foundations.Patients
{
    public partial class PatientServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public async Task ShouldThrowValidationExceptionOnEverythingAsync(
            string invalidText)
        {
            // given
            string inputId = invalidText;
            string randomFilePath = inputId;
            CancellationToken inputCancellationToken = default;

            var patientServiceMock = new Mock<PatientService>(
               this.fhirFileBrokerMock.Object)
            { CallBase = true };

            InvalidArgumentPatientServiceException invalidArgumentPatientServiceException =
                new InvalidArgumentPatientServiceException(
                    "Invalid patient service argument. Please correct the errors and try again.");

            invalidArgumentPatientServiceException.AddData(
                key: "id",
                values: "Text is invalid");

            var expectedPatientValidationException =
                new PatientValidationException(
                    "Patient validation error occurred, please fix the errors and try again.",
                    invalidArgumentPatientServiceException);

            var patientService = patientServiceMock.Object;

            // when
            ValueTask<Bundle> everythingTask = patientService.EverythingAsync(
                id: inputId,
                cancellationToken: inputCancellationToken);

            PatientValidationException actualPatientValidationException =
                await Assert.ThrowsAsync<PatientValidationException>(
                    everythingTask.AsTask);

            // then
            actualPatientValidationException.Should().BeEquivalentTo(expectedPatientValidationException);

            this.fhirFileBrokerMock.VerifyNoOtherCalls();
        }
    }
}
