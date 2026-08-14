using GameZone.Modelos;
using Microsoft.EntityFrameworkCore;
using GameZone.Modelos.Usuario;
using GameZone.Modelos.Empresa;


namespace GameZone.DatabaseContext
{
    public class GameZonerDBContext : DbContext
    {
        public GameZonerDBContext(DbContextOptions<GameZonerDBContext> options) : base(options)
        {
             //Es un constructor sin inyección de dependencias, su única función es generar el instanciamiento de la configuración de GameZonerDBContext que se configuró en Program CS y enviar ese objeto de configuración al constructor Padre en la Clase Padre (recordando que la clase padre se llama DbContext), de esa manera, puede realizar el instanciamiento de DBContext y que mi clase lo pueda usar
    
        }

        //Se crean las colecciones para cada uno de los objetos que se van a almacenar en la base de datos, en este caso, Videojuegos y Usuarios. Estas colecciones representan las tablas en la base de datos y permiten realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) sobre los datos almacenados.
        public DbSet<Videojuego> Videojuegos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Empresa> Empresas { get; set; }

        //Como UsuarioDTO NO es una tabla en nuestra base de datos, No se declara, C# Puede acceder a los otros modelos simplemente trayendo a la carpeta de modelos. En DB context solo se añaden los modelos que serán tablas en SQL

    }
}

//NOTA: ORGANIZAR LA ARQUITECTURA DE LA BASE DE DATOS PARA MAEJAR INTERFACES Y REPOSITORIOS DE OBJETOS. DE ESA MANERA, CUMPLIR CON EL DESACOPLAMIENTO Y ELIMINAR A LAS CLASES COMO ARGUMENTOS EN CONSTRUCTORES