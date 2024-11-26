using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IBLab.Models;

namespace IBLab.Data
{
    public class IBLabContext : DbContext
    {
        public IBLabContext (DbContextOptions<IBLabContext> options)
            : base(options)
        {
        }

        public DbSet<IBLab.Models.User> Users { get; set; } = default!;
        public DbSet<IBLab.Models.TempUser> TempUsers { get; set; } = default!;
    }
}
