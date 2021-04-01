# BookStackApi
A C# bookstack API

``` csharp
string tokenId; // Create Id and Secret from your profile section in BookStack
string tokenSecret;

var api = new ApiService("https://my-bookstack.net/api/", tokenId, tokenSecret);

var bookResponse = api.GetList<Book>();
Console.WriteLine($"Books {bookResponse.Total}");

foreach (var book in bookResponse.Data) {
    Console.WriteLine($"{book.Id} {book.Slug}");
}
```

## Todo

The query options are still missing from the GetList and GetListAsync.

Any contributions are welcome
