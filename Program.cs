using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ApiInventario.Data;
using ApiInventario.Services;
using Serilog;
using ApiInventario.Security;
//para jwt//
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

/****************RESGISTRAR LOS SERVICIOS**************************
Es el punto de entrada de tu aplicación (como el main en otros lenguajes).
Aqui se Define como funciona mi App
*/

var builder = WebApplication.CreateBuilder(args); //1.Construye la aplicación


// =====================
// 1.Servicios
// =====================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => 
	{ c.AddSecurityDefinition("Bearer",
		new Microsoft.OpenApi.Models.OpenApiSecurityScheme { 
			Name = "Authorization", 
			Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http, 
			Scheme = "Bearer", 
			BearerFormat = "JWT", 
			In = Microsoft.OpenApi.Models.ParameterLocation.Header, 
			Description = "Ingrese el token JWT: Bearer {token}" });
			
	c.AddSecurityRequirement(
		new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
			{ new Microsoft.OpenApi.Models.OpenApiSecurityScheme { 
				Reference = new Microsoft.OpenApi.Models.OpenApiReference { 
								Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, 
								Id = "Bearer" } }, new string[] {} } }); });


builder.Services.AddControllers()  //2.Registra servicios (inyección de dependencias)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });  
	

// =====================
// 2. Entity Framework
// =====================
 builder.Services.AddDbContext<AppDbContext>(options => /**Conecta la app a la BD**/
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// =====================
// 3.Servicios propios
// =====================
//PERMISOS
builder.Services.AddScoped<IPermissionService, PermissionService>();// PERMISO DE ACCESO DEL USUARIO POR ROL
//PRODUCTOS
builder.Services.AddScoped<IProductoService, ProductoService>(); //Ae agrego el servicio de Productos
//COMPRAS
builder.Services.AddScoped<ICompraService, CompraService>(); //Se agrego servicio de Compras
//VENTAS
builder.Services.AddScoped<IVentaService, VentaService>(); //Se agrego servicio  de Ventas
//REPORTES
builder.Services.AddScoped<IReporteService, ReporteService>();// se agrego servicio de Reportes

// =====================
// 4.JWT
// =====================
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IAuthService, AuthService>(); //Servicio de Autorizacion

builder.Services.AddScoped<ITokenService, TokenService>(); // Servicio de Tokenizacion

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<PasswordService>();

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{

		options.TokenValidationParameters = new TokenValidationParameters
		{

			ValidateIssuer = true,

			ValidateAudience = true,

			ValidateLifetime = true,

			ValidateIssuerSigningKey = true,


			ValidIssuer =
			builder.Configuration["Jwt:Issuer"],


			ValidAudience =
			builder.Configuration["Jwt:Audience"],


			IssuerSigningKey =
			new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(
					builder.Configuration["Jwt:Key"]!
				)
			)

		};

	});
	
// =====================
// 5.Construir aplicación
// =====================
var app = builder.Build();  //3. Construye la app final


// =====================
// 6. Middleware
// =====================
//builder.Host.UseSerilog();
// Swagger
if (app.Environment.IsDevelopment()) //4. Configura el pipeline (middleware)-Doumentacion
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseMiddleware<ApiInventario.Middleware.ExceptionMiddleware>(); // Uso de Middleware

// Middleware (orden IMPORTANTE) //5. Filtros antes de llegar  a la BD
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();