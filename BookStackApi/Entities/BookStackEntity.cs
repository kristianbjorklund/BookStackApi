namespace BookStackApi {
  public class BookStackEntity : IBookStackEntity {
    public int Id { get; set; }
    public string Name { get; set; }
    [BookStackNoUpdate]
    public string Slug { get; set; }
  }
}