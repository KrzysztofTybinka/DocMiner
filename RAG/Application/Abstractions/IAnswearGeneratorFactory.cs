

using Domain.Message;

namespace Application.Abstractions
{
    public interface IAnswearGeneratorFactory
    {
        IAnswearGenerator CreateAnswearGenerator(
            Dictionary<string, object>? parameters);
    }
}
