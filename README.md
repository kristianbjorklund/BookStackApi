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

Error handling (e.g. 427 - tomm many request) is wanted.

Any contributions are welcome
