// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Health.DynamicsCrm;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;

namespace Microsoft.Health.Fhir.DynamicsCrm
{
    public class DynamicsCrmFhirDataStore
    {
        public JObject GetCdsObservationData()
        {
            ClientCredential credential = new ClientCredential("fe4c5bad-24a7-433a-a523-318a435ad676", "n3eIliUcxnEzRLu**G0Z3n.@xCys6kKC");
            string authorityUri = "https://login.microsoftonline.com/2c55b088-a14f-4c9e-8a12-c13454aa56dd/oauth2/authorize";

            AuthenticationContext context = new AuthenticationContext(authorityUri);
            var result = context.AcquireTokenAsync("https://cggtech.crm.dynamics.com", credential);

            var authToken = result.Result.AccessToken;

            JObject observationData = null;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var response = httpClient.GetAsync("https://cggtech.crm.dynamics.com/api/data/v9.1/msemr_observations?$select=msemr_observationid,msemr_description,createdon&$top=20&$filter=contains(msemr_description,'Santiago')").Result;
            if (response.IsSuccessStatusCode)
            {
                //// Get the response content and parse it.
                observationData = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            }

            return observationData;
        }

        public void PutCdsObservationData(string streamData)
        {
            using (HttpClient client = DynamicsCrmHelper.GetHttpClient())
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(streamData);
                MemoryStream stream = new MemoryStream(byteArray);

                // convert stream to string
                StreamReader reader = new StreamReader(stream);
                Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(reader.ReadLine());

                // Observation Values
                // id
                string id = (string)jObject["id"];

                // effectivedate
                string status = (string)jObject["status"];

                // effectivedate
                string date = (string)jObject["effectiveDateTime"];

                // patient
                string patient = (string)jObject["subject"]["reference"];

                // Code
                string code = (string)jObject["code"]["coding"][0]["display"];

                // ValueQuantity
                string value = (string)jObject["valueQuantity"]["value"];
                string unit = (string)jObject["valueQuantity"]["unit"];
                string system = (string)jObject["valueQuantity"]["system"];
                string valueCode = (string)jObject["valueQuantity"]["code"];

                JObject observation = new JObject();
                observation.Add("msemr_observationid", id);
                observation.Add("msemr_description", "Santiago calatrava - Observation");
                observation.Add("msemr_valuetypequantityvalue", decimal.Parse(value));
                observation.Add("msemr_valuequantityunit", unit);
                var createrequest1 = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress + "msemr_observations");
                createrequest1.Content = new StringContent(observation.ToString());
                createrequest1.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                HttpResponseMessage createResponse1 = client.SendAsync(createrequest1, HttpCompletionOption.ResponseHeadersRead).Result;
                if (createResponse1.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw new Exception(string.Format("Failed to Post Records", createResponse1.ReasonPhrase));
                }
            }
        }
    }
}
