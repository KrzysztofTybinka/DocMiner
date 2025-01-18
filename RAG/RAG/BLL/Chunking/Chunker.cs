using System.Text.RegularExpressions;

namespace RAG.BLL.Chunking
{
    public class Chunker
    {
        private int _tokenAmount;
        private string _text;

        public Chunker(int tokenAmount, string text)
        {
            _tokenAmount = tokenAmount;
            _text = text;
        }

        public List<string> GetChunks()
        {
            string[] splittedBySpace = _text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] delimiters = new[] { ";", ".", "?", "!" };
            List<string> chunkedByTokenNumber = [];

            //Iterate over the tokens and build chunks containing at least _tokenAmount tokens
            for (int from = 0, to = _tokenAmount - 1; to < splittedBySpace.Length - 1; to++)
            {
                //To keep the context we can sligtly extend token number
                //and find the end of the sentence, but the chunk should't 
                //exceed the amount of tokens by too much, that's why _tokenAmount * 1.2
                if (delimiters.Any(splittedBySpace[to].Contains) || to == _tokenAmount * 1.2)
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
                    to = from + _tokenAmount - 1; //-1 because next iteration will to++
                }
            }

            return chunkedByTokenNumber;
        }
    }
}
