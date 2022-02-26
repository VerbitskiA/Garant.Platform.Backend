using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Core.Data
{
    public static class DbContextFactory
    {
        private static Dictionary<string, string> ConnectionStrings { get; set; }

        public static void SetConnectionString(Dictionary<string, string> connStrs)
        {
            ConnectionStrings = connStrs;
        }

        public static PostgreDbContext CreateDbContext(string connid)
        {
            if (!string.IsNullOrEmpty(connid))
            {
                var connStr = ConnectionStrings[connid];
                var optionsBuilder = new DbContextOptionsBuilder<PostgreDbContext>();
                optionsBuilder.UseNpgsql(connStr);
                
                return new PostgreDbContext(optionsBuilder.Options);
            }
            
            throw new ArgumentNullException("ConnectionId");
        }
        
        public static IdentityDbContext CreateIdentityDbContext(string connid)
        {
            if (!string.IsNullOrEmpty(connid))
            {
                var connStr = ConnectionStrings[connid];
                var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
                optionsBuilder.UseNpgsql(connStr);
                
                return new IdentityDbContext(optionsBuilder.Options);
            }
            
            throw new ArgumentNullException("ConnectionId");
        }
    }
}