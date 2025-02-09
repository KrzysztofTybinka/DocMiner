using Domain.Abstractions;

namespace Domain.ProcessedDocument
{
    public static class DocumentChunker
    {
        public static Result<List<string>> ChunkContent(
            string content,
            int tokenAmount,
            string[] delimiters
        )
        {
            //Clean up result
            content = content
                .Replace("\r\n", " ")
                .Replace("\n", " ");

            string[] splittedBySpace = content.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            List<string> chunkedByTokenNumber = [];

            //Iterate over the tokens and build chunks containing at least _tokenAmount tokens
            for (int from = 0, to = tokenAmount - 1; to < splittedBySpace.Length - 1; to++)
            {
                //To keep the context we can sligtly extend token number
                //and find the end of the sentence, but the chunk should't 
                //exceed the amount of tokens by too much, that's why _tokenAmount * 1.2
                if (delimiters.Any(splittedBySpace[to].Contains) || to == tokenAmount * 1.2)
                {
                    chunkedByTokenNumber.Add(
                        string.Join(" ",
                        splittedBySpace
                        .Skip(from)
                        .Take(to)));

                    //Example for token number: 10 -
                    //First chunk found from [0] to [13]
                    //so we need to start searching for the second
                    //chunk from [14] to [24]
                    from = to + 1;
                    to = from + tokenAmount - 1; //-1 because next iteration will to++
                }
            }

            return Result<List<string>>.Success(chunkedByTokenNumber);
        }
    }
}
