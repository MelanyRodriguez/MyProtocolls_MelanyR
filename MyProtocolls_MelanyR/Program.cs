using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyProtocolls_MelanyR.Models;

namespace MyProtocolls_MelanyR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // vamos a leer la etiqueta CNNSTR de appsettings.json para configurar la conexion
            //a la base de datos
            var CnnStrBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("CNNSTR"));

            //eliminamos de la CNNSTR el dato del password ya que seria muy sencillo obtener la info 
            //de conexion del usuario de SQL Server del archivo de config apssettings.json
            CnnStrBuilder.Password = "123456";

            //CnnStrBuilder es un objeto que permite la construccion de cadenas de conexion a bases de datos
            //se pueden modificar cada parte de la misma, pero al final debemos extraer un string con la info final
            string cnnStr = CnnStrBuilder.ConnectionString;

            //conectamos el proyecto a la base de datos 
            builder.Services.AddDbContext<MyProtocolsBDContext>(options => options.UseSqlServer(cnnStr));








            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}