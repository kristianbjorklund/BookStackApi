using System;
using System.Collections.Generic;

namespace BookStackApi {
  public class ListParameters {
    private int _count;
    private int _offset;

    public int Count {
      get => _count;
      set {
        if (value <= 0) throw new ArgumentOutOfRangeException(nameof(Count), "must be greater than zero");
        if (value > 500) throw new ArgumentOutOfRangeException(nameof(Count), "must be less than 500");
        _count = value;
      }
    }

    public int Offset {
      get => _offset;
      set {
        if (value < 0) throw new ArgumentOutOfRangeException(nameof(Count), "must be zero or greater");
        _offset = value;
      }
    }

    public SortDirection SortDirection { get; set; }
    public string SortField { get; set; }

    public List<Filter> Filters { get; }
    public ListParameters(int count = 100, int offset = 0) {
      Count = count;
      Offset = offset;
      SortDirection = SortDirection.Ascending;
      SortField = null;
      Filters = new List<Filter>();
    }


    public string AsQuery() {
      var p = $"?count={Count}&offset={Offset}";
      if (!string.IsNullOrWhiteSpace(SortField)) {
        p += $"&{translateSortDirection(SortDirection)}{SortField.ToSnakeCase()}";
      }

      foreach (var filter in Filters) {
        p += filter.AsQueryField();
      }

      return p;
    }

    private string translateSortDirection(SortDirection sortDirection) {
      switch (sortDirection) {
        case SortDirection.Ascending:
          return "+";
        case SortDirection.Descending:
          return "-";
        default:
          throw new ArgumentOutOfRangeException(nameof(sortDirection), sortDirection, null);
      }
    }


  }

  public class Filter {
    public FilterOperator Operator { get; set; }
    public string Field { get; set; }
    public string Value { get; set; }

    public string AsQueryField() {
      var p = $"&filter[{Field.ToSnakeCase()}:{translateOperator(Operator)}]={Value.UrlSafe()}";
      return p;
    }

    private string translateOperator(FilterOperator filterOperator) {
      switch (filterOperator) {
        case FilterOperator.Equals:
          return "eq";
        case FilterOperator.NotEquals:
          return "ne";
        case FilterOperator.GreaterThan:
          return "gt";
        case FilterOperator.LessThan:
          return "lt";
        case FilterOperator.GreaterThanOrEqual:
          return "gte";
        case FilterOperator.LessThanOrEqual:
          return "lte";
        case FilterOperator.Like:
          return "like";
        default:
          throw new ArgumentOutOfRangeException(nameof(filterOperator), filterOperator, null);
      }

    }
  }
  public enum SortDirection {
    Ascending, Descending
  }

  public enum FilterOperator {
    Equals, NotEquals, GreaterThan, LessThan, GreaterThanOrEqual, LessThanOrEqual, Like
  }
}
