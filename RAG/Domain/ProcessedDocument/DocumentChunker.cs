using Domain.Abstractions;

namespace Domain.ProcessedDocument
{
    public static class DocumentChunker
    {
        public static Result<List<string>> ChunkContent(
            string content,
            int tokenAmount,
            string[] delimiters)
        {
            if (tokenAmount <= 0)
            {
                return Result<List<string>>.Failure(
                    Error.Failure("DocumentChunker.InvalidTokenAmount",
                    "Token amount must be greater than 0"));
            }


            // Clean up content
            content = content.ReplaceLineEndings(" ");
            var splittedBySpace = content.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var chunks = new List<string>();
            int currentIndex = 0;

            while (currentIndex + tokenAmount <= splittedBySpace.Length)
            {
                int chunkEnd = FindChunkEndIndex(splittedBySpace, currentIndex, tokenAmount, delimiters);
                chunks.Add(CreateChunk(splittedBySpace, currentIndex, chunkEnd));
                currentIndex = chunkEnd + 1;
            }

            // Add remaining tokens if they meet minimum size
            if (currentIndex < splittedBySpace.Length &&
                (splittedBySpace.Length - currentIndex) >= tokenAmount)
            {
                chunks.Add(CreateChunk(splittedBySpace, currentIndex, splittedBySpace.Length - 1));
            }

            return Result<List<string>>.Success(chunks);
        }

        private static int FindChunkEndIndex(string[] tokens, int startIndex, int baseTokenAmount, string[] delimiters)
        {
            int maxIndex = Math.Min(startIndex + (int)(baseTokenAmount * 1.2) - 1, tokens.Length - 1);
            int preferredEnd = startIndex + baseTokenAmount - 1;

            for (int i = preferredEnd; i <= maxIndex; i++)
            {
                if (delimiters.Any(d => tokens[i].Contains(d)))
                {
                    return i;
                }
            }

            // If no delimiter found, use the maximum allowed index
            return Math.Min(startIndex + baseTokenAmount - 1, tokens.Length - 1);
        }

        private static string CreateChunk(string[] tokens, int start, int end)
        {
            return string.Join(" ", tokens.Skip(start).Take(end - start + 1));
        }
    }
}
