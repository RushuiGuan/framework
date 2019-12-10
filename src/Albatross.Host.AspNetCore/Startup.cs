using Albatross.Authentication;
using Albatross.Config;
using Albatross.Config.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.Host.AspNetCore {
	public class Startup {
		public const string DefaultApp_RootPath = "wwwroot";
		public const string DefaultApp_BaseHref = "";
		public const string BearerAuthenticationScheme = "Bearer";
		public IConfiguration Configuration { get; }
		AuthorizationSetting AuthorizationSetting => new GetAuthorizationSetting(Configuration).Get();

		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		protected virtual void ConfigureCors(CorsPolicyBuilder builder) {
			builder.AllowAnyHeader();
			builder.AllowAnyMethod();
			builder.AllowCredentials();
			builder.SetIsOriginAllowed(args => true);
		}

		public virtual void AddCustomServices(IServiceCollection services) {
		}

		#region swagger
		public virtual IServiceCollection AddSwagger(IServiceCollection services) {
			services.AddOpenApiDocument(cfg => {
				cfg.Title = this.GetType().Assembly.GetName().Name;
				cfg.PostProcess = doc => { };
				cfg.AddSecurity("bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme {
					Type = OpenApiSecuritySchemeType.OAuth2,
					Description = "OpenId Authentication",
					Flow = OpenApiOAuth2Flow.Implicit,
					Flows = new OpenApiOAuthFlows() {
						Implicit = new OpenApiOAuthFlow() {
							Scopes = new Dictionary<string, string> {
								{ AuthorizationSetting.SwaggerScope, string.Empty},
							},
							AuthorizationUrl = AuthorizationSetting.AuthorizeUrl,
							TokenUrl = AuthorizationSetting.TokenUrl,
						},
					}
				});
				cfg.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("bearer"));
			});
			return services;
		}
		public virtual void UseSwagger(IApplicationBuilder app) {
			app.UseOpenApi().UseSwaggerUi3(options => {
				options.DocumentPath = "/swagger/v1/swagger.json";
				options.OAuth2Client = new NSwag.AspNetCore.OAuth2ClientSettings() {
					ClientId = AuthorizationSetting.SwaggerClientId,
				};
			});
		}
		#endregion

		#region authorization
		public virtual IServiceCollection AddIdentityServer(IServiceCollection services) {
			services.AddConfig<AuthorizationSetting, GetAuthorizationSetting>();
			services.AddAuthorization();
			services.AddAuthentication(BearerAuthenticationScheme).AddJwtBearer(BearerAuthenticationScheme, options => {
				options.Authority = AuthorizationSetting.Authority;
				options.Audience = AuthorizationSetting.Audience;
				options.RequireHttpsMetadata = false;
				options.IncludeErrorDetails = true;
				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters {
					ValidateIssuerSigningKey = false,
				};
			});
			return services;
		}
		#endregion

		public IServiceCollection AddSpa(IServiceCollection services) {
			services.AddSpaStaticFiles(cfg => cfg.RootPath = DefaultApp_RootPath);
			return services;
		}

		public void ConfigureServices(IServiceCollection services) {
			services.AddControllers();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddConfig<ProgramSetting, GetProgramSetting>();
			services.AddSingleton<GlobalExceptionHandler>();
			services.AddCors(opt => opt.AddDefaultPolicy(ConfigureCors));
			services.AddAspNetCorePrincipalProvider();
			services.AddMvc();
			AddSwagger(services);
			AddSpa(services);
			AddIdentityServer(services);
			AddCustomServices(services);
		}

		public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, GlobalExceptionHandler globalExceptionHandler, ProgramSetting program) {
			var environments = new {
				aspnetcore = env.EnvironmentName,
				program = program.Environment,
			};
			Log.Information("Environments: @{environments}", environments);
			//app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthentication().UseAuthorization();
			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
			});

			app.UseCors();
			app.UseExceptionHandler(new ExceptionHandlerOptions { ExceptionHandler = context => globalExceptionHandler.RunAsync(context) });
			UseSwagger(app);
			UseSpa(app);
			
		}
		public void UseSpa(IApplicationBuilder app) {
			app.UseStaticFiles();
			app.UseSpaStaticFiles();
			string baseRef = DefaultApp_BaseHref;
			app.Map(baseRef, web => web.UseSpa(spa => { }));
		}
	}
}