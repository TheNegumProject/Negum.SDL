using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Negum.Core.Engines;
using Negum.Game.Client;

namespace Negum.SDL
{
    /// <summary>
    /// Builder used for building window title.
    /// </summary>
    /// 
    /// <author>
    /// https://github.com/TheNegumProject/Negum.SDL
    /// </author>
    public static class NegumSdlTitleBuilder
    {
        /// <summary>
        /// </summary>
        /// <returns>Title for the current Negum SDL Client.</returns>
        public static string Build()
        {
            var builder = new StringBuilder();

            BuildAssemblyVersion<NegumSdlClient>(builder);
            builder.Append(' ');

            BuildAssemblyVersion<IEngineProvider>(builder);
            builder.Append(' ');

            BuildAssemblyVersion<INegumClient>(builder);

            return builder.ToString();
        }

        private static void BuildAssemblyVersion<TType>(StringBuilder builder)
        {
            var assembly = typeof(TType).Assembly;

            builder.Append('[');

            var assemblyName = GetAssemblyAttributeValue<AssemblyTitleAttribute>(assembly);
            builder.Append(assemblyName).Append(" v");

            var version = GetAssemblyAttributeValue<AssemblyInformationalVersionAttribute>(assembly);
            builder.Append(version).Append(']');
        }

        private static string GetAssemblyAttributeValue<TType>(Assembly assembly)
        {
            return assembly?
                       .GetCustomAttributesData()
                       .FirstOrDefault(ad => ad.AttributeType.Equals(typeof(TType)))?
                       .ConstructorArguments[0].Value.ToString()
                   ?? throw new ArgumentException($"Error when getting attribute value: [Assembly: {assembly.FullName}; Attribute: {typeof(TType)}]");
        }
    }
}