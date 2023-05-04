using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using Npgsql;
using NpgsqlTypes;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var fileStream = File.OpenRead(@"C:\Users\madsj\OneDrive\Skrivebord\intro.pdf");
            var streamContent = new StreamContent(fileStream);
            var formData = new MultipartFormDataContent();
            
            // name and fileName can be w/e. The name dont matter from what i understand, its just for identifying it. The first is field and 2nd is the file name
            formData.Add(streamContent, "file", "intro.pdf");

            var connectionString = "Host=localhost;Username=postgres;Password=12345;Database=complexit_files";

            
            await SaveFileToPostgres(formData, connectionString, "intro", "Room123");
        }

        static async Task SaveFileToPostgres(MultipartFormDataContent formData, string connectionString, string fileName, string room)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = "INSERT INTO files(name, room, data) VALUES (@name, @room, @data)";
                    cmd.Parameters.AddWithValue("name", fileName);
                    cmd.Parameters.AddWithValue("room", room);
                    

                    using (var stream = await formData.ReadAsStreamAsync())
                    {
                        // read the stream into a byte array
                        byte[] data = new byte[stream.Length];
                        await stream.ReadAsync(data, 0, (int)stream.Length);

                        // set the bytea parameter
                        cmd.Parameters.AddWithValue("data", NpgsqlDbType.Bytea, data);
                    }

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}