// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Microsoft.Health.Fhir.SqlServer.Features.Storage
{
    internal static class MicrosoftDynamicsCrmFhirDataConnect
    {
        // Connection Params
        private static IOrganizationService organizationService = null;
        private static Uri uri;
        private static ClientCredentials clientCredentials;

        public static IOrganizationService GetOrganizationService()
        {
            // Organization Url
            uri = new Uri("https://cggtech.crm.dynamics.com/XRMServices/2011/Organization.svc");

            // Client Credentials
            clientCredentials = new ClientCredentials();
            clientCredentials.UserName.UserName = "sathis@cggtech.onmicrosoft.com";
            clientCredentials.UserName.Password = "Password@123";

            // For Dynamics 365 Customer Engagement V9.X, set Security Protocol as TLS12
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Get the URL from CRM
            organizationService = new OrganizationServiceProxy(uri, null, clientCredentials, null);

            return organizationService;
        }
    }
}
