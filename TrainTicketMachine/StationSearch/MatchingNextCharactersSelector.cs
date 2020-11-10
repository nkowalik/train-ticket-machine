using System.Collections.Generic;
using System.Linq;
using TrainTicketMachine.Models;

namespace TrainTicketMachine.StationSearch
{
    public class MatchingNextCharactersSelector
    {
        public List<Station> MatchingStations { get; }

        public MatchingNextCharactersSelector(List<Station> matchingStations)
        {
            MatchingStations = matchingStations;
        }

        public List<char> GetNextChars(string firstChars)
        {
            var nextChars = MatchingStations.Select(station =>
            {
                var matchingStationName = station.StationName.ToUpper();
                var partStationName = matchingStationName.Replace(firstChars.ToUpper(), string.Empty);

                return partStationName.Equals(string.Empty) ? ' ' : partStationName.First();
            }).Distinct().ToList();

            return nextChars;
        }
    }
}
