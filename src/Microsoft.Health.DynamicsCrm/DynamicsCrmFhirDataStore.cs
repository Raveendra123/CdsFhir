// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Net.Http;
using System.Net.Http.Headers;
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

            var response = httpClient.GetAsync("https://cggtech.crm.dynamics.com/api/data/v9.1/msemr_observations?$select=msemr_observationid,msemr_description,createdon&$top=20").Result;
            if (response.IsSuccessStatusCode)
            {
                //// Get the response content and parse it.
                observationData = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            }

            return observationData;
        }
    }
}
