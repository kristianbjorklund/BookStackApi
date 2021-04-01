using System;
using System.Reflection;

namespace BookStackApi {
  [AttributeUsage(AttributeTargets.Class)]
  public class BookStackEntityAttribute : Attribute {
    public string Path { get; }

    public BookStackEntityAttribute(string path) {
      Path = path.Trim('/');
    }

    public static BookStackEntityAttribute GetAttribute(Type type) {
      var attribute = type.GetCustomAttribute<BookStackEntityAttribute>();
      return attribute;
    }
  }
}