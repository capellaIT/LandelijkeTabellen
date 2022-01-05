using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Capella.LandelijkeTabellen.Landentabel.Data
{
    /// <summary>
    /// Helper class to read embedded files from an assembly.
    /// </summary>
    internal static class EmbeddedResources
    {
        private static readonly object _sync = new object();

        /// <summary>
        /// Method reads an embedded file within a given assembly by searching the correct pathname
        /// <example>
        /// "MyData.Reports.ManagerInfo.xml" should be located at [project root]/MyData/Reports/ManagerInfo.xml
        /// </example>
        /// </summary>
        /// <param name="embeddedFileName">Name of the embedded file.</param>
        /// <param name="containerAssembly">The container assembly.</param>
        /// <returns>Content of the file as a string.</returns>
        /// <exception>Occurs if the resource file cannot be found.
        ///     <cref>System.ArgumentException</cref>
        /// </exception>
        public static string ReadFileAsString(string embeddedFileName, Assembly containerAssembly = null)
        {
            Stream stream = ReadStream(embeddedFileName, containerAssembly);

            if (stream != null)
            {
                var reader = new StreamReader(stream);
                string text = reader.ReadToEnd();

                return text;
            }

            throw new ArgumentException(string.Format(
                CultureInfo.InvariantCulture,
                "Resource file '{0}' could not be found. Are you sure the file is embedded?",
                embeddedFileName));
        }

        private static Stream ReadStream(string embeddedFileName, Assembly containerAssembly = null)
        {
            // load the xml
            lock (_sync)
            {
                Assembly asm = containerAssembly ?? Assembly.GetAssembly(typeof(EmbeddedResources));
                string assemblyNamespace = asm.FullName.IndexOf(",", StringComparison.Ordinal) != -1
                                            ? asm.FullName.Substring(0, asm.FullName.IndexOf(",", StringComparison.Ordinal))
                                            : asm.FullName;

                string embeddedResourcePath = embeddedFileName
                    .Replace(@"\", ".")
                    .Replace(@"/", ".")
                    .Replace(@"~", string.Empty);

                if (embeddedResourcePath.StartsWith(".", StringComparison.Ordinal))
                {
                    embeddedResourcePath = embeddedResourcePath.Substring(1, embeddedResourcePath.Length - 1);
                }

                Stream stream = asm.GetManifestResourceStream(string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}.{1}",
                    assemblyNamespace,
                    embeddedResourcePath));

                return stream;
            }
        }
    }
}
