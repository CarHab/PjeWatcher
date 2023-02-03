using HtmlAgilityPack;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Watcher;
public class Retriever
{
    public async Task<string> GetText(string url)
    {
        string html = await CallUrl(url);

        string output = ParseHtml(html);

        return output;
    }

    public async Task<string> CallUrl(string url)
    {
        try
        {
            HttpClient client = new();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = await client.GetStringAsync(url);
            return response;
        }
        catch (Exception e)
        {
            return e.Message;
        }
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
