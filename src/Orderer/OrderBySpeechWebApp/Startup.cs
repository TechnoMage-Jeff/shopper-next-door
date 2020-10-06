using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderBySpeechWebApp.Proxies;

namespace OrderBySpeechWebApp
{
    public class Startup
    {
        private string _speechServiceKey;
        private string _speechServiceEndpoint;
        private string _textAnalyticsKey;
        private string _textAnalyticsEndpoint;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // configure proxies from application secrets
            _speechServiceKey = Configuration["SpeechServiceKey"];
            _speechServiceEndpoint = Configuration["SpeechServiceEndPoint"];
            _textAnalyticsKey = Configuration["TextAnalyticsServiceKey"];
            _textAnalyticsEndpoint = Configuration["TextAnalyticsServiceEndpoint"];

            InitializeProxies();

            services.AddControllersWithViews();
        }

        private void InitializeProxies()
        {
            SpeechSynthesisProxy.Initialize(_speechServiceEndpoint, _speechServiceKey);
            SpeechRecognitionProxy.Initialize(_speechServiceEndpoint, _speechServiceKey);
            TextAnalyticsProxy.Initialize(_textAnalyticsEndpoint, _textAnalyticsKey);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
