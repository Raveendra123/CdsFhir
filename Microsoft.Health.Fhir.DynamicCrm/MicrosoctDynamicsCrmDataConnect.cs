using System;

//proj related
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Query;

namespace LiveDriveAppSdk.Wrapper
{
    class FHIRDataConnect
    {
        //Connection Params
        private static IOrganizationService organizationService = null;
        private static Uri uri;
        private static ClientCredentials clientCredentials;

        //Organization Service
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

        //Connect to Crm Instance with User credentials defined fetched from config file.
        static FHIRDataConnect()
        {
            //Organization Url
            uri = new Uri(System.Configuration.ConfigurationManager.AppSettings["OnlineFhirUrl"].ToString());

            //Client Credentials
            clientCredentials = new ClientCredentials();
            clientCredentials.UserName.UserName = System.Configuration.ConfigurationManager.AppSettings["FhirUsername"].ToString();
            clientCredentials.UserName.Password = System.Configuration.ConfigurationManager.AppSettings["FhirPassword"].ToString();

            // For Dynamics 365 Customer Engagement V9.X, set Security Protocol as TLS12
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // Get the URL from CRM
            organizationService = (IOrganizationService)new OrganizationServiceProxy(uri, null, clientCredentials, null);
        }

        /// <summary>
        /// Fetch the entity columns from an entity fetched based on attribute name with attribute value.
        /// </summary>
        /// <param name="service">Service object having CRM instance values.</param>
        /// <param name="entityName">Entity that is fetched from CRM</param>
        /// <param name="attributeName">Entity attribute(filter condition) which is used to query record columns.</param>
        /// <param name="attributeValue">Entity attribute value(filter condition) which is used to query record columns.</param>
        /// <param name="cols">Record columns of an entity.</param>
        /// <returns></returns>
        public static EntityCollection GetEntityCollection(IOrganizationService service, string entityName, string attributeName, string attributeValue, ColumnSet cols)
        {
            QueryExpression query = new QueryExpression
            {
                EntityName = entityName,
                ColumnSet = cols,
                Criteria = new FilterExpression
                {
                    Conditions =
                                {
                                new ConditionExpression
                                    {
                                        AttributeName = attributeName,
                                        Operator = ConditionOperator.Equal,
                                        Values = { attributeValue }
                                    }
                                }
                }
            };
            return service.RetrieveMultiple(query);
        }
    }
}
