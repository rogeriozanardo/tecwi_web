using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.API.HangFireJobs;
using TecWi_Web.Application.MappingProfiles;

namespace TecWi_Web.API
{
    public class Startup
    {
        const string CRON_JOB_UMA_HORA = "0 */1 * * *";
        const string CRON_JOB_DUAS_HORAS = "0 */2 * * *";
        const string CRON_JOB_VINTE_MINUTOS = "*/20 * * * *";
        const string CRON_JOB_TRINTA_MINUTOS = "*/30 * * * *";
        const string CRON_JOB_QUARENTA_MINUTOS = "*/40 * * * *";

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabase(_configuration);
            services.AddControllers();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddRepositories();
            services.AddHttpContextAcessors();
            services.AddAuthentication(_configuration);
            services.AddServices();

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(_configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer(options => options.WorkerCount = 1);
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(cors => cors
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
                );

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHangfireDashboard();

            recurringJobManager.AddOrUpdate<ProdutoJobs>("Sincronizar_Produtos_DataBase", x => x.SincronizarProdutosAsync(), CRON_JOB_DUAS_HORAS);
            recurringJobManager.AddOrUpdate<ProdutoJobs>("Enviar_Produtos_Mercocamp", x => x.EnviarProdutosMercocampAsync(), CRON_JOB_TRES_HORAS);
            recurringJobManager.AddOrUpdate<PedidoJobs>("Sincronizar_Pedidos_DataBases", x => x.SincronizarPedidosAsync(), CRON_JOB_VINTE_MINUTOS);
            recurringJobManager.AddOrUpdate<PedidoJobs>("Atualizar_Status_Pedidos_DataBases", x => x.AlterarStatusPedidoFaturadoParaEncerradoAsync(), CRON_JOB_UMA_HORA);
            recurringJobManager.AddOrUpdate<PedidoJobs>("Enviar_Pedidos_Mercocamp", x => x.EnviarPedidosMercoCampAsync(), CRON_JOB_VINTE_MINUTOS);
            recurringJobManager.AddOrUpdate<MovimentoFiscalJobs>("Enviar_Notas_Fiscais", x => x.EnviarNotas(), "*/2 * * * *");

            //#### Usado para teste de 1 e 2 minutos o job do hang fire
            //recurringJobManager.AddOrUpdate<ProdutoJobs>("Sincronizar_Produtos_DataBase", x => x.SincronizarProdutosAsync(), "*/1 * * * *");
            //recurringJobManager.RemoveIfExists("Enviar_Produtos_Mercocamp");
            //recurringJobManager.AddOrUpdate<ProdutoJobs>("Enviar_Produtos_Mercocamp", x => x.EnviarProdutosMercocampAsync(), "*/2 * * * *");
            //recurringJobManager.AddOrUpdate<PedidoJobs>("Sincronizar_Pedidos_DataBases", x => x.SincronizarPedidosAsync(), "*/4 * * * *");
            //recurringJobManager.AddOrUpdate<PedidoJobs>("Atualizar_Status_Pedidos_DataBases", x => x.AlterarStatusPedidoFaturadoParaEncerradoAsync(), "*/1 * * * *");
            //recurringJobManager.RemoveIfExists("Enviar_Pedidos_Mercocamp");
            //recurringJobManager.AddOrUpdate<PedidoJobs>("Enviar_Pedidos_Mercocamp", x => x.EnviarPedidosMercoCampAsync(), "*/2 * * * *");

            // recurringJobManager.RemoveIfExists("Sincronizar_Notas_Fiscais");
            // recurringJobManager.AddOrUpdate<MovimentoFiscalJobs>("Sincronizar_Notas_Fiscais", x => x.Sincronizar(), "*/2 * * * *");

            //recurringJobManager.RemoveIfExists("Enviar_Notas_Fiscais");
            //recurringJobManager.AddOrUpdate<MovimentoFiscalJobs>("Enviar_Notas_Fiscais", x => x.EnviarNotas(), "*/2 * * * *");
        }
    }
}
