using fancy_boxes.src;
using System.IO;

namespace FancyBoxes.UnitTests
{
    internal static class TestHelpers
    {
        public static string GetFullPathForInputFile(string fileName)
        {
            var testCasesLocationDirName = "tests";
            var pathToFile = Util.GetSolutionPath() + Path.DirectorySeparatorChar + testCasesLocationDirName + Path.DirectorySeparatorChar + fileName;
            return pathToFile;
        }
    }
}
