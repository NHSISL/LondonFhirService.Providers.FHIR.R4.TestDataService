// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Brokers.Loggings;
using LondonFhirService.Providers.FHIR.R4.TestDataService.Providers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace LondonFhirService.Providers.FHIR.R4.TestDataService.Functions
{
    public class PatientEverythingFunction
    {
        private readonly ITdsR4Provider tdsR4Provider;
        private readonly ILoggingBroker loggingBroker;

        public PatientEverythingFunction(
            ITdsR4Provider tdsR4Provider,
            ILoggingBroker loggingBroker)
        {
            this.tdsR4Provider = tdsR4Provider;
            this.loggingBroker = loggingBroker;
        }

        [Function("PatientEverythingFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                "post",
                Route = "Patient/{id}/$everything")]
            HttpRequestData req,
            string id,
            CancellationToken cancellationToken)
        {
            loggingBroker.LogInformation(
                $"Patient $everything operation invoked for Patient ID: {id}");

            try
            {
                string requestBody;

                using (var reader = new StreamReader(req.Body))
                {
                    requestBody = await reader.ReadToEndAsync(cancellationToken);
                }

                var fhirJsonParser = new FhirJsonParser();
                Parameters parameters = fhirJsonParser.Parse<Parameters>(requestBody);

                DateTimeOffset? start = ExtractDateTimeParameter(parameters, "start");
                DateTimeOffset? end = ExtractDateTimeParameter(parameters, "end");
                string typeFilter = ExtractStringParameter(parameters, "_type");
                DateTimeOffset? since = ExtractDateTimeParameter(parameters, "_since");
                int? count = ExtractIntParameter(parameters, "_count");

                Bundle bundle = await this.tdsR4Provider
                    .Patients
                    .Everything(
                        id: id,
                        start: start,
                        end: end,
                        typeFilter: typeFilter,
                        since: since,
                        count: count,
                        cancellationToken: cancellationToken);

                var fhirJsonSerializer = new FhirJsonSerializer();
                string bundleJson = await fhirJsonSerializer.SerializeToStringAsync(bundle);

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/fhir+json");
                await response.WriteStringAsync(bundleJson);

                loggingBroker.LogInformation(
                    $"Successfully returned bundle for Patient ID: {id}");

                return response;
            }
            catch (Exception ex)
            {
                loggingBroker.LogError(ex);
                throw;
            }
        }

        private static string ExtractStringParameter(Parameters parameters, string name)
        {
            var parameter = parameters?.Parameter?.FirstOrDefault(p => p.Name == name);

            return parameter?.Value is FhirString fhirString ? fhirString.Value : null;
        }

        private static DateTimeOffset? ExtractDateTimeParameter(Parameters parameters, string name)
        {
            var parameter = parameters?.Parameter?.FirstOrDefault(p => p.Name == name);

            if (parameter?.Value is FhirDateTime fhirDateTime && fhirDateTime.Value != null)
            {
                return DateTimeOffset.Parse(fhirDateTime.Value);
            }

            if (parameter?.Value is Date date && date.Value != null)
            {
                return DateTimeOffset.Parse(date.Value);
            }

            return null;
        }

        private static int? ExtractIntParameter(Parameters parameters, string name)
        {
            var parameter = parameters?.Parameter?.FirstOrDefault(p => p.Name == name);

            return parameter?.Value is Integer integer ? integer.Value : null;
        }
    }
}
