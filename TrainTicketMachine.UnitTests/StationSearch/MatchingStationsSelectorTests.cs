using System.Collections.Generic;
using System.Linq;
using TrainTicketMachine.Models;
using TrainTicketMachine.StationSearch;
using Xunit;

namespace TrainTicketMachine.UnitTests.StationSearch
{
    public class MatchingStationsSelectorTests
    {
        [Fact]
        public void GetMatchingStationsSucceeded()
        {
            const string userInput = "ab";
            var stations = new List<Station>
            {
                new Station {StationCode = "ABC", StationName = "Abc Name"},
                new Station {StationCode = "DEF", StationName = "Def Name"},
                new Station {StationCode = "ABW", StationName = "Abbey Wood"}
            };
            var expectedMatchingStations = new List<Station>
            {
                new Station {StationCode = "ABC", StationName = "Abc Name"},
                new Station {StationCode = "ABW", StationName = "Abbey Wood"}
            };

            var stationsSelector = new MatchingStationsSelector(stations);
            var matchingStations = stationsSelector.GetMatchingStations(userInput);
            
            Assert.True(expectedMatchingStations.All(
                expectedStation => matchingStations.Any(
                    station => station.StationName.Equals(expectedStation.StationName))));
            
        }

        [Fact]
        public void GetMatchingStationsShouldReturnEmptyListWhenThereIsNoMatch()
        {
            const string userInput = "ab";
            var stations = new List<Station>
            {
                new Station {StationCode = "DEF", StationName = "Def Name"}
            };
            var expectedStations = new List<Station>();

            var stationsSelector = new MatchingStationsSelector(stations);
            var matchingStations = stationsSelector.GetMatchingStations(userInput);

            Assert.Equal(expectedStations, matchingStations);
        }
    }
}
