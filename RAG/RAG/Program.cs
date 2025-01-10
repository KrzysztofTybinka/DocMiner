
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace RAG
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

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

            app.UseHttpsRedirection();
            app.UseAuthorization();


            app.MapPost("/testocr", async (IFormFile file) =>
            {
                if (file == null)
                    return Results.BadRequest("No file was uploaded.");

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                // Prepare file content for forwarding
                var fileBytes = memoryStream.ToArray();
                var fileContent = new ByteArrayContent(fileBytes);

                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                using var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(fileContent, "file", file.FileName);

                // Send file to another app
                var httpClient = new HttpClient();
                var dockerAppUrl = "http://host.docker.internal:8081/ocr";
                var response = await httpClient.PostAsync(dockerAppUrl, multipartContent);

                if (!response.IsSuccessStatusCode)
                    return Results.StatusCode((int)response.StatusCode);

                var textFromFil = await response.Content.ReadAsStringAsync();

                return Results.Ok();
            }).DisableAntiforgery();

            app.Run();
        }
    }
}
