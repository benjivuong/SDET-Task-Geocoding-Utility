using Moq;
using System.Text;

namespace Utility_Integration_Tests
{
    [TestClass]
    public class AppTests
    {
        StringBuilder _ConsoleOutput;
        Mock<TextReader> _ConsoleInput;

        [TestInitialize]
        public void TestInitialize()
        {
            _ConsoleOutput = new StringBuilder();
            var consoleOutputWriter = new StringWriter(_ConsoleOutput);
            _ConsoleInput = new Mock<TextReader>();
            Console.SetOut(consoleOutputWriter);
            Console.SetIn(_ConsoleInput.Object);
        }

        private MockSequence SetupUserResponses(params string[] userResponses)
        {
            var sequence = new MockSequence();
            foreach (var response in userResponses)
                _ConsoleInput.InSequence(sequence).Setup(x => x.ReadLine()).Returns(response);
            return sequence;
        }

        private string[] RunMainAndGetConsoleOutput()
        {
            SDETTaskGeocodingUtility.Program.Main();
            return _ConsoleOutput.ToString().Split("\r\n");
        }

        [TestMethod]
        public void Main_AsksForALocation_WhenExecuted()
        {
            SetupUserResponses("53766", "QUIT");
            var expectedPrompt = "Enter a location name or a zip code (Enter QUIT to stop): ";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.AreEqual(expectedPrompt, outputLines[0]);
        }

        [TestMethod]
        public void Main_ReturnsNotFound_WhenZipIsInvalid()
        {
            SetupUserResponses("53766", "QUIT");
            var expectedResult = "Not Found";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.AreEqual(expectedResult, outputLines[3]);
        }

        [TestMethod]
        public void Main_ReturnsCorrectInformation_WhenZipIsValid()
        {
            SetupUserResponses("12345", "QUIT");
            var expectedSubstring = "Name: Schenectady\tZip: 12345";
                
            var outputLines = RunMainAndGetConsoleOutput();

            Assert.IsTrue(outputLines[3].Contains(expectedSubstring));
        }

        [TestMethod]
        public void Main_ReturnsCorrectInformation_WhenLocationNameIsValid()
        {
            SetupUserResponses("Madison,WI", "QUIT");
            var expectedSubstring = "Latitude: 43.07476\tLongitude: -89.38376";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.IsTrue(outputLines[3].Contains(expectedSubstring));
        }

        [TestMethod]
        public void Main_ReturnsMultipleLines_WhenMultipleValuesAreProvided()
        {
            SetupUserResponses("Madison,WI", "12345", "QUIT");
            var expectedNumOutputs = 8;

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.AreEqual(expectedNumOutputs, outputLines.Length);
        }

        [TestMethod]
        public void Main_ReturnsEmptyLine_WhenNoLocationsAreFound()
        {
            SetupUserResponses("Neverland,WI", "QUIT");
            var expectedResult = "";

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.AreEqual(expectedResult, outputLines[3]);
        }

        [TestMethod]
        public void Main_ReturnsOnlyTwoLines_WhenUserQuitsImmediately()
        {
            SetupUserResponses("QUIT");
            var expectedNumOutputs = 2;

            var outputLines = RunMainAndGetConsoleOutput();

            Assert.AreEqual(expectedNumOutputs, outputLines.Length);
        }
    }
}