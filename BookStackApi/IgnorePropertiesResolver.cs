using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BookStackApi {
  public class IgnorePropertiesResolver : DefaultContractResolver {
    private readonly HashSet<string> ignoreProps;
    public IgnorePropertiesResolver(IEnumerable<string> propNamesToIgnore) {
      ignoreProps = new HashSet<string>(propNamesToIgnore);
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
      var hasIgnore = member.GetCustomAttributes<BookStackNoUpdateAttribute>().Any();

      JsonProperty property = base.CreateProperty(member, memberSerialization);
      if (ignoreProps.Contains(property.PropertyName)) {
        property.ShouldSerialize = _ => false;
      } else if (hasIgnore) {
        property.ShouldSerialize = _ => false;
      }
      return property;
    }
  }
}