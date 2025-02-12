

using Domain.Abstractions;

namespace Domain.Message
{
    public interface IAnswearGenerator
    {
        Task<Result<string>> GenerateAnswearAsync(Message message);
    }
}
