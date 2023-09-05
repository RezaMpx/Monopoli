using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Monopoli
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<MonoDB>(m =>
            {
                m.UseSqlite("Data Source=Monopoli.db;");
                //"Data Source=C:\\Inetpub\\vhosts\\reza.farabiran.ir\\wwwroot\\wwwroot\\Reza.db;"; 
                //m.UseSqlServer("Data Source=DESKTOP;Initial Catalog=RezaDb;Integrated Security=True");
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Turns.RoomList = new(){ };
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseEndpoints(e =>
            {
                e.MapDefaultControllerRoute();
            });
        }
    }
}
