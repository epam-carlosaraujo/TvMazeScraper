namespace Scraper.Types.TvMazeAPI
{
    public class TvMazeAPIEndpoints
    {
        public string Shows { get; set; }
        public string Cast { get; set; }

        public string ForShows(int page)
        {
            return string.Format(Shows, page);
        }

        public string ForCast(int showId)
        {
            return string.Format(Cast, showId);
        }
    }
}
