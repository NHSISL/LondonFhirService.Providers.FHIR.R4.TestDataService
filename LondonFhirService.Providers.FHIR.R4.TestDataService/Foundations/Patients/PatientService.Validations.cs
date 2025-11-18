// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Models.Foundations.Patients.Exceptions;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Foundations.Patients
{
    public partial class PatientService
    {
        private async ValueTask ValidateEverythingParams(string id)
        {
            Validate(
                createException: () => new InvalidArgumentPatientServiceException(
                    message: "Invalid patient service argument. Please correct the errors and try again."),

                (Rule: IsInvalid(id), Parameter: nameof(id)));
        }

        private static dynamic IsInvalid(string value) => new
        {
            Condition = string.IsNullOrWhiteSpace(value),
            Message = "Text is invalid"
        };

        private static void Validate(
            Func<InvalidArgumentPatientServiceException> createException,
            params (dynamic Rule, string Parameter)[] validations)
        {
            InvalidArgumentPatientServiceException invalidArgumentPatientServiceException = createException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentPatientServiceException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentPatientServiceException.ThrowIfContainsErrors();
        }
    }
}
