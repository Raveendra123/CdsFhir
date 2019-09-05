using System;

//proj related
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;
using System.Collections.Generic;

namespace LiveDriveAppSdk.Wrapper
{
    public class FhirAccelerator
    {
        //Constants
        private const string EntityName = "Dynamics Health 365";

        private const string fetchObservation = "Fetch Observation XML";

        public const string observation = "Observation";

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

        //AttributeName
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

        //AttributeName
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

        //EntityCollection
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

        //ResultEntity
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

        //Get record collection
        public static EntityCollection RetrieveRecords(string fetchCriteria)
        {
            string xmlData = string.Empty;
            switch (fetchCriteria)
            {
                case fetchObservation:
                    xmlData = FetchObservationXml;
                    break;
            }
            return FHIRDataConnect.OrganizationService.RetrieveMultiple(new FetchExpression(xmlData));
        }

        //Fetch the result for required entity
        public static string FetchQueryResult(string queryFilter)
        {
            string errMessage = string.Empty;
            string xmlFilter = string.Empty;

            switch (queryFilter)
            {
                case FhirAccelerator.observation:
                    xmlFilter = fetchObservation;
                    break;
            }
            EntityCollection = RetrieveRecords(xmlFilter);
            if (EntityCollection.Entities.Count <= 0)
            {
                errMessage = MessageResource.NoRecords;
            }
            else if (EntityCollection.Entities.Count > 0)
            {
                errMessage = string.Empty;
            }

            return errMessage;
        }
    }
}
