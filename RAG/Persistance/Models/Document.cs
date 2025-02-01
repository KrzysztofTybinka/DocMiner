using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsProcessed { get; set; }
        public string StorageLocation { get; set; }
    }
}
