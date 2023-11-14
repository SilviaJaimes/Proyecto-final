using System.Globalization;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using Dominio.Entities;
using Microsoft.Extensions.Logging;

namespace Persistencia;

public class ApiContextSeed
{
    public static async Task SeedAsync(ApiContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            var ruta = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            /* if (!context.Especies.Any())
            {
                using (var readerEntity = new StreamReader("../Persistencia/Data/Csvs/especie.csv"))
                {
                    using (var csv = new CsvReader(readerEntity, CultureInfo.InvariantCulture))
                    {
                        var list = csv.GetRecords<Especie>();
                        context.Especies.AddRange(list);
                        await context.SaveChangesAsync();
                    }
                }
            }  */

            /* if (!context.Razas.Any())
            {
                using (var reader = new StreamReader(ruta + @"\Data\Csvs\raza.csv"))
                {
                    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HeaderValidated = null, // Esto deshabilita la validación de encabezados
                        MissingFieldFound = null
                    }))
                    {
                        // Resto de tu código para leer y procesar el archivo CSV
                        var list = csv.GetRecords<Raza>();
                        List<Raza> entidad = new List<Raza>();
                        foreach (var item in list)
                        {
                            entidad.Add(new Raza
                            {
                                Id = item.Id,
                                IdEspecieFk = item.IdEspecieFk,
                                Nombre = item.Nombre
                            });
                        }
                        context.Razas.AddRange(entidad);
                        await context.SaveChangesAsync();
                    }
                }
            } */

            if (!context.Usuarios.Any())
            {
                using (var reader = new StreamReader(ruta + @"/Data/Csv/usuario.csv"))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var list = csv.GetRecords<Usuario>();
                        context.Usuarios.AddRange(list);
                        await context.SaveChangesAsync();
                    }
                }
            }

            if (!context.RolUsuarios.Any())
            {
                using (var reader = new StreamReader(ruta + @"\Data\Csv\rolUsuario.csv"))
                {
                    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HeaderValidated = null, // Esto deshabilita la validación de encabezados
                        MissingFieldFound = null
                    }))
                    {
                        // Resto de tu código para leer y procesar el archivo CSV
                        var list = csv.GetRecords<RolUsuario>();
                        List<RolUsuario> entidad = new List<RolUsuario>();
                        foreach (var item in list)
                        {
                            entidad.Add(new RolUsuario
                            {
                                IdUsuarioFk = item.IdUsuarioFk,
                                IdRolFk = item.IdRolFk
                            });
                        }
                        context.RolUsuarios.AddRange(entidad);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<ApiContext>();
            logger.LogError(ex.Message);
        }
    }
    public static async Task SeedRolesAsync(ApiContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            if (!context.Roles.Any())
            {
                var roles = new List<Rol>()
                        {
                            new Rol{Id=1, Nombre="Administrador"},
                            new Rol{Id=2, Nombre="Empleado"}
                        };
                context.Roles.AddRange(roles);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<ApiContext>();
            logger.LogError(ex.Message);
        }
    }
}