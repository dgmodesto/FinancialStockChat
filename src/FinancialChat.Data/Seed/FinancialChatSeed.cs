using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Data.Seed
{
    public static  class FinancialChatSeed
    {
        public static void InitializeDataBase(IApplicationBuilder app)
        {
            using var db = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            db.Database.EnsureCreated();
        }
    }
}
