namespace BookStackApi {
  public class BookStackResponse<T> where T : class, IBookStackEntity, new() {
    public T[] Data { get; set; }
    public int Total { get; set; }
  }
}

