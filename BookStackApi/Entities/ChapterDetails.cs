using System;

namespace BookStackApi {
  [BookStackEntity("chapters")]
  public class ChapterDetails : BookStackEntity {
    public int BookId { get; set; }
    public string Description { get; set; }
    [BookStackNoUpdate]
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User CreatedBy { get; set; }
    public User UpdatedBy { get; set; }
    public User OwnedBy { get; set; }
    public Tag[] Tags { get; set; }
    [BookStackNoUpdate]
    public Page[] Pages { get; set; }
  }
}