using Coworking.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Coworking.MVC.Controllers
{
    public class ReservaController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7065/api/Reserva";

        public ReservaController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var reservas = await _httpClient.GetFromJsonAsync<List<ReservaViewModel>>($"{_baseUrl}/ObterTodas");
            return View(reservas);
        }

        public async Task<IActionResult> Criar()
        {
            var usuarios = await _httpClient.GetFromJsonAsync<List<UsuarioViewModel>>("https://localhost:7065/api/Usuario/ObterUsuarios");
            var salas = await _httpClient.GetFromJsonAsync<List<SalaViewModel>>("https://localhost:7065/api/Sala/ObterSalas");

            ViewBag.Usuarios = usuarios;
            ViewBag.Salas = salas;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Criar(CriarReservaViewModel model)
        {
            try
            {

                var usuario = await _httpClient.GetFromJsonAsync<UsuarioViewModel>($"https://localhost:7065/api/Usuario/ObterUsuarioPorId/{model.UsuarioId}");
                var sala = await _httpClient.GetFromJsonAsync<SalaViewModel>($"https://localhost:7065/api/Sala/ObterSalaPorId/{model.SalaId}");

                model.NomeUsuario = usuario?.Nome ?? "Desconhecido";
                model.EmailUsuario = usuario?.Email ?? "testeerro@email.com";
                model.NomeSala = sala?.Nome ?? "Desconhecida";                

                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Criar", model);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                return RedirectToAction("Criar");
            }
            catch (Exception e)
            {
                var mensagem = e.Message;
                throw;
            }
        }

        public async Task<IActionResult> Editar(Guid id)
        {
            var reserva = await _httpClient.GetFromJsonAsync<ReservaViewModel>($"{_baseUrl}/ObterPorId/{id}");
            var salas = await _httpClient.GetFromJsonAsync<List<SalaViewModel>>("https://localhost:7065/api/Sala/ObterSalas");

            ViewBag.Salas = salas;
            return View(reserva);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Guid id, EditarReservaViewModel model)
        {
            var usuario = await _httpClient.GetFromJsonAsync<UsuarioViewModel>($"https://localhost:7065/api/Usuario/ObterUsuarioPorId/{model.UsuarioId}");
            var sala = await _httpClient.GetFromJsonAsync<SalaViewModel>($"https://localhost:7065/api/Sala/ObterSalaPorId/{model.SalaId}");

            model.NomeUsuario = usuario?.Nome ?? "Desconhecido";
            model.NomeSala = sala?.Nome ?? "Desconhecida";
            model.EmailUsuario = usuario?.Email ?? "aooteste@email.com";
            
            var json = JsonSerializer.Serialize(model);

            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/Editar/{id}", model);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(model);
        }

        public async Task<IActionResult> Cancelar(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/Cancelar/{id}");
            return RedirectToAction("Index");
        }
    }
}
