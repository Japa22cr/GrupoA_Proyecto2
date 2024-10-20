using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EF
{
    public class ClaseDbContext: DbContext
    {
        public ClaseDbContext(DbContextOptions<ClaseDbContext> options) : base(options) 
        {
          
        }
    }
}
