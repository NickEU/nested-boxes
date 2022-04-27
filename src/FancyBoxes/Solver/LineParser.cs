using fancy_boxes.src.Implementations;
using fancy_boxes.src.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fancy_boxes.src
{
    internal class LineParser
    {
        internal static IResult<Dictionary<string, IEnumerable<Box>>> ParseLines(IList<string> lines)
        {
            var result = new Result<Dictionary<string, IEnumerable<Box>>>();
            var colorsToContents = new Dictionary<string, IEnumerable<Box>>();
            try
            {
                foreach (var line in lines)
                {
                    // An alternative to splitting into substrings is to use regexp
                    var splitNameToContents = line.Split("boxes contain");
                    var lineColor = splitNameToContents[0].Trim();

                    var boxes = new List<Box>();

                    var boxesInside = splitNameToContents[1].Split(',');
                    foreach (var colorAndCountRaw in boxesInside)
                    {
                        if (colorAndCountRaw.Contains("no other boxes."))
                            break;
                        var colorAndCountSplit = colorAndCountRaw.Split("box")[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                        var count = int.Parse(colorAndCountSplit[0]);
                        var color = string.Join(" ", colorAndCountSplit.Skip(1)).Trim();
                        // TODO: can store color as enum to save some memory if the datasets can be really big.
                        var boxInfo = new Box { color = color, count = count };
                        boxes.Add(boxInfo);
                    }

                    // TODO: is duplicate entry for the same color an illegal file format or is that something we should ignore or overwrite?
                    // need to clarify. is considered invalid and shows an illegal format msg for now
                    colorsToContents.Add(lineColor, boxes);

                }
                result.IsSuccess = true;
                result.Value = colorsToContents;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result.ErrorMessage = Messages.INVALID_FILE_FORMAT;
            }

            return result;
        }
    }
}
