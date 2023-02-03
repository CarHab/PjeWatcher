using HtmlAgilityPack;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Watcher;
public class Retriever
{
    public string GetText(string url)
    {
        Task<string> html = CallUrl(url);

        string output = ParseHtml(html.Result);

        return output;
    }
    public async Task<string> CallUrl(string url)
    {
        HttpClient client = new();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
        client.DefaultRequestHeaders.Accept.Clear();
        var response = client.GetStringAsync(url);
        Thread.Sleep(1000);
        return await response;
    }

    public string ParseHtml(string html)
    {
        HtmlDocument htmlDoc = new();
        htmlDoc.LoadHtml(html);

        HtmlNode htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"j_id134:processoEvento:0:j_id498\"]");

        string decodedText = WebUtility.HtmlDecode(htmlBody.InnerText);
        return decodedText;
    }
}
