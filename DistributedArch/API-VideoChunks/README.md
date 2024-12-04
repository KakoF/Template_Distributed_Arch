# Escopo

```
CREATE TABLE VideoChunks (
    Id INT PRIMARY KEY IDENTITY(1,1),
    VideoId INT,
    ChunkIndex INT,
    ChunkData VARBINARY(MAX)
);




using System;
using System.Data.SqlClient;
using System.IO;

public class VideoChunker
{
    public void StoreVideoInChunks(string videoPath, int chunkSize)
    {
        byte[] videoData = File.ReadAllBytes(videoPath);
        int totalChunks = (int)Math.Ceiling((double)videoData.Length / chunkSize);
        
        using (SqlConnection conn = new SqlConnection("sua-string-de-conexao"))
        {
            conn.Open();
            for (int i = 0; i < totalChunks; i++)
            {
                int offset = i * chunkSize;
                int size = Math.Min(chunkSize, videoData.Length - offset);
                byte[] chunkData = new byte[size];
                Array.Copy(videoData, offset, chunkData, 0, size);

                SqlCommand cmd = new SqlCommand("INSERT INTO VideoChunks (VideoId, ChunkIndex, ChunkData) VALUES (@VideoId, @ChunkIndex, @ChunkData)", conn);
                cmd.Parameters.AddWithValue("@VideoId", 1); // Supondo que estamos armazenando um único vídeo
                cmd.Parameters.AddWithValue("@ChunkIndex", i);
                cmd.Parameters.AddWithValue("@ChunkData", chunkData);

                cmd.ExecuteNonQuery();
            }
        }
    }
}


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

public class VideoController : ControllerBase
{
    [HttpGet("video/{videoId}")]
    public async Task<IActionResult> GetVideo(int videoId)
    {
        SqlConnection conn = new SqlConnection("sua-string-de-conexao");
        await conn.OpenAsync();

        SqlCommand cmd = new SqlCommand("SELECT ChunkData FROM VideoChunks WHERE VideoId = @VideoId ORDER BY ChunkIndex", conn);
        cmd.Parameters.AddWithValue("@VideoId", videoId);

        SqlDataReader reader = await cmd.ExecuteReaderAsync();

        MemoryStream videoStream = new MemoryStream();
        while (await reader.ReadAsync())
        {
            byte[] chunk = (byte[])reader["ChunkData"];
            await videoStream.WriteAsync(chunk, 0, chunk.Length);
        }

        videoStream.Position = 0;
        return File(videoStream, "video/mp4");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}



```