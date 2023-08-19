using Flurl;
using Flurl.Http;

namespace QuizDb.Module
{
    public static class QuizDbGetter
    {
        private const string HOST = "https://db.chgk.info";
        private const string TOURNAMENTS_LIST_URL = "last";
        private const string TOURNAMENT_URL = "tour";

        public static async Task<string> GetTournamentList(int page = 0)
        {
            var dbUrl = BuildUrl(TOURNAMENTS_LIST_URL, GetTournamentsQueryParams(page));
            var response = await dbUrl.GetAsync();

            var htmlContent = await response.GetStringAsync();            

            return htmlContent;
        }

        public static async Task<string> GetTournamentQuestions(string link)
        {
            var url = $"{TOURNAMENT_URL}/{link}";

            var dbUrl = BuildUrl(url);
            var response = await dbUrl.GetAsync();

            var htmlContent = await response.GetStringAsync();

            return htmlContent;
        }

        private static Url BuildUrl(string path = null, Dictionary<string, object> queryParams = null)
        {
            var url = new Url(HOST);

            if (!string.IsNullOrWhiteSpace(path))
            {
                url = url.AppendPathSegment(path);
            }

            queryParams?.Keys.ToList().ForEach(name => url = url.SetQueryParam(name, queryParams[name]));

            return url;
        }

        public static Dictionary<string, object> GetTournamentsQueryParams(int page)
        {
            var queryParams = new Dictionary<string, object>();

            if (page == 0)
            {
                return queryParams;
            }

            queryParams.Add("page", page);

            return queryParams;
        }
    }
}
