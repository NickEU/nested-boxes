using fancy_boxes.src;
using fancy_boxes.src.Implementations;
using fancy_boxes.src.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace fancy_boxes
{
    internal class Program
    {
        readonly IOutput cli = new ConsoleOutput();

        static async Task Main(string[] args)
        {
            var program = new Program();
            await program.Run();
        }

        private async Task Run()
        {
            const string inputFileName = "input.txt";
            const string outputFileName = "README.MD";
            const string targetColor = "shiny gold";

            var pathToFile = Util.GetSolutionPath() + Path.DirectorySeparatorChar + inputFileName;
            var fileReader = new FileReader(pathToFile);

            var solver = new ProblemSolver(fileReader);

            cli.Write("Trying to solve problems with nested boxes..." + Environment.NewLine);

            var numofColors = solver.CountBoxColorsContainingColor(targetColor);
            if (!numofColors.IsSuccess)
            {
                cli.Write(numofColors.ErrorMessage);
                return;
            }

            var answerFirstQuestion = $"Answer to question 1: {numofColors.Value} colors can eventually contain at least one {targetColor} box!";
            cli.Write(answerFirstQuestion);

            var numOfBoxes = solver.CountNumberOfBoxesInsideThisColor(targetColor);
            if (!numOfBoxes.IsSuccess)
            {
                cli.Write(numOfBoxes.ErrorMessage);
                return;
            }

            var answerSecondQuestion = $"Answer to question 2: {numOfBoxes.Value} individual boxes are found inside one {targetColor} box!";
            cli.Write(answerSecondQuestion);

            var answer = answerFirstQuestion + Environment.NewLine + answerSecondQuestion;
            cli.Write($"Successfully solved both problems, writing the answers to {outputFileName} in root directory of the solution!");
            await WriteAnswersToFile(outputFileName, answer);

            cli.Write(Environment.NewLine + Messages.FINISHED_PROGRAM);
            Console.ReadKey();
        }

        private async Task WriteAnswersToFile(string fileName, string answer)
        {
            try
            {
                var projectPath = Util.GetSolutionPath();
                var writeFileToPath = projectPath + Path.DirectorySeparatorChar + fileName;
                var fileWriter = new FileOutput(writeFileToPath);

                await fileWriter.WriteAsync(answer);
            }
            catch (Exception ex)
            {
                cli.Write(ex.Message);
            }
        }
    }
}
