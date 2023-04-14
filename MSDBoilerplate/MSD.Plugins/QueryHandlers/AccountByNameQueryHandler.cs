using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using MSD.Plugins.Queries;
using MSD.Shared.Abstract;
using MSD.Shared.Abstract.Cqrs.Handlers;
using System.Linq;

namespace MSD.Plugins.QueryHandlers
{
    public class AccountByNameQueryHandler : DataverseQueryHandler<AccountByNameQuery, Entity>
    {
        public AccountByNameQueryHandler(IServiceFactory serviceFactory) : base(serviceFactory)
        {
        }

        protected override Entity Execute(AccountByNameQuery request)
        {
            var query = new QueryExpression("account")
            {
                ColumnSet = request.Columns,
                TopCount = 1
            };

            query.Criteria.AddCondition("name", ConditionOperator.Equal, request.AccountName);
            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);

            return GetOrganizationService().RetrieveMultiple(query).Entities.FirstOrDefault();
        }
    }
}
