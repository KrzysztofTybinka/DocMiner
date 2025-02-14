

using Domain.Abstractions;

namespace Tests.Application.UnitTests
{
    public static class MockErrors
    {
        public static Error TestError => Error.Failure(
            "MockErrors.TestError",
            "This error is for testing puroposes"
        );
    }
}
