using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concierge.Characters.Collections
{
    public class Chapter
    {
        public Chapter()
        {
            Documents = new List<Document>();
            ID = Guid.NewGuid();
        }

        public Chapter(Guid id)
        {
            Documents = new List<Document>();
            ID = id;
        }

        public Document GetDocumentById(Guid id)
        {
            return Documents.Where(x => x.ID.Equals(id)).Single();
        }

        public List<Document> Documents { get; set; }
        public string Name { get; set; }
        public Guid ID { get; private set; }
    }
}
