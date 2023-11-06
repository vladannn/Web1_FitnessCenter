using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserType
    {
        Owner,
        Coach,
        Visitor
    }
}