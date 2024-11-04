using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Net.Http.Headers;

namespace APP_ALUMNOS_CONSUME_API.Models
{
    public class AlumnosService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string _urlApi;

        public AlumnosService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _urlApi = builder.GetSection("ApiConnection").Value;
        }

        public async Task<List<AlumnosView>> Consultar()
        {
            var alumnos = new List<AlumnosView>();
            try
            {
                var responseTask = await client.GetAsync(_urlApi);
                if (responseTask.IsSuccessStatusCode)
                {
                    var resJson = await responseTask.Content.ReadAsStringAsync();
                    alumnos = JsonConvert.DeserializeObject<List<AlumnosView>>(resJson);
                }
                else
                {
                    throw new Exception($"WebApi respondio con error {responseTask.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"WebApi respondio con error {ex.Message}");
            }
            return alumnos;
        }
        public async Task<AlumnosView> Consultar(int? id)
        {
            var alumnos = new AlumnosView();
            try
            {
                var responseTask = await client.GetAsync(_urlApi + $"/{id}");
                if (responseTask.IsSuccessStatusCode)
                {
                    var resJson = await responseTask.Content.ReadAsStringAsync();
                    alumnos = JsonConvert.DeserializeObject<AlumnosView>(resJson);
                }
                else
                {
                    throw new Exception($"WebApi respondio con error {responseTask.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"WebApi respondio con error {ex.Message}");
            }
            return alumnos;
        }
        public async Task<AlumnosView> Agregar(AlumnosView alumnos)
        {
            try
            {
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(alumnos), Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var responseTask = await client.PostAsync(_urlApi, httpContent);
                if (responseTask.IsSuccessStatusCode)
                {
                    var resJson = await responseTask.Content.ReadAsStringAsync();

                    alumnos = JsonConvert.DeserializeObject<AlumnosView>(resJson);
                }
                else
                {
                    throw new Exception($"WebApi respondio con error {responseTask.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"WebApi respondio con error {ex.Message}");
            }
            return alumnos;
        }
        public async Task Actualizar(AlumnosView alumnos)
        {
            try
            {
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(alumnos), Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var responseTask = await client.PostAsync(_urlApi + $"/{alumnos.Id}", httpContent);
                if(!responseTask.IsSuccessStatusCode)
                {
                    throw new Exception($"error{responseTask.StatusCode}");
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Error{ex.Message}");
            }
        }
       
        public async Task Eliminar(int id)
        {
            try
            {
                var responseTask = await client.DeleteAsync(_urlApi + $"/{id}");
                if (!responseTask.IsSuccessStatusCode)
                {
                    throw new Exception($"ERROR {responseTask.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
