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
    public class VideosRepository : IVideosRepository
    {
        private readonly AluraFlixConfig _config;
        public VideosRepository(IOptions<AluraFlixConfig> config)
        {
            _config = config.Value;
        }

        public async Task<(bool success, Video video)> Change(VideoUpdateRequest request)
        {
            var video = new Video();
            var success = false;
            var query = "UPDATE VIDEOS " +
                "SET TITULO = @titulo, DESCRICAO = @descricao, URL = @url " +
                "WHERE ID = @id";
            using (var conn = new SqlConnection(_config.Database.ConnectionString))
            {
                var command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@titulo", request.Titulo);
                command.Parameters.AddWithValue("@descricao", request.Descricao);
                command.Parameters.AddWithValue("@url", request.Url);
                command.Parameters.AddWithValue("@id", request.Id);
                try
                {
                    conn.Open();
                    long updated_rows = await command.ExecuteNonQueryAsync();
                    if(updated_rows == 1)
                    {
                        success = true;
                    }
                    video.Id = request.Id;
                    video.Titulo = request.Titulo;
                    video.Descricao = request.Descricao;
                    video.Url = request.Url;
                }
                catch (Exception ex)
                {
                    success = false;
                    Console.WriteLine(ex.Message);
                }
            }
            return (success, video);
        }

        public async Task<bool> Delete(long id)
        {
            var success = false;
            var query = "DELETE FROM VIDEOS " +
                "WHERE ID = @id";
            using (var conn = new SqlConnection(_config.Database.ConnectionString))
            {
                var command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@id", id);
                try
                {
                    conn.Open();
                    var deleted_rows = await command.ExecuteNonQueryAsync();
                    if(deleted_rows > 0)
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

        public async Task<Video> Find(long id)
        {
            var video = new Video();
            var query = "SELECT ID, TITULO, DESCRICAO, URL " +
                "FROM VIDEOS " +
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
                        video.Id = reader.GetInt32(0);
                        video.Titulo = reader.GetString(1);
                        video.Descricao = reader.GetString(2);
                        video.Url = reader.GetString(3);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return video;
        }

        public async Task<IList<Video>> ListAll()
        {
            var videos = new List<Video>();
            var query = "SELECT ID, TITULO, DESCRICAO, URL" +
                " FROM VIDEOS";
            using (var conn = new SqlConnection(_config.Database.ConnectionString))
            {
                var command = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var video = new Video
                        {
                            Id = reader.GetInt32(0),
                            Titulo = reader.GetString(1),
                            Descricao = reader.GetString(2),
                            Url = reader.GetString(3)
                        };

                        videos.Add(video);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return videos;
        }

        public async Task<(bool success, Video video)> Register(VideoRequest request)
        {
            var video = new Video();
            var success = false;
            var query = "INSERT INTO VIDEOS (TITULO, DESCRICAO, URL) OUTPUT INSERTED.ID " +
                "VALUES (@titulo, @descricao, @url)";
            using (var conn = new SqlConnection(_config.Database.ConnectionString))
            {
                var command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@titulo", request.Titulo);
                command.Parameters.AddWithValue("@descricao", request.Descricao);
                command.Parameters.AddWithValue("@url", request.Url);
                try
                {
                    conn.Open();
                    var inserted_id = (int) await command.ExecuteScalarAsync();
                    if(inserted_id > 0)
                    {
                        success = true;
                    }
                    video.Id = inserted_id;
                    video.Titulo = request.Titulo;
                    video.Descricao = request.Descricao;
                    video.Url = request.Url;
                }
                catch (Exception ex)
                {
                    success = false;
                    Console.WriteLine(ex.Message);
                }
            }
            return (success, video);
        }
    }
}
