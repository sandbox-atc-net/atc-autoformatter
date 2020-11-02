#nullable enable
using EnvDTE;

namespace Atc.AutoFormatter.Services
{
    public interface IDocumentLocator
    {
        Document? FindDocument(uint docCookie);
    }
}