using Microsoft.AspNetCore.Components;
using RCLAPI.DTO;
using RCLAPI.Services;
using System.Net.NetworkInformation;

namespace RCLAPI.Shared {

    public partial class PagCarrinho : ComponentBase
    {
        [Inject]
        public IApiServices _apiServices { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private List<ItemCarrinhoCompra>? carrinhoItens { get; set; }
        private decimal carrinhoTotal { get; set; }
        private string? ErrorMessage { get; set; }

        private bool IsLoading { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            ErrorMessage = null;
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
            carrinhoItens = await _apiServices.GetItensDoCarrinho("user");
            if (carrinhoItens is null)
            {
                ErrorMessage = "Ocorreu um erro ao obter os dados da API";
            }
            CalcularTotal();
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