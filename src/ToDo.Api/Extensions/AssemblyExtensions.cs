using System.IO;
using System.Reflection;

namespace ToDo.Api.Utils
{
    internal static class AssemblyExtensions
    {
        public static string DocumentationFilePath(this Assembly assembly)
        {
            return Path.ChangeExtension(assembly.Location, "xml");
        }
    }
}