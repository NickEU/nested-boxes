using fancy_boxes.src.Interfaces;
using System.Collections.Generic;

namespace fancy_boxes.src.Implementations
{
    public class StringSource : IInputSource
    {
        private string sourceText;

        public StringSource(string sourceText)
        {
            this.sourceText = sourceText;
        }

        public IResult<IList<string>> Read(string source = "")
        {
            if (!string.IsNullOrEmpty(source))
            {
                sourceText = source;
            }
            // Just a quick example class to make sure ProblemSolver works with input source type other than FileReader
            // and all the dependency injection wasn't for naught :D
            var result = new Result<IList<string>>();
            result.Value = sourceText.Split("\n");
            result.IsSuccess = true;
            return result;
        }
    }
}
