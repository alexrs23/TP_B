using Microsoft.AspNetCore.Components;
using RCLAPI.DTO;
using RCLAPI.Services;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace RCLAPI.Shared
{

    public partial class PagCarrinho : ComponentBase
    {
        [Inject]
        public IApiServices _apiServices { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        private List<ItemCarrinhoCompra>? carrinhoItens { get; set; }
        private decimal carrinhoTotal { get; set; }
        private string? ErrorMessage { get; set; }
        private bool IsLoading { get; set; } = false;
        private string? _clienteId;
        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            ErrorMessage = null;
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
                _clienteId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            await CarregarCarrinho();
            IsLoading = false;
        }

        private void Voltar()
        {
            NavigationManager.NavigateTo("/");
        }

        private void FinalizarCompra()
        {
            // Implementar a lógica para finalizar compra
            NavigationManager.NavigateTo("/");
        }

        private async Task CarregarCarrinho()
        {
            if (!string.IsNullOrEmpty(_clienteId))
            {
                carrinhoItens = await _apiServices.GetItensDoCarrinho(_clienteId);
                if (carrinhoItens is null)
                {
                    ErrorMessage = "Ocorreu um erro ao obter os dados da API";
                }
                CalcularTotal();
            }
            else
            {
                ErrorMessage = "Ocorreu um erro ao obter os dados da API - ClienteID vazio.";
            }

        }

        private async Task RemoverItem(int itemCarrinhoId)
        {
            IsLoading = true;
            ErrorMessage = null;
            await _apiServices.RemoveItemDoCarrinho(itemCarrinhoId);
            await CarregarCarrinho();
            IsLoading = false;
        }

        private void CalcularTotal()
        {
            carrinhoTotal = (decimal)(carrinhoItens?.Sum(item => item.ValorTotal) ?? 0);
        }
    }
}