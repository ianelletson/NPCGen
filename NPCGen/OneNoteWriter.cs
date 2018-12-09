using System.Linq;
using System.Xml.Linq;
using Microsoft.Office.Interop.OneNote;

namespace NPCGen
{
    // https://stackoverflow.com/questions/27294510/how-to-write-to-a-onenote-2013-page-using-c-sharp-and-the-onenote-interop
    public class OneNoteWriter
    {
        static Application _app = new Application();
        public static string Notebook;
        public static string Section;

        static Application oneNoteApp = new Application();
        private static XNamespace ns = null;

        public static void Execute(Npc npc)
        {
            if (string.IsNullOrWhiteSpace(Notebook))
                Notebook = "NPCGen";
            if (string.IsNullOrWhiteSpace(Section))
                Section = "Generated";

            GetNamespace();
            var notebookId = GetObjectId(null, HierarchyScope.hsNotebooks, Notebook);
            var sectionId = GetObjectId(notebookId, HierarchyScope.hsSections, Section);
            var npid = CreatePage(sectionId, npc);
        }

        static void GetNamespace()
        {
            oneNoteApp.GetHierarchy(null, HierarchyScope.hsNotebooks, out var xml);
            var doc = XDocument.Parse(xml);
            if (doc.Root != null) ns = doc.Root.Name.Namespace;
        }

        static string GetObjectId(string parentId, HierarchyScope scope, string objectName)
        {
            oneNoteApp.GetHierarchy(parentId, scope, out var xml);
            var doc = XDocument.Parse(xml);
            string nodeName;

            switch (scope)
            {
                case (HierarchyScope.hsNotebooks):
                    nodeName = "Notebook";
                    break;
                case (HierarchyScope.hsSections):
                    nodeName = "Section";
                    break;
                case (HierarchyScope.hsPages):
                    nodeName = "Page";
                    break;
                default:
                    return null;
            }

            var node = doc.Descendants(ns + nodeName)
                          .FirstOrDefault(n => n.Attribute("name")?.Value == objectName);
            return node?.Attribute("ID")?.Value;
        }

        static string CreatePage(string sectionId, Npc npc)
        {
            oneNoteApp.GetHierarchy(sectionId, HierarchyScope.hsPages, out var xml);
            var doc = XDocument.Parse(xml);

            oneNoteApp.CreateNewPage(sectionId, out var newPageId);
            var newPage = MakePage(newPageId, npc.Name);

            newPage.Add(MakeHeadingWithBody("Basics:", $"{npc.GetBasicData()}"));
            newPage.Add(MakeHeadingWithBody("Knows:", string.Empty));
            foreach (var e in npc.GetAllDataFormatted())
            {
                newPage.Add(MakeHeadingWithBody(e.Key, e.Value));
            }

            oneNoteApp.UpdatePageContent(newPage.ToString());
            return newPageId;
        }

        static XElement MakePage(string newPageId, string title)
        {
            var newPage = new XElement(ns + "Page");
            newPage.SetAttributeValue("ID", newPageId);
            newPage.Add(new XElement(ns + "Title",
                    new XElement(ns + "OE", new XElement(ns + "T", new XCData(title)))));

            return newPage;
        }

        static XElement MakeHeadingWithBody(string heading, string body)
        {
            var xe = new XElement(ns + "Outline",
                    new XElement(ns + "OEChildren",
                            new XElement(ns + "OE", new XElement(ns + "T", new XCData(heading)),
                                    new XElement(ns + "OEChildren",
                                            new XElement(ns + "OE",
                                                    new XElement(ns + "T", new XCData(body)))))));

            return xe;
        }
    }
}