using System;

namespace BookStackApi {
  public class Cover : IBookStackEntity {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }

    public string Path { get; set; }
    public string Type { get; set; }

    public int UploadedTo { get; set; }

  }
}