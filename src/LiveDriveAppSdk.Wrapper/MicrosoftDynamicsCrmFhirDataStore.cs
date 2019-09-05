// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Microsoft.Health.FHIR.DynamicsCrm
{
    public class MicrosoftDynamicsCrmFhirDataStore
    {
        private const string EntityName = "Dynamics Health 365";

        private const string FetchObservation = "Fetch Observation XML";

        public const string Observation = "Observation";

        private static System.Collections.Generic.List<Entity> entityResult;

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

        // ResultEntity
        public static List<Entity> EntityResult
        {
            get
            {
                return entityResult;
            }

            set
            {
                entityResult = value;
            }
        }

        // Get record collection
        public static EntityCollection RetrieveRecords(string fetchCriteria)
        {
            string xmlData = string.Empty;
            switch (fetchCriteria)
            {
                case FetchObservation:
                    xmlData = FetchObservationXml;
                    break;
            }

            return DynamicsCrmFHIRDataConnect.OrganizationService.RetrieveMultiple(new FetchExpression(xmlData));
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
