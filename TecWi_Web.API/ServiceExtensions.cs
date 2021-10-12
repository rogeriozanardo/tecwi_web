﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Application.Services;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Dapper;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Data.Repositories;
using TecWi_Web.Data.UoW;

namespace TecWi_Web.API
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration iConfiguration)
        {
            services.AddDbContext<DataContext>(x => x.UseSqlServer(iConfiguration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IDapper, Data.Dapper.Dapper>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioAplicacaoRepository, UsuarioAplicacaoRepository>();
            services.AddScoped<IPagarReceberRepository, PagarReceberRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IContatoCobrancaRepository, ContatoCobrancaRepository>();
            services.AddScoped<IContatoCobrancaLancamentoRepository, ContatoCobrancaLancamentoRepository>();
            services.AddScoped<ILogOperacaoRepository, LogOperacaoRepository>();
            services.AddScoped<IClienteContatoReposiry, ClienteContatoRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IAutorizacaoService, AutorizacaoService>();
            services.AddScoped<IUsuarioAplicacaoService, UsuarioAplicacaoService>();
            services.AddScoped<IPagarReceberService, PagarReceberService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IContatoCobrancaService, ContatoCobrancaService>();
            services.AddScoped<ILogOperacaoService, LogOperacaoService>();
            services.AddScoped<IClienteContatoService, ClienteContatoService>();
            return services;
        }

        public static IServiceCollection AddHttpContextAcessors(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }

        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration iConfiguration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                        .GetBytes(iConfiguration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            return services;
        }
    }
}
