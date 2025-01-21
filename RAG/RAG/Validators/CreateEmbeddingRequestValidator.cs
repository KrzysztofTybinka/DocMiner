using RAG.Requests;
using RAG.Common;
namespace RAG.Validators
{
    public static class CreateEmbeddingRequestValidator
    {
        public static ValidationResult IsValid(this CreateEmbeddingRequest request)
        {
            var result = new ValidationResult();

            if (request.File == null)
            {
                result.Errors.Add("No file was uploaded.");
                result.IsValid = false;
                return result;
            }

            if (request.NumberOfTokens < 50)
            {
                result.Errors.Add("Number of tokens can't be less than 50.");
                result.IsValid = false;
                return result;
            }

            if (String.IsNullOrEmpty(request.File.FileName))
            {
                result.Errors.Add("File name cannot be empty.");
                result.IsValid = false;
                return result;
            }

            result.IsValid = true;
            return result;
        }
    }
}
