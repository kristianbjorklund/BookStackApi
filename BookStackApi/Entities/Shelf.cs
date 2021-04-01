using System;

namespace BookStackApi {
  [BookStackEntity("shelves")]
  public class Shelf : BookStackEntity {
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
    public int OwnedBy { get; set; }
    public int ImageId { get; set; }

  }
}