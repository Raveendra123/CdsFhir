// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Net.Http;

namespace Microsoft.Health.DynamicsCrm
{
    public static class DynamicsCrmHelper
    {
        public static HttpClient GetHttpClient()
        {
            string url = "https://cggtech.crm.dynamics.com";
            string username = "sathis@cggtech.onmicrosoft.com";
            string password = "Password@123";
            string clientId = "fe4c5bad-24a7-433a-a523-318a435ad676";
            string version = "v9.1";

            try
            {
                HttpMessageHandler messageHandler;
                messageHandler = new OAuthMessageHandler(url, clientId, username, password, new HttpClientHandler());

                HttpClient httpClient = new HttpClient(messageHandler)
                {
                    BaseAddress = new Uri(string.Format("{0}/api/data/{1}/", url, version)),
                    Timeout = new TimeSpan(0, 2, 0),  // 2 minutes
                };

                return httpClient;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
