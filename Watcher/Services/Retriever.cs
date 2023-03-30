using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;

namespace Watcher.Services;
public class Retriever
{
    private readonly string _baseUrl = "https://pje1g.trf1.jus.br/consultapublica/ConsultaPublica/listView.seam";
    private string? _url = null;
    private readonly string _code;

    public Retriever(string code)
    {
        _code = code;
    }

    public void GetUrl(string code)
    {
        if (!Settings.IsLinkIsStale())
        {
            _url = Settings.GetLastLink();
            return;
        }

        try
        {
            EdgeOptions options = new();
            options.AddArgument("--headless");
            options.AddArgument("--log-level=3");

            EdgeDriver driver = new($"{Directory.GetCurrentDirectory()}\\msedgedriver.exe", options);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(2000);

            driver.Navigate().GoToUrl(_baseUrl);

            var textBox = driver.FindElement(By.Id("fPP:numProcesso-inputNumeroProcessoDecoration:numProcesso-inputNumeroProcesso"));
            textBox.SendKeys(code);

            var submitButton = driver.FindElement(By.Id("fPP:searchProcessos"));
            submitButton.Click();

            var linkElement = driver.FindElement(By.XPath("/html/body/div[6]/div/div/div/div[2]/form/div[2]/div/table/tbody/tr/td[2]/a"));

            var linkNumber = linkElement.GetAttribute("onclick").Split("=")[1].Split("'")[0];

            _url = $"https://pje1g.trf1.jus.br/consultapublica/ConsultaPublica/DetalheProcessoConsultaPublica/listView.seam?ca={linkNumber}";

            Settings.SetLastLink(_url);

            driver.Quit();
        }
        catch (Exception e)
        {
            throw new Exception("Um erro ocorreu. Tentando novamente...", e);
        }
    }

    public async Task<string> GetText()
    {
        try
        {
            if (_url is null)
            {
                GetUrl(_code);
            }

            string html = await CallUrl();

            string output = ParseHtml(html);

            return output;
        }
        catch (Exception e)
        {
            throw new Exception("Um erro ocorreu. Tentando novamente...", e);
        }
    }

    public async Task<string> CallUrl()
    {
        try
        {
            HttpClient client = new();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = await client.GetStringAsync(_url);
            return response;
        }
        catch (Exception e)
        {
            throw new Exception("Um erro ocorreu. Tentando novamente...", e);
        }
    }

    public string ParseHtml(string html)
    {
        try
        {
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(html);

            HtmlNode htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"j_id134:processoEvento:0:j_id498\"]");

            string decodedText = WebUtility.HtmlDecode(htmlBody.InnerText);
            return decodedText;
        }
        catch (Exception e)
        {
            throw new Exception("Um erro ocorreu. Tentando novamente...", e);
        }
    }
}
