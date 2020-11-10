using System.Collections.Generic;
using TrainTicketMachine.Models;
using TrainTicketMachine.StationSearch;
using Xunit;

namespace TrainTicketMachine.UnitTests.StationSearch
{
    public class MatchingNextCharactersSelectorTests
    {
        [Fact]
        public void GetNextCharsSucceeded()
        {
            const string userInput = "ab";
            var matchingStations = new List<Station>
            {
                new Station {StationCode = "ABC", StationName = "Abc Name"},
                new Station {StationCode = "ABW", StationName = "Abbey Wood"}
            };
            var expectedNextChars = new List<char> {'C', 'B'};

            var selector = new MatchingNextCharactersSelector(matchingStations);
            var nextCharsForUserInput = selector.GetNextChars(userInput);

            Assert.Equal(expectedNextChars, nextCharsForUserInput);
        }

        [Fact]
        public void GetNextCharsShouldRemoveDuplicatesInResults()
        {
            const string userInput = "ab";
            var matchingStations = new List<Station>
            {
                new Station {StationCode = "ABC", StationName = "Abc Name"},
                new Station {StationCode = "ABC", StationName = "Abc"}
            };
            var expectedNextChars = new List<char> { 'C' };

            var selector = new MatchingNextCharactersSelector(matchingStations);
            var nextCharsForUserInput = selector.GetNextChars(userInput);

            Assert.Equal(expectedNextChars, nextCharsForUserInput);
        }

        [Fact]
        public void GetNextCharsShouldReturnEmptyListWhenThereIsNoMatchingStation()
        {
            const string userInput = "ab";
            var matchingStations = new List<Station>();
            var expectedNextChars = new List<char>();

            var selector = new MatchingNextCharactersSelector(matchingStations);
            var nextCharsForUserInput = selector.GetNextChars(userInput);

            Assert.Equal(expectedNextChars, nextCharsForUserInput);
        }
    }
}
