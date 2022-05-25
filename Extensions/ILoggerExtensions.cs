using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace AZFunction.Log.Payload.Extensions
{
    public static class ILoggerExtensions
    {
        public static IDisposable BeginNamedScope(this ILogger logger,
            string name, params ValueTuple<string, object>[] properties)
        {
            if (null == logger) throw new ArgumentNullException(nameof(logger));

            var dictionary = properties.ToDictionary(p => p.Item1, p => p.Item2);
            dictionary[name + ".Scope"] = Guid.NewGuid();
            return logger.BeginScope(dictionary);

        }
    }
}
