using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace MiniTwit.Utils.CustomJson
{
    public class CustomJsonResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            var attr = member.GetCustomAttribute(typeof(JsonPropName));
            if (attr == null)
            {
                return prop;
            }
            var jsonName = ((JsonPropName) attr).PropertyName;
            prop.PropertyName = jsonName;
            return prop;
        }
    }
}