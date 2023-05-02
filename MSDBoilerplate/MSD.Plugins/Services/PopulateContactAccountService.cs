﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using MSD.Plugins.Queries;
using MSD.Shared.Abstract;
using MSD.Shared.Abstract.Cqrs.Handlers;
using MSD.Shared.Definitions;

namespace MSD.Plugins.Services
{
    public class PopulateContactAccountService : IPluginService
    {
        private readonly DataverseQueryHandler<AccountByNameQuery, Entity> _accountHandler;

        public PopulateContactAccountService(DataverseQueryHandler<AccountByNameQuery, Entity> accountHandler)
        {
            _accountHandler = accountHandler;
        }

        public void Execute(IServiceFactory serviceFactory)
        {
            var target = serviceFactory.GetInputParameter<Entity>(Common.Target);

            var account = _accountHandler.Handle(new AccountByNameQuery("some account", new ColumnSet("accountid")));

            if (account != null)
            {
                target["parentcustomerid"] = account.ToEntityReference();
            }
        }
    }
}
