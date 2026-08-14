using GameZone.DatabaseContext;
using GameZone.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GameZone.Interfaces__contrato_condicion_.InterfacesUsuario; 
using GameZone.Servicios.Servicios_Usuario;
using GameZone.Servicios.Token;
using GameZone.Interfaces__contrato_condicion_.InterfacesEmpresa;
using GameZone.Servicios.Servicios_Empresa;
using GameZone.Interfaces__contrato_condicion_.InterfacesToken;

var builder = WebApplication.CreateBuilder(args);

//SE REALIZA EL PROCESO DE CADENA DE CONEXIÓN Y CREACIÓN DEL SERVICIO PARA AÑADIR EL DBCONTEXT
var ConnectionString = builder.Configuration.GetConnectionString("SupabaseConnection");

//Evalua la connectionString como un string, solamente de esa manera puede terminar si es vacío o nulo
if (string.IsNullOrEmpty(ConnectionString))
{
    throw new InvalidOperationException("La conexíón con la base de datos es inválida o no se ha proporcionado. Por favor, revise la configuración.");
}

//Crear un DBContext para la aplicacion, de esta manera, usamos MYSQL (por sintaxis, pide la cadena de conexion y version del servidor de esta misma cadena de conexion)
builder.Services.AddDbContext<GameZonerDBContext>(options =>
{
    options.UseNpgsql(ConnectionString);

});

//Construir el servico de CORS  
builder.Services.AddCors(options =>
//Se usan llaves cuando se realizan varias configuraciones consecutivas, asi como se aprecia, options realmente necesita 2 configuraciones. Tambien se pueden manejar varios options a la vez
       {
           options.AddPolicy(name: "React-Front",
                policy => //Las arrow functions son funciones anonimas, es decir, que no tienen nombre, y se usan para pasar funciones como argumentos a otras funciones. En este caso, se pasa una funcion anonima que recibe un objeto policy y se le aplica la configuracion de CORS
                //Se interpretan de forma facil como: "se tiene el objeto policy , y se le aplica la configuracion de CORS que se le pasa como argumento a la funcion anonima"
                {
                    policy.WithOrigins("https://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
       });

//Servicio de Tokens
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";  //Configuro el nombre de mi esquema y es el mismo nombre que DEBE aparecer en el Header del Token. Para poder validar y de lo contrario, gracias a DefaultChallengeScheme, indica que hace falta ese fragmento de autorizacion llamado "Bearer"
    options.DefaultChallengeScheme = "Bearer";
})
    .AddJwtBearer("Bearer", options => //Esquema de configuracion y opciones adicionales para validar el Token (Bearer es mi esquema de autenticacion por default)
        {
            options.TokenValidationParameters= new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime= true,             //Parametros de configuración para desempaquetado de Token y verificación de su autenticidad
                ValidateIssuerSigningKey= true,
                ValidIssuer= builder.Configuration["Issuer"],
                ValidAudience= builder.Configuration["Audience"],
                IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["ApiSettings:SecretKey"]))
            };
    });

builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AccesoRestringidoAdmin", policy => policy.RequireRole("Admin")); //Para emplear RequireRole, realmente quiere decir que necesita un Claim de tipo Rol con un valor String de "Admin"
            options.AddPolicy("AccesoGlobalLogueado", policy => policy.RequireRole("Cliente", "Publisher"));
    }
);

//Los siguientes Servicios necesitan realizar una instancia por petición HTTP. Pero solo tener vida útil durante esa petición en específico. Por eso se maneja AddScoped
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<InterfaceUsuarioClaims, UserHttpContextValidatorClaims>();
builder.Services.AddScoped<InterfaceUsuarioController, UsuarioClaseServicioControlador>();

builder.Services.AddScoped<InterfaceTokenService,TokenService>();

builder.Services.AddScoped<InterfaceEmpresaValidarClaims, EmpresaHttpContextValidatorClaims>();
builder.Services.AddScoped<InterfaceEmpresaController, EmpresaClaseServicioControlador>();
//

//Constructor para el servicio de AutoMapper
builder.Services.AddAutoMapper(typeof(UsuarioProfile));
builder.Services.AddControllers(); //Crear el servicio para añadir los controladores

//TODOS LOS SERVICIOS DE LA APLICACIÓN SE AGREGAN ANTES DE CONSTRUIRLA, POR LO TANTO, SI SE QUIERE AGREGAR UN SERVICIO NUEVO, SE DEBE HACER ANTES DE ESTA LÍNEA
// Add services to the container.
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("React-Front"); //Se le pasa como argumento el nombre de la politica para que reciba peticiones
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//No se necesitan configurar endpoints, los endpoints ya se configuran por defecto en los controladores mediante ApiController y las URL de cada método
app.MapControllers();
app.MapRazorPages();
app.Run();
//Para alternar entre proyectos, se usa el comando cd ../GameZone o cd ../gamezone-Frontend
