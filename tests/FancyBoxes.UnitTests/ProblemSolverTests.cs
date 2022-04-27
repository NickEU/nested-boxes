using fancy_boxes.src;
using fancy_boxes.src.Implementations;
using fancy_boxes.src.Interfaces;
using FancyBoxes.UnitTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FancyBoxesTester
{
    [TestClass]
    public class ProblemSolverTests
    {
        private const string SHINY_GOLD = "shiny gold";

        [TestMethod]
        public void CountsBoxesCorrectly()
        {
            // I can't include the input test files in Git, will have to send the test cases I've created via email.
            // Hosting them somewhere and downloading them over network during the first test run looks a bit overkill for the scope of the task
            // and it would still ultimately expose the files to third parties which is not something we want.
            const string inputFileName = "input-test-box-count.txt";
            var pathToFile = TestHelpers.GetFullPathForInputFile(inputFileName);
            var fileReader = new FileReader(pathToFile);

            var solver = new ProblemSolver(fileReader);
            var numOfBoxes = solver.CountNumberOfBoxesInsideThisColor(SHINY_GOLD);

            Assert.IsTrue(numOfBoxes.IsSuccess);

            const int expectedBoxCount = 126;
            var actualBoxCount = numOfBoxes.Value;

            Assert.AreEqual(expectedBoxCount, actualBoxCount);
        }

        [TestMethod]
        public void CountsZeroBoxesCorrectly()
        {
            const string inputFileName = "input-test-box-count-zero.txt";
            var pathToFile = TestHelpers.GetFullPathForInputFile(inputFileName);
            var fileReader = new FileReader(pathToFile);

            var solver = new ProblemSolver(fileReader);
            var numOfBoxes = solver.CountNumberOfBoxesInsideThisColor(SHINY_GOLD);

            Assert.IsTrue(numOfBoxes.IsSuccess);

            const int expectedBoxCount = 0;
            var actualBoxCount = numOfBoxes.Value;

            Assert.AreEqual(expectedBoxCount, actualBoxCount);
        }

        [TestMethod]
        public void CountsColorsCorrectly()
        {
            const string inputFileName = "input-test-color-count.txt";
            var pathToFile = TestHelpers.GetFullPathForInputFile(inputFileName);
            var fileReader = new FileReader(pathToFile);

            var solver = new ProblemSolver(fileReader);
            var numofColors = solver.CountBoxColorsContainingColor(SHINY_GOLD);

            Assert.IsTrue(numofColors.IsSuccess);

            const int expectedColorCount = 4;
            var actualColorCount = numofColors.Value;

            Assert.AreEqual(expectedColorCount, actualColorCount);
        }

        [TestMethod]
        public void CountsZeroColorsCorrectly()
        {
            const string inputFileName = "input-test-color-count-zero.txt";
            var pathToFile = TestHelpers.GetFullPathForInputFile(inputFileName);
            var fileReader = new FileReader(pathToFile);

            var solver = new ProblemSolver(fileReader);
            var numofColors = solver.CountBoxColorsContainingColor(SHINY_GOLD);

            Assert.IsTrue(numofColors.IsSuccess);

            const int expectedColorCount = 0;
            var actualColorCount = numofColors.Value;

            Assert.AreEqual(expectedColorCount, actualColorCount);
        }

        [TestMethod]
        public void NoFileExistsFails()
        {
            const string inputFileName = "i-do-not-exist.txt";
            var pathToFile = TestHelpers.GetFullPathForInputFile(inputFileName);
            var fileReader = new FileReader(pathToFile);

            var solver = new ProblemSolver(fileReader);
            var numofColors = solver.CountBoxColorsContainingColor(SHINY_GOLD);

            Assert.IsFalse(numofColors.IsSuccess);

            var expectedErrMsg = "Could not find file";
            var actualErrMsg = numofColors.ErrorMessage;
            StringAssert.StartsWith(actualErrMsg, expectedErrMsg);
        }

        [TestMethod]
        public void NoFilenameSpecifiedFails()
        {
            var fileReader = new FileReader();
            var solver = new ProblemSolver(fileReader);

            var numofColors = solver.CountBoxColorsContainingColor(SHINY_GOLD);

            Assert.IsFalse(numofColors.IsSuccess);

            var expectedErrMsg = Messages.NO_FILENAME_SPECIFIED;
            var actualErrMsg = numofColors.ErrorMessage;
            Assert.AreEqual(actualErrMsg, expectedErrMsg);
        }

        [TestMethod]
        public void InvalidFileFormatFails()
        {
            const string inputFileName = "input-test-color-count-duplicate-entry.txt";
            var pathToFile = TestHelpers.GetFullPathForInputFile(inputFileName);
            var fileReader = new FileReader(pathToFile);

            var solver = new ProblemSolver(fileReader);
            var numofColors = solver.CountBoxColorsContainingColor(SHINY_GOLD);

            Assert.IsFalse(numofColors.IsSuccess);

            var expectedErrMsg = Messages.INVALID_FILE_FORMAT;
            var actualErrMsg = numofColors.ErrorMessage;
            Assert.AreEqual(actualErrMsg, expectedErrMsg);
        }

        [TestMethod]
        public void ConsoleInputWorksCorrectly()
        {
            const string inputFileName = "input-test-box-count.txt";
            var pathToFile = TestHelpers.GetFullPathForInputFile(inputFileName);
            IInputSource fileReader = new FileReader(pathToFile);

            var linesFromFile = fileReader.Read().Value;
            Assert.AreNotEqual(0, linesFromFile.Count);

            var sourceText = string.Join("\n", linesFromFile);
            var stringReader = new StringSource(sourceText);
            var solver = new ProblemSolver(stringReader);
            var numOfBoxes = solver.CountNumberOfBoxesInsideThisColor(SHINY_GOLD);
            System.Console.WriteLine(numOfBoxes.ErrorMessage);
            Assert.IsTrue(numOfBoxes.IsSuccess);

            const int expectedBoxCount = 126;
            var actualBoxCount = numOfBoxes.Value;

            Assert.AreEqual(expectedBoxCount, actualBoxCount);

        }
    }
}
