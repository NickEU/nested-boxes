using fancy_boxes.src.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace fancy_boxes.src.Implementations
{
    public class FileReader : IInputSource
    {
        private readonly string _fileName;
        public FileReader(string fileName = "")
        {
            _fileName = fileName;
        }

        public IResult<IList<string>> Read(string source)
        {
            var result = new Result<IList<string>>();
            var lines = new List<string>();
            var fileName = string.IsNullOrEmpty(source) ? _fileName : source;

            if (string.IsNullOrEmpty(fileName))
            {
                result.ErrorMessage = Messages.NO_FILENAME_SPECIFIED;
                return result;
            }

            try
            {
                using (StreamReader sr = File.OpenText(fileName))
                {
                    string line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
            }
            catch(Exception ex)
            {
                result.ErrorMessage = ex.Message;
                return result;
            }

            result.Value = lines;
            result.IsSuccess = true;
            return result;
        }
    }
}
