using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Wavelength.Core.Models;
using Wavelength.Server.WebAPI.Hubs;
using Wavelength.Server.WebAPI.Providers;
using Wavelength.Server.WebAPI.Repositories;

namespace Wavelength.Server.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private CouchbaseConfig _couchbaseConfig;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _couchbaseConfig = new CouchbaseConfig();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSignalR();
            services.AddControllers();
            //Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Wavelength Demo",
                        Version = "v1"
                    });
                c.CustomSchemaIds((type) => type.FullName);
                c.DescribeAllParametersInCamelCase();
            });
            services.Configure<CouchbaseConfig>(Configuration.GetSection(CouchbaseConfig.Section));

            _couchbaseConfig = new CouchbaseConfig();
            Configuration.GetSection(CouchbaseConfig.Section).Bind(_couchbaseConfig);

            if (_couchbaseConfig.Mode == CouchbaseConfig.ModeServer) 
	        {
                services.AddCouchbase(Configuration.GetSection(CouchbaseConfig.Section));
                services.AddCouchbaseBucket<IWavelengthBucketProvider>("wavelength");
                services.AddSingleton<IAuctionRepository, AuctionRepository>();
            }
            else 
	        { 
	        }

            //adding mediatr for cqrs support
            services.AddMediatR(typeof(Program));

            //https://timdows.com/projects/use-mediatr-with-fluentvalidation-in-the-asp-net-core-pipeline/
            //register validators with dependcy injection
            AssemblyScanner
                .FindValidatorsInAssembly(typeof(Startup).Assembly)
                .ForEach(result => services.AddTransient(result.InterfaceType, result.ValidatorType));


            //pipeline for validation
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
	        IApplicationBuilder app, 
	        IWebHostEnvironment env,
            IHostApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            //enable middleware to serve generated swagger as JSON enpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wavelength Demo");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<BiddingHub>("hubs/bidding");
                endpoints.MapControllers();
            });
        }
    }
}
