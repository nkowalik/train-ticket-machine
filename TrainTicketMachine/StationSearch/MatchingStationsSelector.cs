using System.Collections.Generic;
using System.Linq;
using TrainTicketMachine.Models;

namespace TrainTicketMachine.StationSearch
{
    public class MatchingStationsSelector
    {
        public List<Station> CompleteStationsList { get; }

        public MatchingStationsSelector(List<Station> completeStationsList)
        {
            CompleteStationsList = completeStationsList;
        }

        /// <summary>
        /// Searches for matching names of train stations based on characters entered by the user.
        /// </summary>
        /// <param name="firstChars">A <see cref="string"/> that represents the characters typed by the user.</param>
        /// <returns>List of <see cref="Station"/> instances that represents train stations that begin with the string specified by the user.</returns>
        public List<Station> GetMatchingStations(string firstChars)
        {
            var matchingStations = CompleteStationsList.Where(
                station => station.StationName.ToLower().StartsWith(firstChars.ToLower()))
                .Distinct().ToList();

            return matchingStations;
        }
    }
}
