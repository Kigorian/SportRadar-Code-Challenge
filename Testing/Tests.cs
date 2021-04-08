using Newtonsoft.Json;
using NHL_API.Model;
using NHL_API.Services;
using NUnit.Framework;
using System;

namespace Testing
{
    public class Tests
    {
        private static string _nhlApiUrl { get; set; }

        [SetUp]
        public void Setup()
        {
            _nhlApiUrl = "https://statsapi.web.nhl.com/api/v1";
        }

        #region Team Pipeline

        [TestCase(1, 1950)]
        [TestCase(1, 2020)]
        public void Test_TryGetTeamData(int teamId, int year)
        {
            #region Expected Values

            var expectedTeam = new Team(); ;
            switch (year)
            {
                case 1950:
                    expectedTeam.ID = 1;
                    expectedTeam.Name = "New Jersey Devils";
                    expectedTeam.VenueName = "Prudential Center";
                    expectedTeam.GamesPlayed = 0;
                    expectedTeam.Wins = 0;
                    expectedTeam.Losses = 0;
                    expectedTeam.Points = 0;
                    expectedTeam.GoalsPerGame = 0;
                    expectedTeam.SeasonFirstGameDate = new DateTimeOffset();
                    expectedTeam.SeasonFirstGameOpponent = null;
                    break;

                case 2020:
                    expectedTeam.ID = 1;
                    expectedTeam.Name = "New Jersey Devils";
                    expectedTeam.VenueName = "Prudential Center";
                    expectedTeam.GamesPlayed = 37;
                    expectedTeam.Wins = 13;
                    expectedTeam.Losses = 18;
                    expectedTeam.Points = 32;
                    expectedTeam.GoalsPerGame = 2.459m;
                    expectedTeam.SeasonFirstGameDate = DateTimeOffset.Parse("2021-01-15T00:00:00+00:00");
                    expectedTeam.SeasonFirstGameOpponent = "Boston Bruins";
                    break;
            }

            #endregion Expected Values

            var result = ApiService.TryGetTeamData(teamId, year, _nhlApiUrl);

            // There should be a result and no error.
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.EntityResult);

            var team = (Team)result.EntityResult;

            // Serialize to JSON so we can compare it to the expected values.
            var expectedTeamJson = JsonConvert.SerializeObject(expectedTeam);
            var teamJson = JsonConvert.SerializeObject(team);
            Assert.AreEqual(expectedTeamJson, teamJson);
        }

        [Test]
        public void Test_TryGetTeamData_NonExistentTeam()
        {
            var result = ApiService.TryGetTeamData(teamId: 9999, year: 2020, _nhlApiUrl);

            // There should be an error and no result.
            Assert.IsNull(result.EntityResult);
            Assert.IsNotNull(result.Exception);
            Assert.AreEqual(
                "The remote server returned an error: (404) Not Found.",
                result.Exception.Message
            );
        }

        #endregion Team Pipeline

        #region Player Pipeline

        // This would be nearly identical to the Team Pipeline tests.

        #endregion Player Pipeline

        #region JSON Converters

        // Some tests to compare the expected value results of the JSON Converters.
        // We could copy some JSON taken from an API result, and compare it to some
        // expected Team/Player/etc. values similar to the Team Pipeline tests.

        #endregion JSON Converters

        #region CSV

        // Tests to compare an expected CSV string to the one generated for a given
        // sample Team/Player.

        #endregion CSV
    }
}