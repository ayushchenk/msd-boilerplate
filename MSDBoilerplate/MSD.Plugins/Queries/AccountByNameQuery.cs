using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using MSD.Shared.Abstract.Cqrs.Requests;

namespace MSD.Plugins.Queries
{
    public class AccountByNameQuery : IQuery<Entity>
    {
        public string AccountName { get; }
        public ColumnSet Columns { get; }

        public AccountByNameQuery(string accountName, ColumnSet columns)
        {
            AccountName = accountName;
            Columns = columns;
        }
    }
}
