using System.Net;
using Entity;
using Services.CrosswordInstaller.NYT;

namespace Services.CrosswordInstaller
{

    public class NYTCrosswordFetcher
    {

        private static HttpClient httpClient = new HttpClient();
        private const String baseUrl = "https://nytsyn.pzzl.com/nytsyn-crossword-mh/nytsyncrossword";

        private NYTCrosswordParser parser;

        public NYTCrosswordFetcher(NYTCrosswordParser parser)
        {
            this.parser = parser;
        }

        public async Task<Crossword?> Fetch(DateOnly date /*url date*/)
        {

            Trace.WriteLine($"fetching nyt {date.ToString()}");

            String dayStr = date.Day.ToString().PadLeft(2, '0');
            String monthStr = date.Month.ToString().PadLeft(2, '0');
            String yearStr = date.Year.ToString().Substring(2);
            String dateStr = $"?date={yearStr}{monthStr}{dayStr}";
            String url = baseUrl + dateStr;
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Trace.WriteLine("NYT Download Failed : Bad Return Code");
                return null;
            }

            NYTCrosswordData? data = parseContent(await response.Content.ReadAsStringAsync());
            if (!data.HasValue)
            {
                Trace.WriteLine("NYT Data Parse Failed");
                return null;
            }

            try
            {
                Crossword? crossword = parser.ParseData(data.Value);
                crossword.Published = data.Value.published.ToDateTime(TimeOnly.MinValue);
                crossword.Title = $"NYT {crossword.Published.ToShortDateString()}";
                return crossword;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"NYT Parse Failed : URL {url}");
                Trace.WriteLine(ex.ToString());
                return null;
            }

        }

        // TODO : make safe
        public NYTCrosswordData? parseContent(String content) 
        {

            StringReader reader = new StringReader(content);

            if (!reader.ReadLine().Equals("ARCHIVE"))
            {
                Trace.WriteLine("NYT Parser Failed : Bad Format");
                return null;
            }

            reader.ReadLine(); //blank line

            //url date and puzzle date are offset : TODO
            String realDateStr = reader.ReadLine(); // date line
            int year = int.Parse(realDateStr.ToString().Substring(0, 2)) + 2000;
            int month = int.Parse(realDateStr.ToString().Substring(2, 2));
            int day = int.Parse(realDateStr.ToString().Substring(4, 2));
            DateOnly published = new DateOnly(year, month, day);

            reader.ReadLine(); // blank line
            String bDateStr = reader.ReadLine();

            reader.ReadLine(); // blank line
            String bAuthorStr = reader.ReadLine();

            reader.ReadLine(); // blank lin
            int bRows = int.Parse(reader.ReadLine());
            reader.ReadLine(); // blank line
            int bColumns = int.Parse(reader.ReadLine());
            reader.ReadLine(); // blank line
            int bAcrossCount = int.Parse(reader.ReadLine());
            reader.ReadLine(); // blank line
            int bDownCount = int.Parse(reader.ReadLine());

            reader.ReadLine(); // blank line
                               //solution
            List<String> bSolution = new List<String>();
            String solutionRow = reader.ReadLine();
            while (solutionRow.Count() != 0)
            {
                bSolution.Add(solutionRow);
                Trace.WriteLine(solutionRow);
                solutionRow = reader.ReadLine();
            }

            //across clues
            List<String> bAcrossClues = new List<String>();
            String clueRowAcross = reader.ReadLine();
            while (clueRowAcross.Count() != 0)
            {
                bAcrossClues.Add(clueRowAcross);
                Trace.WriteLine(clueRowAcross);
                clueRowAcross = reader.ReadLine();
            }

            //down clues
            List<String> bDownClues = new List<String>();
            String clueRowDown = reader.ReadLine();
            while (clueRowDown.Count() != 0)
            {
                bDownClues.Add(clueRowDown);
                Trace.WriteLine(clueRowDown);
                clueRowDown = reader.ReadLine();
            }

            return new NYTCrosswordData()
            {
                rows = bRows,
                columns = bColumns,
                solution = bSolution,
                acrossClues = bAcrossClues,
                downClues = bDownClues,
                published = published
            };

        }

    }


}
