using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalapulgaMerge.Data
{
    public class KalapulkContext : DbContext
    {
        public KalapulkContext(DbContextOptions<KalapulkContext> options) : base(options) { }
    }
}
