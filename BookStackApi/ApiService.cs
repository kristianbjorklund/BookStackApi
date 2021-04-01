using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BookStackApi {
  public class ApiService {
    private readonly string _tokenId;
    private readonly string _tokenSecret;
    private readonly JsonSerializerSettings _settings;
    private readonly JsonSerializerSettings _settingsWithoutId;
    public string BaseUrl { get; }
    public string LastReason { get; private set; }
    /// <summary>
    /// Create the Api service
    /// </summary>
    /// <param name="baseUrl">Base Url for API - e.g. https://bookstack.test/api  </param>
    /// <param name="tokenId"></param>
    /// <param name="tokenSecret"></param>
    public ApiService(string baseUrl, string tokenId, string tokenSecret) {
      _tokenId = tokenId;
      _tokenSecret = tokenSecret;
      BaseUrl = baseUrl;
      if (!BaseUrl.EndsWith("/")) BaseUrl += "/";

      var contractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };
      var contractResolverNoId = new IgnorePropertiesResolver(new[] { "id", "created_at", "created_by", "updated_at", "updated_by", "owned_by", "image_id"}) { NamingStrategy = new SnakeCaseNamingStrategy() };
      _settings = new JsonSerializerSettings
      {
        ContractResolver = contractResolver,
        Formatting = Formatting.Indented, 
        NullValueHandling = NullValueHandling.Ignore
      };
      _settingsWithoutId = new JsonSerializerSettings
      {
        ContractResolver = contractResolverNoId,
        NullValueHandling = NullValueHandling.Ignore
      };

    }

    /// <summary>
    /// Get list of entities (async)
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <returns>Returns an object containing a list and the total count</returns>
    public async Task<BookStackResponse<T>> GetListAsync<T>() where T : class, IBookStackEntity, new()
    {
      LastReason = null;
      using var client = new HttpClient();
      var url = getUrlForEntity(typeof(T));
      client.DefaultRequestHeaders.Add("Authorization", $"Token {_tokenId}:{_tokenSecret}");
      var response = await client.GetStringAsync(url);

      var result = JsonConvert.DeserializeObject<BookStackResponse<T>>(response, _settings);
      return result;
    }

    public BookStackResponse<T> GetList<T>() where T : class, IBookStackEntity, new() => GetListAsync<T>().GetAwaiter().GetResult();


    /// <summary>
    /// Get the details of a single entity
    /// </summary>
    /// <typeparam name="T">Entity type (use the *Details version</typeparam>
    /// <param name="id">Id of entity to get</param>
    /// <returns>Returns single entity in *Details Version</returns>
    public async Task<T> GetDetailsAsync<T>(int id) where T : class, IBookStackEntity, new()
    {
      LastReason = null;
      using var client = new HttpClient();
      var url = getUrlForEntity(typeof(T), id);
      client.DefaultRequestHeaders.Add("Authorization", $"Token {_tokenId}:{_tokenSecret}");
      var response = await client.GetStringAsync(url);
      var result = JsonConvert.DeserializeObject<T>(response, _settings);
      return result;
    }

    public T GetDetails<T>(int id) where T : class, IBookStackEntity, new() => GetDetailsAsync<T>(id).GetAwaiter().GetResult();

    /// <summary>
    /// Delete an entity
    /// </summary>
    /// <typeparam name="T">Type of entity to delete</typeparam>
    /// <param name="id">Id of entity to delete</param>
    /// <returns>Returns true if objects was found and deleted</returns>
    public async Task<bool> DeleteAsync<T>(int id) where T : class, IBookStackEntity, new()
    {
      using var client = new HttpClient();
      var url = getUrlForEntity(typeof(T), id);
      client.DefaultRequestHeaders.Add("Authorization", $"Token {_tokenId}:{_tokenSecret}");
      var response = await client.DeleteAsync(url);
      LastReason = response.ReasonPhrase;
      return response.IsSuccessStatusCode;
    }



    public bool Delete<T>(int id) where T : class, IBookStackEntity, new() => DeleteAsync<T>(id).GetAwaiter().GetResult();

    /// <summary>
    /// Updates an entity
    /// </summary>
    /// <typeparam name="T">Type of entity to update</typeparam>
    /// <param name="entity">Entity with updates - The ID must be set, some attributes that cannot be set are ignored - null values are ignored</param>
    /// <returns>Returns the updated entity</returns>
    public async Task<T> PutAsync<T>(T entity) where T : class, IBookStackEntity, new()
    {
      using var client = new HttpClient();
      var url = getUrlForEntity(typeof(T), entity.Id);
      client.DefaultRequestHeaders.Add("Authorization", $"Token {_tokenId}:{_tokenSecret}");
      client.DefaultRequestHeaders.Add("Accept", "application/json");

      var content = JsonConvert.SerializeObject(entity, _settingsWithoutId);
      HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
      var response = await client.PutAsync(url, httpContent);
      var success = response.IsSuccessStatusCode;
      LastReason = response.ReasonPhrase;
      if (!success) return null;

      var stringResult = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<T>(stringResult, _settings);
    }

    public T Put<T>(T entity) where T : class, IBookStackEntity, new() => PutAsync(entity).GetAwaiter().GetResult();

    /// <summary>
    /// Creates an entity
    /// </summary>
    /// <typeparam name="T">Type of entity to create</typeparam>
    /// <param name="entity">Entity with information - The ID must be set, some attributes that cannot be set are ignored</param>
    /// <returns>Returns the created entity</returns>
    public async Task<T> PostAsync<T>(T entity) where T : class, IBookStackEntity, new()
    {
      using var client = new HttpClient();
      var url = getUrlForEntity(typeof(T));
      client.DefaultRequestHeaders.Add("Authorization", $"Token {_tokenId}:{_tokenSecret}");
      client.DefaultRequestHeaders.Add("Accept", "application/json");

      var content = JsonConvert.SerializeObject(entity, _settingsWithoutId);
      HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
      var response = await client.PostAsync(url, httpContent);
      var success = response.IsSuccessStatusCode;
      LastReason = response.ReasonPhrase;
      if (!success) return null;
      var stringResult = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<T>(stringResult, _settings);
    }

    public T Post<T>(T entity) where T : class, IBookStackEntity, new() => PostAsync(entity).GetAwaiter().GetResult();

    /// <summary>
    /// Helper to get the url for an entity type
    /// </summary>
    /// <param name="type">Entity type</param>
    /// <param name="id">Id, if relevant for the url</param>
    /// <returns>Returns the entire url incl. id if available</returns>
    private string getUrlForEntity(Type type, int? id=null) {
      var url =  $"{BaseUrl}{BookStackEntityAttribute.GetAttribute(type).Path}";
      if (id == null) return url;
      return $"{url}/{id.Value}";
    }
  }
}