using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BookStackApi;

namespace TestApi {
  class Program {
    static void Main(string[] args) {
      var settings = ReadSecrets();

      var tokenId = settings["id"];
      var tokenSecret = settings["secret"];
      var url = settings["url"];

      var program = new Program(url, tokenId, tokenSecret);
      //program.TestBookCreate();
      //program.TestPageCreate();
      program.TestParameter();


      Console.WriteLine("Done");
      Console.ReadLine();
    }

    private readonly ApiService _api;
    private readonly BookStackResponse<Book> _bookResponse;
    private readonly BookStackResponse<Chapter> _chapterResponse;
    public Program(string url, string tokenId, string tokenSecret) {
      _api = new ApiService(url, tokenId, tokenSecret);

      _bookResponse = _api.GetList<Book>();
      Console.WriteLine($"Books {_bookResponse.Total}");

      _chapterResponse = _api.GetList<Chapter>();
      Console.WriteLine($"Chapters {_chapterResponse.Total}");
      foreach (var book in _bookResponse.Data) {
        Console.WriteLine($"{book.Id} {book.Slug}");
      }

      {
        var book = _api.GetDetails<BookDetails>(1);
        Console.WriteLine($"{book.Name} : {book.CreatedAt}");
      }


    }

    private void TestParameter() {
      var importedId = new List<int>();
      var pageSize = 100;
      var pageNo = 0;
      do {
        var parameter = new ListParameters(pageSize, pageSize * pageNo);
        var pageResponse = _api.GetList<Page>(parameter);
        Console.WriteLine($"Page# {pageNo} - Size={pageSize} - Response={pageResponse.Total}");
        if (pageResponse.Data.Length == 0) break;

        foreach (var page in pageResponse.Data) {
          Thread.Sleep(500);
          var pageDetail = _api.GetDetails<PageDetails>(page.Id);
          var tag = pageDetail.Tags.SingleOrDefault(p => p.Name == "ImportId");
          if (tag == null) {
            Console.WriteLine($"No import tag on page: {page.Id} {page.Name}");
            continue;
          }

          var techId = int.Parse(tag.Value);
          importedId.Add(techId);
        }

        pageNo++;
      } while (true);




      Console.WriteLine($"Found pages: {importedId.Count}");
    }

    private void TestBookCreate() {
      Console.WriteLine($"Highest book id {_bookResponse.Data.OrderByDescending(book => book.Id).First().Id}");
      int id;

      {
        var result = _api.Post(new Book { Name = "API-book", Description = "Created by API Post call" });
        Console.WriteLine($"Create result {result.Id}");
        id = result.Id;
      }
      {
        var result = _api.Put(new Book { Id = id, Name = "API-book Original" });
        Console.WriteLine($"Update result {result.Name}");
      }

      {
        var result = _api.Delete<Book>(id);
        Console.WriteLine($"Delete result {result}");
      }



    }

    private void TestPageCreate() {
      var tags = new List<Tag>();
      tags.Add(new Tag { Name = "ImportId", Value = $"{12}" });
      tags.Add(new Tag { Name = "Relation", Value = $"{10}|{"Datalogik P/S"}" });
      tags.Add(new Tag { Name = "Original", Value = $"{20}|{"Kristian Bjørklund"}|{"kb@datalogik.dk"}" });
      tags.Add(new Tag { Name = "CreateDate", Value = $"{DateTime.Today.AddDays(-268):d}" });
      tags.Add(new Tag { Name = "ImportDate", Value = $"{DateTime.Today:d}" });
      var page = new PageDetails
      {
        Name = "Demo artikel",
        Html = "<html><h1>Mega nice</h1><p>Slet mig bare</p></html>",
        Tags = tags.ToArray()
      };

      page.ChapterId = _chapterResponse.Data.First().Id;

      var insertedPage = _api.Post(page);
      if (insertedPage != null)
        Console.WriteLine($"Page: {insertedPage.Id}:{insertedPage.Name}");
      else {
        Console.WriteLine("Error occurred during insert");
      }
    }

    private static Dictionary<string, string> ReadSecrets() {
      var settings = new Dictionary<string, string>();
      var lines = System.IO.File.ReadAllLines("c:\\temp\\apitest\\bookstack.txt");
      foreach (var line in lines) {
        var s = line.Split('=');
        settings.Add(s[0], s[1]);
      }

      return settings;
    }
  }
}
