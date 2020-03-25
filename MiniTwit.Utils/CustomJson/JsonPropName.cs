using System;

namespace MiniTwit.Utils.CustomJson
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonPropName : Attribute
    {
        public string PropertyName { get; protected set; }

        public JsonPropName(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}