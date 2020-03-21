using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ElvantoMaps.Models;
using Microsoft.AspNetCore.Authorization;
using ElvantoMaps.Data;
using Geocoding.Google;
using Geocoding;
using System.Net.Http;
using System.Web;
using ElvantoApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

namespace ElvantoMaps.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/GetData")]
        [Authorize]
        public async Task<IActionResult> GetDataAsync()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            var elvanto = new ElvantoApi.Client(accessToken, true);
            var persons = (await elvanto.PeopleGetAllAsync(new ElvantoApi.Models.GetAllPeopleRequest()
            {
                Archived = "no",
                Contact = "no",
                Suspended = "no",
                Category_id = Environment.GetEnvironmentVariable("PERSON_CATEGORY"),
                Fields = new[] { "home_address", "home_address2", "home_city", "home_state", "home_postcode", "home_country", "birthday" }
            })).People.Person;
            var locations = this.db.Locations.ToList();

            await GetAllLocationsAsync(persons
                .Where(i => !locations.Any(j => LocationToString(j) == PersonAddressToString(i))));

            var result = this.db.Locations
                .ToList()
                .GroupJoin(persons.OrderBy(i => i.Birthday),
                           i => LocationToString(i),
                           i => PersonAddressToString(i),
                           (location, person) => new { person, location })
                .Where(i => i.person.Any())
                .GroupBy(i => i.location.FormattedAddress)
                .Select(i => i.First());

            return Json(result);
        }

        private static string PersonAddressToString(Person i)
        {
            return Normalization(i.Home_address + i.Home_address2 + i.Home_city + i.Home_postcode + i.Home_state + i.Home_country);
        }

        private static string LocationToString(Models.Location i)
        {
            return Normalization(i.Address + i.Address2 + i.City + i.PostCode + i.State + i.Country);
        }
        private static string Normalization(string i)
        {
            return i.Replace("  ", " ").Replace(" ", "").Replace("Straße", "Str.").Trim();
        }

        private async Task GetAllLocationsAsync(IEnumerable<Person> persons)
        {
            var httpclient = new HttpClient();

            foreach (var item in persons.Select(i => (i.Home_address, i.Home_address2, i.Home_city, i.Home_postcode, i.Home_state, i.Home_country)).Distinct())
            {
                var address = HttpUtility.UrlEncode($"{item.Home_address} {item.Home_address2} {item.Home_city} {item.Home_state} {item.Home_postcode} {item.Home_country}".Replace("  ", " ").Trim());
                var response = await httpclient.GetAsync($"https://maps.googleapis.com/maps/api/geocode/json?address={address}&key={Environment.GetEnvironmentVariable("GOOGLE_KEY")}");
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.GeocodingResponse>(await response.Content.ReadAsStringAsync());

                await Task.Delay(400);
                if (data.results.Any())
                {
                    Result match = data.results.First();
                    var coordinates = match.geometry.location;
                    db.Locations.Add(new Models.Location()
                    {
                        Address = item.Home_address,
                        Address2 = item.Home_address2,
                        City = item.Home_city,
                        PostCode = item.Home_postcode,
                        State = item.Home_state,
                        Country = item.Home_country,
                        FormattedAddress = match.formatted_address,
                        Latitude = coordinates.lat,
                        Longitude = coordinates.lng
                    });
                    await db.SaveChangesAsync();
                }
            }
        }

        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new Microsoft.AspNetCore.Authentication.AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
