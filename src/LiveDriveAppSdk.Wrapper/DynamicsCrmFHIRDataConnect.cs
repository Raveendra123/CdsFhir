// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Microsoft.Health.FHIR.DynamicsCrm
{
    internal class DynamicsCrmFHIRDataConnect
    {
        // Connection Params
        private static IOrganizationService organizationService = null;
        private static Uri uri;
        private static ClientCredentials clientCredentials;

        // Connect to Crm Instance with User credentials defined fetched from config file.
        static DynamicsCrmFHIRDataConnect()
        {
            // Organization Url
            uri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineFhirUrl"].ToString());

            // Client Credentials
            clientCredentials = new ClientCredentials();
            clientCredentials.UserName.UserName = System.Configuration.ConfigurationManager.AppSettings["FhirUsername"].ToString();
            clientCredentials.UserName.Password = System.Configuration.ConfigurationManager.AppSettings["FhirPassword"].ToString();

            // For Dynamics 365 Customer Engagement V9.X, set Security Protocol as TLS12
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Get the URL from CRM
            organizationService = new OrganizationServiceProxy(uri, null, clientCredentials, null);
        }

        // Organization Service
        public static IOrganizationService OrganizationService
        {
            get
            {
                return organizationService;
            }

            set
            {
                organizationService = value;
            }
        }
    }
}
