using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System.Collections.Generic;
using System.Linq;

namespace MSD.Shared.Extensions
{
    public static class OrganizationServiceBulkExtensions
    {
        private const int MaxRequestsInExecuteMultiple = 500;

        public static List<ExecuteMultipleResponseItem> BulkCreate(this IOrganizationService service, List<Entity> entities, bool continueOnError, bool returnResponses)
        {
            var requests = entities.Select(e => new CreateRequest { Target = e }).ToList();

            return service.BulkExecute(requests, continueOnError, returnResponses);
        }
        public static List<ExecuteMultipleResponseItem> BulkUpdate(this IOrganizationService service, List<Entity> entities, bool continueOnError, bool returnResponses)
        {
            var requests = entities.Select(e => new UpdateRequest { Target = e }).ToList();

            return service.BulkExecute(requests, continueOnError, returnResponses);
        }

        public static List<ExecuteMultipleResponseItem> BulkDelete(this IOrganizationService service, List<Entity> entities, bool continueOnError, bool returnResponses)
        {
            var requests = entities.Select(e => new DeleteRequest { Target = e.ToEntityReference() }).ToList();

            return service.BulkExecute(requests, continueOnError, returnResponses);
        }

        public static List<ExecuteMultipleResponseItem> BulkUpsert(this IOrganizationService service, List<Entity> entities, bool continueOnError, bool returnResponses)
        {
            var requests = entities.Select(e => new UpsertRequest { Target = e }).ToList();

            return service.BulkExecute(requests, continueOnError, returnResponses);
        }

        public static ExecuteTransactionResponse TransactionBulkCreate(this IOrganizationService service, List<Entity> entities, bool returnResponses)
        {
            var requests = entities.Select(e => new CreateRequest { Target = e }).ToList();

            return service.TransactionBulkExecute(requests, returnResponses);
        }
        public static ExecuteTransactionResponse TransactionBulkUpdate(this IOrganizationService service, List<Entity> entities, bool returnResponses)
        {
            var requests = entities.Select(e => new UpdateRequest { Target = e }).ToList();

            return service.TransactionBulkExecute(requests, returnResponses);
        }

        public static ExecuteTransactionResponse TransactionBulkExecute<T>(this IOrganizationService service, List<T> requests, bool returnResponses) where T : OrganizationRequest
        {
            if (requests == null || requests.Count == 0)
            {
                return new ExecuteTransactionResponse();
            }

            var multipleRequest = new ExecuteTransactionRequest()
            {
                Requests = new OrganizationRequestCollection(),
                ReturnResponses = returnResponses,
            };

            multipleRequest.Requests.AddRange(requests);

            return (ExecuteTransactionResponse)service.Execute(multipleRequest);
        }

        public static List<ExecuteMultipleResponseItem> BulkExecute<T>(this IOrganizationService service, List<T> requests, bool continueOnError, bool returnResponses) where T : OrganizationRequest
        {
            if (requests == null || requests.Count == 0)
            {
                return new List<ExecuteMultipleResponseItem>();
            }

            var multipleRequest = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = continueOnError,
                    ReturnResponses = returnResponses
                },
                Requests = new OrganizationRequestCollection()
            };

            multipleRequest.Requests.AddRange(requests);

            return service.ExecuteMultiple(multipleRequest);
        }

        private static List<ExecuteMultipleResponseItem> ExecuteMultiple(this IOrganizationService service, ExecuteMultipleRequest multipleRequest)
        {
            var separatedRequests = new List<ExecuteMultipleRequest>();
            var pageNumber = 0;
            const int pageSize = MaxRequestsInExecuteMultiple;
            List<OrganizationRequest> requestsPaged;
            do
            {
                requestsPaged = multipleRequest.Requests.Skip(pageSize * pageNumber).Take(pageSize).ToList();
                if (requestsPaged.Any())
                {
                    var separatedRequest = new ExecuteMultipleRequest()
                    {
                        Settings = new ExecuteMultipleSettings()
                        {
                            ContinueOnError = multipleRequest.Settings.ContinueOnError,
                            ReturnResponses = multipleRequest.Settings.ReturnResponses
                        },
                        Requests = new OrganizationRequestCollection()
                    };
                    separatedRequest.Requests.AddRange(requestsPaged);
                    separatedRequests.Add(separatedRequest);
                    pageNumber++;
                }
            }
            while (requestsPaged.Count > 0);

            var responses = new List<ExecuteMultipleResponseItem>();
            separatedRequests.ForEach(separatedRequest =>
            {
                var separatedResponse = (ExecuteMultipleResponse)service.Execute(separatedRequest);

                if (separatedResponse.Responses != null && separatedResponse.Responses.Any())
                {
                    responses.AddRange(separatedResponse.Responses);
                }
            });
            return responses;
        }
    }
}
