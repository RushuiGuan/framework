using Albatross.Authentication;
using Albatross.Caching;
using Albatross.Config;
using Albatross.Config.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace Albatross.Hosting {
	public class Startup {
		public const string DefaultApp_RootPath = "wwwroot";
		
		protected AuthorizationSetting AuthorizationSetting { get; }
		protected IConfiguration Configuration { get; }
		JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		public virtual bool Swagger { get; } = true;
		public virtual bool WebApi { get; } = true;
		public virtual bool Secured { get; } = false;
		public virtual bool Spa { get; } = false;
		public virtual bool Grpc { get; } = false;
		public virtual bool Caching { get; } = false;

		public Startup(IConfiguration configuration) {
			this.Configuration = configuration;
			Log.Logger.Information("AspNetCore Startup configuration with secured={secured}, spa={spa}, swagger={swagger}, grpc={grpc}, webapi={webapi}, caching={caching}", Secured, Spa, Swagger, Grpc, WebApi, Caching);
			if (Secured && WebApi) {
				AuthorizationSetting = new GetAuthorizationSetting(configuration).Get();
			} else {
				AuthorizationSetting = new AuthorizationSetting();
			}
		}

		protected virtual void ConfigureCors(CorsPolicyBuilder builder) {
			builder.AllowAnyHeader();
			builder.AllowAnyMethod();
			builder.AllowCredentials();
			var cors = this.Configuration.GetSection("cors").Get<string[]>() ?? new string[0];
			builder.WithOrigins(cors);
		}

		#region swagger
		public virtual IServiceCollection AddSwagger(IServiceCollection services) {
			services.AddOpenApiDocument(cfg => {
				cfg.Title = this.GetType().Assembly.GetName().Name;
				cfg.PostProcess = doc => { };
				if (Secured && AuthorizationSetting.IsBearerAuthentication) {
					cfg.AddSecurity("bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme {
						Type = OpenApiSecuritySchemeType.OAuth2,
						Description = "OpenId Authentication",
						Flow = OpenApiOAuth2Flow.Implicit,
						Flows = new OpenApiOAuthFlows() {
							Implicit = new OpenApiOAuthFlow() {
								Scopes = AuthorizationSetting.SwaggerScopes.ToDictionary<SwaggerScope, string, string>(args => args.Name, args => args.Description),
								AuthorizationUrl = AuthorizationSetting.AuthorizeUrl,
								TokenUrl = AuthorizationSetting.TokenUrl,
							},
						}
					});
				}
				cfg.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("bearer"));
			});
			return services;
		}
		public virtual void UseSwagger(IApplicationBuilder app) {
			app.UseOpenApi().UseSwaggerUi3(options => {
				options.DocumentPath = "/swagger/v1/swagger.json";
				if (Secured) {
					options.OAuth2Client = new NSwag.AspNetCore.OAuth2ClientSettings() {
						ClientId = AuthorizationSetting.SwaggerClientId,
					};
				}
			});
		}
		#endregion

		#region authorization
		protected virtual void ConfigureAuthorization(AuthorizationOptions option) {
			foreach (var policy in this.AuthorizationSetting.Policies ?? new AuthorizationPolicy[0]) {
				option.AddPolicy(policy.Name, builder => BuildAuthorizationPolicy(builder, policy));
			}
		}

		private void BuildAuthorizationPolicy(AuthorizationPolicyBuilder builder, AuthorizationPolicy policy) {
			if (policy.Roles?.Length > 0) {
				builder.RequireRole(policy.Roles);
			}
		}

		public virtual IServiceCollection AddAccessControl(IServiceCollection services) {
			services.AddConfig<AuthorizationSetting, GetAuthorizationSetting>();
			services.AddAuthorization(ConfigureAuthorization);
			AuthenticationBuilder builder = services.AddAuthentication(AuthorizationSetting.Authentication);

			if (AuthorizationSetting.IsKerborosAuthentication) {
				builder.AddNegotiate();
			} else if (AuthorizationSetting.IsBearerAuthentication) {
				builder.AddJwtBearer(AuthorizationSetting.BearerAuthenticationScheme, options => {
					options.Authority = AuthorizationSetting.Authority;
					options.Audience = AuthorizationSetting.Audience;
					options.RequireHttpsMetadata = false;
					options.IncludeErrorDetails = true;
					options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters {
						ValidateIssuerSigningKey = false,
					};
				});
			}
			return services;
		}
		#endregion

		public IServiceCollection AddSpa(IServiceCollection services) {
			services.AddSpaStaticFiles(cfg => cfg.RootPath = DefaultApp_RootPath);
			services.AddConfig<AngularConfig, GetAngularConfig>(true);
			services.AddSingleton<ITransformAngularConfig, TransformAngularConfig>();
			return services;
		}
		public virtual void ConfigureJsonOption(JsonOptions options) { }
		public virtual void ConfigureServices(IServiceCollection services) {
			services.TryAddSingleton<Microsoft.Extensions.Logging.ILogger>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger("default"));

			if (WebApi) {
				services.AddControllers().AddJsonOptions(ConfigureJsonOption);
				services.AddCors(opt => opt.AddDefaultPolicy(ConfigureCors));
				services.AddAspNetCorePrincipalProvider();
				if (Swagger) {
					services.AddMvc();
					AddSwagger(services);
				}
			}
			if (Spa) { AddSpa(services); }
			if (Secured) { AddAccessControl(services); }
			if (Caching) {
				services.AddCaching();
			}
		}

		public virtual void Configure(IApplicationBuilder app, ProgramSetting programSetting, EnvironmentSetting environmentSetting, ILogger<Startup> logger) {
			logger.LogInformation("Initializing {@program} with environment {environment}", programSetting, environmentSetting.Value);
			app.UseExceptionHandler(new ExceptionHandlerOptions { ExceptionHandler = HandleGlobalExceptions });
			app.UseRouting();
			if (WebApi) {
				app.UseCors();
				if (Secured) {
					app.UseAuthentication().UseAuthorization();
				}
				app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
			}
			if (Grpc) { app.UseEndpoints(endpoints => MapGrpcServices(endpoints)); }
			if (WebApi && Swagger) { UseSwagger(app); }
			if (Spa) { UseSpa(app, logger); }
			if (Caching) {
				Albatross.Caching.Extension.UseCache(app.ApplicationServices);
			}
		}

		public virtual void MapGrpcServices(IEndpointRouteBuilder endpoints) { }

		public void UseSpa(IApplicationBuilder app, ILogger<Startup> logger) {
			var config = app.ApplicationServices.GetRequiredService<AngularConfig>();
			logger.LogInformation("Initializing SPA with request path of '{requestPath}' and baseHref of '{baseRef}'", config.RequestPath, config.BaseHref);
			var options = new StaticFileOptions { 
				 RequestPath = config.RequestPath,
			};
			app.UseSpaStaticFiles(new StaticFileOptions { RequestPath = config.RequestPath });
			app.Map(config.RequestPath ?? string.Empty, web => web.UseSpa(spa => { }));
			app.ApplicationServices.GetRequiredService<ITransformAngularConfig>().Transform();
		}

		protected async Task HandleGlobalExceptions(HttpContext context) {
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			context.Response.ContentType = MediaTypeNames.Application.Json;
			var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;

			if (error != null) {
				var msg = CreateExceptionMessage(error);
				msg.StatusCode = context.Response.StatusCode;
				await JsonSerializer.SerializeAsync(context.Response.BodyWriter.AsStream(), msg, JsonSerializerOptions);
			}
		}

		protected virtual ErrorMessage CreateExceptionMessage(Exception error) {
			var msg = new ErrorMessage {
				Message = error.Message,
				Type = error.GetType().FullName,
			};
			if(error.InnerException != null) {
				msg.InnerError = CreateExceptionMessage(error.InnerException);
			}
			return msg;
		}
	}
}