// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Hl7.Fhir.Model;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Models.Foundations.Patients.Exceptions;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Tests.Unit.Foundations.Patients
{
    public partial class PatientServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnEverythingAsync(
            Exception dependencyValidationException)
        {
            // given
            string randomId = GetRandomString();
            string inputId = randomId;
            string inputFilePath = randomId;
            CancellationToken inputCancellationToken = default;

            this.fhirFileBrokerMock.Setup(broker =>
                broker.RetrieveFhirBundleAsync(inputFilePath))
                    .ThrowsAsync(dependencyValidationException);

            var invalidDependencyPatientServiceException = new InvalidDependencyPatientServiceException(
                message: "Invalid dependency error.",
                innerException: dependencyValidationException);

            var expectedPatientDependencyValidationException =
                new PatientDependencyValidationException(
                    "Patient dependency validation error occurred, please fix the errors and try again.",
                    invalidDependencyPatientServiceException);

            // when
            ValueTask<Bundle> everythingTask = patientService.EverythingAsync(
                id: inputId,
                cancellationToken: inputCancellationToken);

            PatientDependencyValidationException actualPatientDependencyValidationException =
                await Assert.ThrowsAsync<PatientDependencyValidationException>(
                    everythingTask.AsTask);

            // then
            actualPatientDependencyValidationException
                .Should().BeEquivalentTo(expectedPatientDependencyValidationException);

            this.fhirFileBrokerMock.Verify(broker =>
                broker.RetrieveFhirBundleAsync(inputFilePath),
                    Times.Once);

            this.fhirFileBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnEverythingAsync(
            Exception dependencyException)
        {
            // given
            string randomId = GetRandomString();
            string inputId = randomId;
            string inputFilePath = randomId;
            CancellationToken inputCancellationToken = default;

            this.fhirFileBrokerMock.Setup(broker =>
                broker.RetrieveFhirBundleAsync(inputFilePath))
                    .ThrowsAsync(dependencyException);

            var failedDependencyPatientServiceException = new FailedDependencyPatientServiceException(
                message: "Failed dependency error.",
                innerException: dependencyException);

            var expectedPatientDependencyException =
                new PatientDependencyException(
                    "Patient dependency error occurred, please fix the errors and try again.",
                    failedDependencyPatientServiceException);

            // when
            ValueTask<Bundle> everythingTask = patientService.EverythingAsync(
                id: inputId,
                cancellationToken: inputCancellationToken);

            PatientDependencyException actualPatientDependencyException =
                await Assert.ThrowsAsync<PatientDependencyException>(
                    everythingTask.AsTask);

            // then
            actualPatientDependencyException
                .Should().BeEquivalentTo(expectedPatientDependencyException);

            this.fhirFileBrokerMock.Verify(broker =>
                broker.RetrieveFhirBundleAsync(inputFilePath),
                    Times.Once);

            this.fhirFileBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnEverythingAsync()
        {
            // given
            Exception someException = new Exception();
            string randomId = GetRandomString();
            string inputId = randomId;
            string inputFilePath = randomId;
            CancellationToken inputCancellationToken = default;

            this.fhirFileBrokerMock.Setup(broker =>
                broker.RetrieveFhirBundleAsync(inputFilePath))
                    .ThrowsAsync(someException);

            var failedPatientServiceException = new FailedPatientServiceException(
                message: "Failed patient service error occurred, please contact support.",
                innerException: someException,
                data: someException.Data);

            var expectedPatientServiceException =
                new PatientServiceException(
                    "Patient service error occurred, please contact support.",
                    failedPatientServiceException);

            // when
            ValueTask<Bundle> everythingTask = patientService.EverythingAsync(
                id: inputId,
                cancellationToken: inputCancellationToken);

            PatientServiceException actualPatientServiceException =
                await Assert.ThrowsAsync<PatientServiceException>(
                    everythingTask.AsTask);

            // then
            actualPatientServiceException
                .Should().BeEquivalentTo(expectedPatientServiceException);

            this.fhirFileBrokerMock.Verify(broker =>
                broker.RetrieveFhirBundleAsync(inputFilePath),
                    Times.Once);

            this.fhirFileBrokerMock.VerifyNoOtherCalls();
        }
    }
}
