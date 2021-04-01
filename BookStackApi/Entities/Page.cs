using System;

namespace BookStackApi {
  [BookStackEntity("pages")]
  public class Page : BookStackEntity {
    public int BookId { get; set; }
    public int ChapterId { get; set; }
    public int Priority { get; set; }
    public bool Draft { get; set; }
    public bool Template { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
    public int OwnedBy { get; set; }

  }
}