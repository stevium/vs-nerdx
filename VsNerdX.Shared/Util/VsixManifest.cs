using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace VsNerdX.Util
{
    public class VsixManifest
    {
        public string Id { get; set; }

        public string Version { get; set; }

        public VsixManifest() { }

        public VsixManifest(string manifestPath)
        {
            var doc = new XmlDocument();
            doc.Load(manifestPath);

            if (doc.DocumentElement == null || doc.DocumentElement.Name != "PackageManifest") return;

            var metaData = doc.DocumentElement.ChildNodes.Cast<XmlElement>().First(x => x.Name == "Metadata");
            var identity = metaData.ChildNodes.Cast<XmlElement>().First(x => x.Name == "Identity");

            Id = identity.GetAttribute("Id");
            Version = identity.GetAttribute("Version");
        }

        public static VsixManifest GetManifest()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyUri = new UriBuilder(assembly.CodeBase);
            var assemblyPath = Uri.UnescapeDataString(assemblyUri.Path);
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
            var vsixManifestPath = Path.Combine(assemblyDirectory, "extension.vsixmanifest");

            return new VsixManifest(vsixManifestPath);
        }
    }
}
