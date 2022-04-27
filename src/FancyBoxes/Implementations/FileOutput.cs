using System.IO;
using System.Threading.Tasks;

namespace fancy_boxes.src.Implementations
{
    internal class FileOutput
    {
        private readonly string fileName;

        public FileOutput(string fileName)
        {
            this.fileName = fileName;
        }

        public async Task WriteAsync(string message)
        {
            await File.WriteAllTextAsync(fileName, message);
        }
    }
}
