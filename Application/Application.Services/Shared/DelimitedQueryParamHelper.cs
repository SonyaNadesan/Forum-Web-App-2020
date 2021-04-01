using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Shared
{
    public class DelimitedQueryParamHelper
    {
        public static T GenerateCollection<T>(string paramValue, string noFilterIndication = "all", char delimiter = ' ') where T : ICollection<string>, new()
        {
            T collection = new T();

            var values = paramValue.ToLower().Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            if (!values.Contains(noFilterIndication.ToLower()))
            {
                foreach (var value in values)
                {
                    collection.Add(value);
                }
            }

            return (T)collection;
        }
    }
}
