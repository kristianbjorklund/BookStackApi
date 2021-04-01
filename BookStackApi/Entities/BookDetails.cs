using System;

namespace BookStackApi {
  [BookStackEntity("books")]
  public class BookDetails : BookStackEntity {
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User CreatedBy { get; set; }
    public User UpdatedBy { get; set; }
    public User OwnedBy { get; set; }
    public Cover Cover { get; set; }

    public Tag[] Tags { get; set; }

  }
}