using EnvDTE;

namespace Atc.AutoFormatter.Services
{
    public interface IDocumentFormatter
    {
        void Format(Document document);
    }
}