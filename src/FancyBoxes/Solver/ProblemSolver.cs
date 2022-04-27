using fancy_boxes.src.Implementations;
using fancy_boxes.src.Interfaces;
using System.Collections.Generic;

namespace fancy_boxes.src
{
    public class ProblemSolver
    {
        private readonly IInputSource input;
        private readonly IResult<IList<string>> inputReadResult;
        private Dictionary<string, IEnumerable<Box>> boxes;

        public ProblemSolver(IInputSource input)
        {
            this.input = input;
            inputReadResult = TryProcessSourceData();
        }

        public IResult<int> CountBoxColorsContainingColor(string targetColor)
        {
            var result = new Result<int>();

            if (!inputReadResult.IsSuccess)
            {
                result.ErrorMessage = inputReadResult.ErrorMessage;
                return result;
            }

            var colorsThatContainTargetColor = new HashSet<string>();
            var colorsThatDoNotContainTargetColor = new HashSet<string>();

            foreach(var kvp in boxes)
            {
                var targetIsInside = false;

                var boxesInsideCurrentBox = kvp.Value;
                var colorCurrentBox = kvp.Key;

                // Can also use a stack here, depends on what we need. Stack looks to be depth first, queue - breadth first.
                var boxesToBeProcessed = new Queue<Box>(boxesInsideCurrentBox);

                while (boxesToBeProcessed.Count > 0)
                {
                    var box = boxesToBeProcessed.Dequeue();

                    if (box.color == targetColor || colorsThatContainTargetColor.Contains(box.color))
                    {
                        targetIsInside = true;
                        break;
                    }

                    // early exit, we use this set to track box colors that we've already checked and we know for a fact don't contain this color
                    if (colorsThatDoNotContainTargetColor.Contains(box.color))
                    {
                        break;
                    }

                    var containsBoxesInside = boxes.TryGetValue(box.color, out IEnumerable<Box> boxesInsideDeepBox);

                    if (!containsBoxesInside)
                    {
                        continue;
                    }

                    // TODO: find out if a dataset can contain infinite loops ( boxes of colors A and B are written down to be within each other)
                    // If that's the case need to think of a way to introduce an additional check
                    // to make sure that we don't get into an infinite loop in this while loop and halt with invalid format error.
                    foreach (var boxToEnqueue in boxesInsideDeepBox)
                    {
                        boxesToBeProcessed.Enqueue(boxToEnqueue);
                    }
                }

                if (targetIsInside)
                {
                    colorsThatContainTargetColor.Add(colorCurrentBox);
                }
                else
                {
                    colorsThatDoNotContainTargetColor.Add(colorCurrentBox);
                }
            }


            result.Value = colorsThatContainTargetColor.Count;
            result.IsSuccess = true;

            return result; 
        }

        public IResult<int> CountNumberOfBoxesInsideThisColor(string targetColor)
        {
            var result = new Result<int>();

            if (!inputReadResult.IsSuccess)
            {
                result.ErrorMessage = inputReadResult.ErrorMessage;
                return result;
            }

            var colorExistsInSet = CheckIfColorExists(targetColor);
            if (!colorExistsInSet)
            {
                result.ErrorMessage = $"Unable to find a main box of {targetColor} color in the set. Please check your input and try again.";
                return result;
            }

            var boxCount = 0;

            boxes.TryGetValue(targetColor, out IEnumerable<Box> targetBoxContents);

            var boxesToBeProcessed = new Queue<BoxWithParentCount>();
            foreach (var box in targetBoxContents)
            {
                var boxWithParentInfo = Util.ConvertBoxToEnhancedBox(box);
                // all the initial boxes are inside 1 target color box
                boxWithParentInfo.parentCount = 1;
                boxesToBeProcessed.Enqueue(boxWithParentInfo);
            }

            while(boxesToBeProcessed.Count > 0)
            {
                var box = boxesToBeProcessed.Dequeue();
                boxCount += box.count * box.parentCount;
                boxes.TryGetValue(box.color, out var nestedBoxes);

                // Also handling an edge case of a box not having its contents listed in data source.
                // TODO: Technically could be considered an illegal data source, need to clarify.
                if (nestedBoxes == null)
                {
                    continue;
                }

                // All the boxes that are inside need to be processed.
                foreach (var boxToEnqueue in nestedBoxes)
                {
                    var boxWithParentInfo = Util.ConvertBoxToEnhancedBox(boxToEnqueue);
                    boxWithParentInfo.parentCount = box.count * box.parentCount;
                    boxesToBeProcessed.Enqueue(boxWithParentInfo);
                }
            }

            result.Value = boxCount;
            result.IsSuccess = true;

            return result;
        }

        private bool CheckIfColorExists(string targetColor)
        {
            var colorExistsInDataset = boxes.ContainsKey(targetColor);
            return colorExistsInDataset;
        }

        private IResult<IList<string>> TryProcessSourceData()
        {
            var inputReadResult = input.Read();

            if (inputReadResult.IsSuccess)
            {
                var parseResult = LineParser.ParseLines(inputReadResult.Value);
                if (parseResult.IsSuccess)
                {
                    boxes = parseResult.Value;
                }
                else
                {
                    inputReadResult.IsSuccess = false;
                    inputReadResult.ErrorMessage = parseResult.ErrorMessage;
                }
            }

            return inputReadResult;
        }
    }
}
