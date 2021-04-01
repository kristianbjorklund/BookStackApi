using System;

namespace BookStackApi {
 [BookStackEntity("chapters")]
  public class Chapter : BookStackEntity {
    public int BookId { get; set; }
    public string Description { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
    public int OwnedBy { get; set; }

  }
}
