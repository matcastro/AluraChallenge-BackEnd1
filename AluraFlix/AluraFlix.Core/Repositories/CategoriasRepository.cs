using AluraFlix.Core.Configs;
using AluraFlix.Core.Models;
using AluraFlix.Core.Models.Requests;
using AluraFlix.Core.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AluraFlix.Core.Repositories
{
    public class CategoriasRepository : ICategoriasRepository
    {
        private readonly AluraFlixConfig _config;
        public CategoriasRepository(IOptions<AluraFlixConfig> config)
        {
            _config = config.Value;
        }

        public async Task<(bool success, Categoria categoria)> Change(CategoriaUpdateRequest request)
        {
            var categoria = new Categoria();
            var success = false;
            var query = "UPDATE CATEGORIAS " +
                "SET TITULO = @titulo, COR = @cor " +
                "WHERE ID = @id";
            using (var conn = new SqlConnection(_config.Database.ConnectionString))
            {
                var command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@titulo", request.Titulo);
                command.Parameters.AddWithValue("@cor", request.Cor);
                command.Parameters.AddWithValue("@id", request.Id);
                try
                {
                    conn.Open();
                    long updated_rows = await command.ExecuteNonQueryAsync();
                    if (updated_rows == 1)
                    {
                        success = true;
                    }
                    categoria.Id = request.Id;
                    categoria.Titulo = request.Titulo;
                    categoria.Cor = request.Cor;
                }
                catch (Exception ex)
                {
                    success = false;
                    Console.WriteLine(ex.Message);
                }
            }
            return (success, categoria);
        }

        public async Task<bool> Delete(long id)
        {
            var success = false;
            var query = "DELETE FROM CATEGORIAS " +
                "WHERE ID = @id";
            using (var conn = new SqlConnection(_config.Database.ConnectionString))
            {
                var command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@id", id);
                try
                {
                    conn.Open();
                    var deleted_rows = await command.ExecuteNonQueryAsync();
                    if (deleted_rows > 0)
                    {
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return success;
        }

        public async Task<Categoria> Find(long id)
        {
            var categoria = new Categoria();
            var query = "SELECT ID, TITULO, COR " +
                "FROM CATEGORIAS " +
                "WHERE ID = @id";
            using (var conn = new SqlConnection(_config.Database.ConnectionString))
            {
                var command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@id", id);
                try
                {
                    conn.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        categoria.Id = reader.GetInt32(0);
                        categoria.Titulo = reader.GetString(1);
                        categoria.Cor = reader.GetString(2);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return categoria;
        }

        public async Task<IList<Categoria>> ListAll()
        {
            var categorias = new List<Categoria>();
            var query = "SELECT ID, TITULO, COR" +
                " FROM CATEGORIAS";
            using (var conn = new SqlConnection(_config.Database.ConnectionString))
            {
                var command = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var categoria = new Categoria
                        {
                            Id = reader.GetInt32(0),
                            Titulo = reader.GetString(1),
                            Cor = reader.GetString(2)
                        };

                        categorias.Add(categoria);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return categorias;
        }

        public async Task<(bool success, Categoria categoria)> Register(CategoriaRequest request)
        {
            var categoria = new Categoria();
            var success = false;
            var query = "INSERT INTO CATEGORIAS (TITULO, COR) OUTPUT INSERTED.ID " +
                "VALUES (@titulo, @cor)";
            using (var conn = new SqlConnection(_config.Database.ConnectionString))
            {
                var command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@titulo", request.Titulo);
                command.Parameters.AddWithValue("@cor", request.Cor);
                try
                {
                    conn.Open();
                    var inserted_id = (int)await command.ExecuteScalarAsync();
                    if (inserted_id > 0)
                    {
                        success = true;
                    }
                    categoria.Id = inserted_id;
                    categoria.Titulo = request.Titulo;
                    categoria.Cor = request.Cor;
                }
                catch (Exception ex)
                {
                    success = false;
                    Console.WriteLine(ex.Message);
                }
            }
            return (success, categoria);
        }
    }
}
