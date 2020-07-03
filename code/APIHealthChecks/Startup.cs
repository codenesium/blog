using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Serilog;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace APIHealthChecks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddOpenApiDocument();

            string connectionString = this.Configuration.GetConnectionString("Default");

            services.AddHealthChecks().AddCheck("sql", () =>
            {
                string sqlHealthCheckDescription = "Tests that we can connect and select from the database.";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("SELECT TOP(1) id from dbo.Pets", connection);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Exception in sql health check");
                        return HealthCheckResult.Unhealthy(sqlHealthCheckDescription);
                    }
                }

                return HealthCheckResult.Healthy(sqlHealthCheckDescription);
            });
        }
    

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseOpenApi();

            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health")
            });
        }
    }
}
