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

        public OAuthMessageHandler(string url, string clientId, string username, string password, HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            // Obtain the Azure Active Directory Authentication Library (ADAL) authentication context.

            AuthenticationParameters ap = AuthenticationParameters.CreateFromResourceUrlAsync(new Uri(url + "/api/data/v9.1/")).Result;

            AuthenticationContext authContext = new AuthenticationContext(ap.Authority, false);

            ClientCredential credential = new ClientCredential("fe4c5bad-24a7-433a-a523-318a435ad676", "n3eIliUcxnEzRLu**G0Z3n.@xCys6kKC");

            string authorityUri = "https://login.microsoftonline.com/2c55b088-a14f-4c9e-8a12-c13454aa56dd/oauth2/authorize";

            AuthenticationContext context = new AuthenticationContext(authorityUri);
            var authResult = context.AcquireTokenAsync("https://cggtech.crm.dynamics.com", credential);

            authHeader = new AuthenticationHeaderValue("Bearer", authResult.Result.AccessToken);
        }

        public OAuthMessageHandler(string serviceUrl, string clientId, object redirectUrl, string username, string password, HttpMessageHandler innerHandler)

            : base(innerHandler)
        {
            // Obtain the Azure Active Directory Authentication Library (ADAL) authentication context.

            AuthenticationParameters ap = AuthenticationParameters.CreateFromResourceUrlAsync(new Uri(serviceUrl + "/api/data/v9.1/")).Result;

            AuthenticationContext authContext = new AuthenticationContext(ap.Authority, false);

            ClientCredential credential = new ClientCredential("fe4c5bad-24a7-433a-a523-318a435ad676", "n3eIliUcxnEzRLu**G0Z3n.@xCys6kKC");

            string authorityUri = "https://login.microsoftonline.com/2c55b088-a14f-4c9e-8a12-c13454aa56dd/oauth2/authorize";

            AuthenticationContext context = new AuthenticationContext(authorityUri);
            var authResult = context.AcquireTokenAsync("https://cggtech.crm.dynamics.com", credential);

            authHeader = new AuthenticationHeaderValue("Bearer", authResult.Result.AccessToken);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            request.Headers.Authorization = authHeader;
            return base.SendAsync(request, cancellationToken);
        }
    }
}
