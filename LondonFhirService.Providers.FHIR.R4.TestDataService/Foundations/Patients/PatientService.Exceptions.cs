// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Models.Foundations.Patients.Exceptions;
using Xeptions;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Foundations.Patients
{
    public partial class PatientService
    {
        private delegate ValueTask<Bundle> ReturningBundleFunction();

        private async ValueTask<Bundle> TryCatch(ReturningBundleFunction returningBundleFunction)
        {
            try
            {
                return await returningBundleFunction();
            }
            catch (InvalidArgumentPatientServiceException invalidArgumentPatientServiceException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentPatientServiceException);
            }
            catch (DecoderFallbackException decoderFallbackException)
            {
                FailedDependencyPatientServiceException invalidDependencyPatientServiceException =
                    new FailedDependencyPatientServiceException("Failed dependency error.", decoderFallbackException);

                throw await CreateAndLogDependencyExceptionAsync(invalidDependencyPatientServiceException);
            }
            catch (ArgumentNullException argumentNullException)
            {
                InvalidDependencyPatientServiceException invalidDependencyPatientServiceException =
                    new InvalidDependencyPatientServiceException("Invalid dependency error.", argumentNullException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidDependencyPatientServiceException);
            }
            catch (ArgumentException argumentException)
            {
                InvalidDependencyPatientServiceException invalidDependencyPatientServiceException =
                    new InvalidDependencyPatientServiceException("Invalid dependency error.", argumentException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidDependencyPatientServiceException);
            }
            catch (PathTooLongException pathTooLongException)
            {
                InvalidDependencyPatientServiceException invalidDependencyPatientServiceException =
                    new InvalidDependencyPatientServiceException("Invalid dependency error.", pathTooLongException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidDependencyPatientServiceException);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                InvalidDependencyPatientServiceException invalidDependencyPatientServiceException =
                    new InvalidDependencyPatientServiceException("Invalid dependency error.", fileNotFoundException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidDependencyPatientServiceException);
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                InvalidDependencyPatientServiceException invalidDependencyPatientServiceException =
                    new InvalidDependencyPatientServiceException(
                        "Invalid dependency error.",
                        directoryNotFoundException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidDependencyPatientServiceException);
            }
            catch (NotSupportedException notSupportedException)
            {
                InvalidDependencyPatientServiceException invalidDependencyPatientServiceException =
                    new InvalidDependencyPatientServiceException("Invalid dependency error.", notSupportedException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidDependencyPatientServiceException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                FailedDependencyPatientServiceException invalidDependencyPatientServiceException =
                    new FailedDependencyPatientServiceException("Failed dependency error.", unauthorizedAccessException);

                throw await CreateAndLogDependencyExceptionAsync(invalidDependencyPatientServiceException);
            }
            catch (IOException ioException)
            {
                FailedDependencyPatientServiceException invalidDependencyPatientServiceException =
                    new FailedDependencyPatientServiceException("Failed dependency error.", ioException);

                throw await CreateAndLogDependencyExceptionAsync(invalidDependencyPatientServiceException);
            }
            catch (JsonException jsonException)
            {
                InvalidDependencyPatientServiceException invalidDependencyPatientServiceException =
                    new InvalidDependencyPatientServiceException("Invalid dependency error.", jsonException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidDependencyPatientServiceException);
            }
            catch (InvalidOperationException invalidOperation)
            {
                FailedDependencyPatientServiceException invalidDependencyPatientServiceException =
                    new FailedDependencyPatientServiceException("Failed dependency error.", invalidOperation);

                throw await CreateAndLogDependencyExceptionAsync(invalidDependencyPatientServiceException);
            }
            catch (Exception exception)
            {
                var failedPatientServiceException =
                    new FailedPatientServiceException(
                        message: "Failed patient service error occurred, please contact support.",
                        innerException: exception,
                        data: exception.Data);

                throw await CreateAndLogServiceExceptionAsync(failedPatientServiceException);
            }
        }

        private async ValueTask<PatientDependencyValidationException> CreateAndLogDependencyValidationExceptionAsync(
            Xeption exception)
        {
            var patientServiceDependencyValidationException = new PatientDependencyValidationException(
                message: "Patient dependency validation error occurred, please fix the errors and try again.",
                innerException: exception);

            return patientServiceDependencyValidationException;
        }

        private async ValueTask<PatientDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var patientServiceDependencyValidationException = new PatientDependencyException(
                message: "Patient dependency error occurred, please fix the errors and try again.",
                innerException: exception);

            return patientServiceDependencyValidationException;
        }

        private async ValueTask<PatientValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var pdsValidationException = new PatientValidationException(
                message: "Patient validation error occurred, please fix the errors and try again.",
                innerException: exception);

            return pdsValidationException;
        }

        private async ValueTask<PatientServiceException> CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var pdsServiceException = new PatientServiceException(
                message: "Patient service error occurred, please contact support.",
                innerException: exception);

            return pdsServiceException;
        }
    }
}
