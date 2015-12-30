using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DpControl.Domain.EFContext
{
    public class DbInitialization
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ShadingContext>();
            if (context.Database == null)
            {
                throw new Exception("DB is null");
            }

            if (context.Alarms.Any())
            {
                return;   // DB has been seeded 
            }

            // Seed data here
        }
    }
}
