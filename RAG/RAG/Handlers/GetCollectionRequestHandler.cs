using ChromaDB.Client;
using ChromaDB.Client.Models;
using Domain.Abstractions;
using RAG.Abstractions;
using RAG.Requests;

namespace RAG.Handlers
{
    public static class GetCollectionRequestHandler
    {
        public static async Task<IResult> Handle(this GetCollectionRequest request)
        {
            //Get propper collection
            var queryhandlerResult = await request
                .QueryhandlerFactory
                .CreateHandlerAsync(request.CollectionName);

            if (!queryhandlerResult.IsSuccess)
            {
                return queryhandlerResult.ToProblemDetails();
            }

            var queryHandler = queryhandlerResult.Data;

            var ids = request.Ids == null || request.Ids.Length == 0 ? null 
                : request.Ids
                .Select(id => new Guid(id))
                .ToList();

            //Query collection
            var result = await queryHandler.Handle(
                ids,
                request.Source);

            return Results.Ok(result);
        }
    }
}
