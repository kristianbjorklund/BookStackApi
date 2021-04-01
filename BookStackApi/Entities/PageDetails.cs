using System;

namespace BookStackApi {
  [BookStackEntity("pages")]
  public class PageDetails : BookStackEntity {
    public int BookId { get; set; }
    public int ChapterId { get; set; }
    public string Html { get; set; }
    public string Markdown { get; set; }
    public int Priority { get; set; }
    public bool Draft { get; set; }
    public bool Template { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User CreatedBy { get; set; }
    public User UpdatedBy { get; set; }
    public User OwnedBy { get; set; }
    public Tag[] Tags { get; set; }
    public int RevisionCount { get; set; }
  }
}