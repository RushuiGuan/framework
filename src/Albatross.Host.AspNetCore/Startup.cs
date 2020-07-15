using Albatross.Authentication;
using Albatross.Config;
using Albatross.Config.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSwag;
using NSwag.Generation.Processors.Security;
using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.Host.AspNetCore {
	public class Startup {
		public const string DefaultApp_RootPath = "wwwroot";
		public const string DefaultApp_BaseHref = "";
		public const string BearerAuthenticationScheme = "Bearer";
		public IConfiguration Configuration { get; }
		protected AuthorizationSetting AuthorizationSetting { get; }
		protected ProgramSetting ProgramSetting { get; }
		JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { 
			 PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		public Startup(IConfiguration configuration) {
			Configuration = configuration;
			ProgramSetting = new GetProgramSetting(configuration).Get();
			AuthorizationSetting = new GetAuthorizationSetting(Configuration).Get();
		}

		protected virtual void ConfigureCors(CorsPolicyBuilder builder) {
			builder.AllowAnyHeader();
			builder.AllowAnyMethod();
			builder.AllowCredentials();
			builder.SetIsOriginAllowed(args => true);
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
							Scopes = AuthorizationSetting.SwaggerScopes.ToDictionary<SwaggerScope, string, string>(args=>args.Name, args=>args.Description),
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
		protected virtual void ConfigureAuthorization(AuthorizationOptions options) { }
		/// <summary>
		/// special treatment is needed for access token transmitted by signalr web sockets.  It is sent using a query string.  <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-3.1"/>
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public virtual IServiceCollection AddAccessControl(IServiceCollection services) {
			services.AddConfig<AuthorizationSetting, GetAuthorizationSetting>();
			services.AddAuthorization(ConfigureAuthorization);
			services.AddAuthentication(BearerAuthenticationScheme)
				.AddJwtBearer(BearerAuthenticationScheme, options => {
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

		public virtual void ConfigureServices(IServiceCollection services) {
			services.AddControllers();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddConfig<ProgramSetting, GetProgramSetting>();
			services.AddCors(opt => opt.AddDefaultPolicy(ConfigureCors));
			services.AddAspNetCorePrincipalProvider();
			services.AddMvc();
			AddSwagger(services);
			AddSpa(services);
			AddAccessControl(services);
		}

		public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, ProgramSetting program, ILogger<Startup> logger) {
			logger.LogInformation("Initializing {@program}", program);
			//app.UseHttpsRedirection();
			app.UseExceptionHandler(new ExceptionHandlerOptions { ExceptionHandler = HandleGlobalExceptions});
			app.UseRouting();
			app.UseCors();
			app.UseAuthentication().UseAuthorization();
			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
			UseSwagger(app);
			UseSpa(app);
		}

		public void UseSpa(IApplicationBuilder app) {
			app.UseStaticFiles();
			app.UseSpaStaticFiles();
			string baseRef = DefaultApp_BaseHref;
			app.Map(baseRef, web => web.UseSpa(spa => { }));
		}

		protected async Task HandleGlobalExceptions(HttpContext context) {
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			context.Response.ContentType = MediaTypeNames.Application.Json;
			var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
			
			if (error != null) {
				var msg = CreateExceptionMessage(error);
				msg.HttpStatus = context.Response.StatusCode;
				await JsonSerializer.SerializeAsync(context.Response.BodyWriter.AsStream(), msg, JsonSerializerOptions);
			}
		}

		protected virtual ErrorMessage CreateExceptionMessage(Exception error) {
			return new ErrorMessage{
				Message = error.Message,
				Type = error.GetType().FullName,
			};
		}
	}
}