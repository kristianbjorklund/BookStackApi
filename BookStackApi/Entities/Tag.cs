﻿namespace BookStackApi {
  public class Tag :IBookStackEntity {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public int Order { get; set; }
  }
}