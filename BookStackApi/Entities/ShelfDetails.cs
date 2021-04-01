using System;

namespace BookStackApi {
  [BookStackEntity("shelves")]
  public class ShelfDetails : BookStackEntity {
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User CreatedBy { get; set; }
    public User UpdatedBy { get; set; }
    public User OwnedBy { get; set; }
    [BookStackNoUpdate]
    public Cover Cover { get; set; }
    public Tag[] Tags { get; set; }
    [BookStackNoUpdate]
    public BookStackEntity[] Books { get; set; }
  }
}