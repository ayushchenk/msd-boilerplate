using Microsoft.Xrm.Sdk;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace MSD.Shared.Extensions
{
    public static class OrganizationServiceExtensions
    {
        public static T Execute<T>(this IOrganizationService service, OrganizationRequest request) where T : OrganizationResponse
        {
            return (T)service.Execute(request);
        }

        public static List<Entity> RetrieveMultipleAll(this IOrganizationService service, QueryExpression query)
        {
            var entities = new List<Entity>();
            query.PageInfo = new PagingInfo()
            {
                Count = 5000,
                PageNumber = 1
            };
            EntityCollection response;
            do
            {
                response = service.RetrieveMultiple(query);
                entities.AddRange(response.Entities);
                query.PageInfo.PageNumber++;
                query.PageInfo.PagingCookie = response.PagingCookie;
            }
            while (response.MoreRecords);
            return entities;
        }

        public static string GetOptionSetLabel(this IOrganizationService service, Entity entity, string attributeName)
        {
            var curValue = entity.GetAttributeValue<OptionSetValue>(attributeName)?.Value ?? -1;
            return service.GetOptionSetLabel(entity.LogicalName, attributeName, curValue);
        }

        public static string GetOptionSetLabel(this IOrganizationService service, string entityName, string attrName, int attrValue)
        {
            var attributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attrName,
                RetrieveAsIfPublished = true
            };

            var attributeResponse = (RetrieveAttributeResponse)service.Execute(attributeRequest);
            var attributeMetadata = (EnumAttributeMetadata)attributeResponse.AttributeMetadata;

            return attributeMetadata.OptionSet.Options
                .Where(option => option.Value.HasValue)
                .FirstOrDefault(option => option.Value.Value == attrValue)
                ?.Label.UserLocalizedLabel.Label;
        }

        public static string GetAttributeDisplayName(this IOrganizationService service, string entityName, string attrName)
        {
            var attributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attrName,
                RetrieveAsIfPublished = true
            };

            var attributeResponse = (RetrieveAttributeResponse)service.Execute(attributeRequest);
            var attributeMetadata = attributeResponse.AttributeMetadata;

            return attributeMetadata.DisplayName.UserLocalizedLabel.Label;
        }
    }
}