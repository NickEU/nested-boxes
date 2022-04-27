using System;
using System.IO;

namespace fancy_boxes.src
{
    public class Util
    {
        public static string GetSolutionPath()
        {
            var workingDirectory = Environment.CurrentDirectory;
            var solutionDir = Directory.GetParent(workingDirectory).Parent.Parent.Parent.Parent.FullName;
            return solutionDir;
        }

        internal static BoxWithParentCount ConvertBoxToEnhancedBox(Box box)
        {
            // TODO: should be using an automapper for this - ran out of time
            // An alternative is to serialize/deserialize to JSON. More general, but probably a hack though.
            BoxWithParentCount result = new()
            {
                count = box.count,
                color = box.color
            };
            return result;
        }
    }
}
