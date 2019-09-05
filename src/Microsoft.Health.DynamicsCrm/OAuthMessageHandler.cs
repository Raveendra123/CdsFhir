// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Microsoft.Health.DynamicsCrm
{
    internal class OAuthMessageHandler : DelegatingHandler
    {
        private AuthenticationHeaderValue authHeader;

        public OAuthMessageHandler(string serviceUrl, string clientId, string username, string password, HttpMessageHandler innerHandler)

            : base(innerHandler)
        {
            // Obtain the Azure Active Directory Authentication Library (ADAL) authentication context.

            AuthenticationParameters ap = AuthenticationParameters.CreateFromResourceUrlAsync(new Uri(serviceUrl + "/api/data/v9.1/")).Result;

            AuthenticationContext authContext = new AuthenticationContext(ap.Authority, false);

            ClientCredential credential = new ClientCredential("d2bff45d-5dba-4f19-8841-d8d979eb6dab", "a+Ly3st-Z6pCmxVhodf/pCLTRHD2Ax?9");

            string authorityUri = "https://login.microsoftonline.com/c1159de4-f0a1-46e0-ad9a-b55bc7c24880/oauth2/authorize";

            AuthenticationContext context = new AuthenticationContext(authorityUri);
            var authResult = context.AcquireTokenAsync("https://ryantest12345.crm.dynamics.com", credential);

            authHeader = new AuthenticationHeaderValue("Bearer", authResult.Result.AccessToken);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            request.Headers.Authorization = authHeader;
            return base.SendAsync(request, cancellationToken);
        }
    }
}
