// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Health.Fhir.SqlServer.Features.Storage
{
        public static class MicrosoftDynamicsCrmFhirDataStore
    {
        private const string FetchObservation = "Fetch Observation XML";

        public const string Observation = "Observation";
        private static string attributeName;

        private static string attributeValue;

        private static EntityCollection entityCollection;

        private const string FetchObservationXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                        <entity name='msemr_observation'>
                                        <attribute name='msemr_observationid' />
                                        <attribute name='msemr_description' />
                                        <attribute name='createdon' />
                                        </entity>
                                        </fetch>";

        // AttributeName
        public static string AttributeName
        {
            get
            {
                return attributeName;
            }

            set
            {
                attributeName = value;
            }
        }

        // AttributeName
        public static string AttributeValue
        {
            get
            {
                return attributeValue;
            }

            set
            {
                attributeValue = value;
            }
        }

        // EntityCollection
        public static EntityCollection EntityCollection
        {
            get
            {
                return entityCollection;
            }

            set
            {
                entityCollection = value;
            }
        }

         // Get record collection
        public static EntityCollection RetrieveRecords(string fetchCriteria)
        {
            try
            {
                string xmlData = string.Empty;
                switch (fetchCriteria)
                {
                    case FetchObservation:
                        xmlData = FetchObservationXml;
                        break;
                }

                // Organization Url
                Uri uri = new Uri("https://cggtech.crm.dynamics.com/XRMServices/2011/Organization.svc");

                // Client Credentials
                var clientCredentials = new ClientCredentials();
                clientCredentials.UserName.UserName = "sathis@cggtech.onmicrosoft.com";
                clientCredentials.UserName.Password = "Password@123";

                // For Dynamics 365 Customer Engagement V9.X, set Security Protocol as TLS12
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // Get the URL from CRM
                IOrganizationService organizationService = new OrganizationServiceProxy(uri, null, clientCredentials, null);

                return organizationService.RetrieveMultiple(new FetchExpression(xmlData));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static string FetchQueryResult(string queryFilter)
        {
            string errMessage = string.Empty;
            string xmlFilter = string.Empty;

            switch (queryFilter)
            {
                case MicrosoftDynamicsCrmFhirDataStore.Observation:
                    xmlFilter = FetchObservation;
                    break;
            }

            // Organization Url
            Uri uri = new Uri("https://cggtech.crm.dynamics.com/XRMServices/2011/Organization.svc");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            EntityCollection = RetrieveRecords(xmlFilter);
            if (EntityCollection.Entities.Count <= 0)
            {
            }
            else if (EntityCollection.Entities.Count > 0)
            {
                errMessage = string.Empty;
            }

            return errMessage;
        }
    }
}
