using RAG.Common;
using RAG.Requests;

namespace RAG.Validators
{
    public static class QueryCollectionRequestValidator
    {
        public static ValidationResult IsValid(this QueryCollectionRequest request)
        {
            var result = new ValidationResult();

            if (string.IsNullOrEmpty(request.CollectionName))
            {
                result.Errors.Add("Collection Id cannot be empty.");
                result.IsValid = false;
                return result;
            }

            if (request.Prompts.Length == 0)
            {
                result.Errors.Add("Your need to provide aprompt message.");
                result.IsValid = false;
                return result;
            }

            result.IsValid = true;
            return result;
        }
    }
}
