using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_UnderTheHood.Dtos;

namespace WebApp_UnderTheHood.Pages
{
    [Authorize(Policy = "HrManagerOnly")]
    public class HrManagerModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;

        [BindProperty] 
        public List<WeatherForecastDto> WeatherForecastItems { get; set; } = new List<WeatherForecastDto>();
        
        public HrManagerModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        
        public async Task OnGetAsync()
        {
            var httpClient = httpClientFactory.CreateClient(name: "OurWebApi");

            WeatherForecastItems = await httpClient.GetFromJsonAsync<List<WeatherForecastDto>>("WeatherForecast") ?? new List<WeatherForecastDto>();
        }
    }
}
