﻿using Application.Responses;
using Domain.Abstractions;
using Domain.Embedings;

namespace Application.Abstractions
{
    public interface IGetEmbeddingsByIdQueryHandler
    {
        Task<Result<List<GetEmbeddingsByIdResponse>>> Handle(
            List<Guid>? ids,
            string? sourceDetails);
    }
}
