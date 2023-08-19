using HtmlAgilityPack;
using QuizDb.Module.Extensions;
using QuizDb.Module.Models;
using QuizDB.DAL.Entities;

namespace QuizDb.QuizDbModule
{
    public static class QuizDbParser
    {
        private const string TOURNAMENT_PATH = "//tr";
        private const string TOURNAMENT_NAME_PATH = "td/a[starts-with(@href, '/tour')]";
        private const string QUESTION_PATH = "//div[@class='question']";
        private const string PARAGRAPH_PATH = "//p";
        private const string IMAGE_PATH = "//img";
        private const string EXTRA_PATH = "//div[@class='razdatka']";
        private const string SIBLING_QUESTION_NODE_PATH = "./following-sibling::text()";

        private const string QUESTION_CLASS = "Question";
        private const string ANSWER_CLASS = "Answer";
        private const string COMMENTS_CLASS = "Comments";
        private const string SOURCES_CLASS = "Sources";
        private const string AUTHORS_CLASS = "Authors";

        public static List<TournamentModel> GetTournaments(string htmlContent)
        {
            var result = new List<TournamentModel>();

            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                var tournamentNodes = doc.DocumentNode.SelectNodes(TOURNAMENT_PATH);

                if (tournamentNodes is null)
                {
                    return result;
                }

                foreach (var tournamentNode in tournamentNodes)
                {
                    var linkNodes = tournamentNode.SelectNodes(TOURNAMENT_NAME_PATH);

                    if (linkNodes is null)
                    {
                        continue;
                    }

                    var linkNode = linkNodes[0];

                    var name = linkNode.InnerText;
                    var link = linkNode.GetAttributeValue("href", "");
                    var splittedLink = link.Split('/');
                    var datePlayed = linkNode.NextSibling.InnerText.Split(',')[0];

                    var dateAdded = Convert.ToDateTime(tournamentNode.SelectNodes("td")[^1].InnerText);

                    result.Add(new TournamentModel(name, splittedLink[^1], datePlayed, dateAdded));
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public static TournamentQuestionsModel GetTournamentQuestions(string htmlContent, int tournamentId)
        {
            var result = new TournamentQuestionsModel();

            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                var questionNodes = doc.DocumentNode.SelectNodes(QUESTION_PATH);

                if (questionNodes is null)
                { 
                    return result; 
                }

                foreach (var linkNode in questionNodes)
                {
                    var questionModel = ParseQuestion(linkNode.InnerHtml);
                    questionModel.TournamentId = tournamentId;

                    result.Questions.Add(questionModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        private static QuestionModel ParseQuestion(string questionContent)
        {
            var questionDoc = new HtmlDocument();
            questionDoc.LoadHtml(questionContent);

            var textNodes = questionDoc.DocumentNode.SelectNodes(PARAGRAPH_PATH);

            var questionModel = new QuestionModel()
            {
                Text = GetNodeTextByClass(textNodes, QUESTION_CLASS),
                Answer = GetNodeTextByClass(textNodes, ANSWER_CLASS),
                Comment = GetNodeTextByClass(textNodes, COMMENTS_CLASS),
                Sources = GetNodeTextByClass(textNodes, SOURCES_CLASS),
                Author = GetNodeTextByClass(textNodes, AUTHORS_CLASS),
            };

            ValidateQuestionText(questionModel, questionDoc);

            var imgNodes = questionDoc.DocumentNode.SelectNodes(IMAGE_PATH);

            if (imgNodes is null) 
            {
                return questionModel;
            }

            var imgNodesAttributes = imgNodes.Where(x => x.Attributes.Any()).SelectMany(x => x.Attributes);
            questionModel.Extra = imgNodesAttributes.FirstOrDefault(x => x.Name == "src" && x.Value.Contains("http"))?.Value ?? string.Empty;

            return questionModel;
        }

        private static void ValidateQuestionText(QuestionModel questionModel, HtmlDocument questionDoc)
        {
            questionModel.Text = questionModel.Text.RemoveParagraphTitle();

            if (!string.IsNullOrWhiteSpace(questionModel.Text)) 
            {
                return;
            }

            var extraNode = questionDoc.DocumentNode.SelectNodes(EXTRA_PATH).FirstOrDefault();

            if (extraNode is null) 
            {
                return;
            }

            var textNode = extraNode
                .SelectNodes(SIBLING_QUESTION_NODE_PATH)
                .Where(node => !string.IsNullOrWhiteSpace(node.InnerText));

            questionModel.Text = string.Join(" ", textNode?.Select(x => x.InnerText));

            var extraTextNode = extraNode
                .ChildNodes
                .FirstOrDefault(node => node.NodeType == HtmlNodeType.Text && !string.IsNullOrWhiteSpace(node.InnerHtml));

            if (extraTextNode is null) 
            {
                return;
            }

            questionModel.Extra = extraTextNode?.InnerText;
        }

        private static string GetNodeTextByClass(HtmlNodeCollection nodes, string className) => nodes.FirstOrDefault(x => x.InnerHtml.Contains(className))?.InnerText.Trim() ?? string.Empty;
    }
}
