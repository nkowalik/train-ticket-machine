using System.Collections.Generic;
using System.Linq;
using TrainTicketMachine.Models;

namespace TrainTicketMachine.StationSearch
{
    public class MatchingStationsSelector
    {
        public List<Station> FullStationsList { get; }

        public MatchingStationsSelector(List<Station> fullStationsList)
        {
            FullStationsList = fullStationsList;
        }

        public List<Station> GetMatchingStations(string firstChars)
        {
            var matchingStations = FullStationsList.Where(
                station => station.StationName.ToLower().StartsWith(firstChars.ToLower()))
                .Distinct().ToList();

            return matchingStations;
        }
    }
}
