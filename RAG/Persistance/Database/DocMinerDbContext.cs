using Microsoft.EntityFrameworkCore;
using Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Database
{
    public class DocMinerDbContext : DbContext
    {
        public DocMinerDbContext(DbContextOptions<DocMinerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }

    }
}
