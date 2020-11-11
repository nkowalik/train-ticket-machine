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

        /// <summary>
        /// Searches for the next matching character based on characters entered by the user and the list of stations whose names begin with the characters entered.
        /// </summary>
        /// <param name="firstChars">A string that represents the characters typed by the user.</param>
        /// <returns>A list of all characters that can be entered by user.</returns>
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
