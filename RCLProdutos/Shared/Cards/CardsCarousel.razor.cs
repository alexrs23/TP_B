using Microsoft.AspNetCore.Components;
using RCLAPI.DTO;
using RCLAPI.Services;
using RCLProdutos.Services.Interfaces;

namespace RCLProdutos.Shared.Cards
{
    public partial class CardsCarousel : ComponentBase
    {
       

        [Parameter]
        public int SelectedId { get; set; }

        [Parameter]
        public int catSel { get; set; }

        [Inject]
        public IApiServices _apiServices { get; set; }

        [Inject]
        public ICardsUtilsServices cardsUtilsServices { get; set; }

        private List<Categoria> categorias { get; set; }

        private bool IsDisabledNext { get; set; } = false;
        private bool IsDisbledPrevious { get; set; } = false;

        private int SelectCategoria;

        private int enviaCat;

        protected override async Task OnInitializedAsync()
        {
            enviaCat = catSel;

            catSel = 0;

            SelectCategoria = SelectedId;

            categorias = await _apiServices.GetCategorias();

            // Verificar se as categorias foram carregadas corretamente
            if (categorias == null || !categorias.Any())
            {
                Console.WriteLine("Nenhuma categoria carregada.");
            }
            else
            {
                Console.WriteLine($"Categorias carregadas: {categorias.Count}");
            }

            await LoadMarginsLeft();
            if(categorias != null)
            {
                int qtdProd = categorias.Count;
                cardsUtilsServices.OnChange += StateHasChanged;
            }

        }

        private async Task<List<Categoria>> LoadCategoriasAsync()
        {
            // Simulação de carregamento de dados
            await Task.Delay(1000); // Simula um atraso na resposta
            return new List<Categoria>
        {
            new Categoria { Id = 1, Nome = "Categoria 1" },
            new Categoria { Id = 2, Nome = "Categoria 2" }
        };
        }

        async Task LoadMarginsLeft()
        {
            if (categorias != null)
            {
                foreach (var categoria in categorias)
                {
                    cardsUtilsServices.MarginLeftSlide.Add("margin-left:0%");
                }
            }
        }

        void PreviousCard()
        {
            if (cardsUtilsServices.CountSlide > 0)  
            {
                cardsUtilsServices.MarginLeftSlide[cardsUtilsServices.CountSlide - 1] = "margin-left:0%";
                cardsUtilsServices.CountSlide--;
                IsDisabledNext = false;
                IsDisbledPrevious = false;
            }
            else
            {
                if (cardsUtilsServices.MarginLeftSlide.Count > 0) 
                {
                    cardsUtilsServices.MarginLeftSlide[0] = "margin-lef:0%";
                    IsDisbledPrevious = true;
                }

            }
            cardsUtilsServices.Index = cardsUtilsServices.CountSlide;
        }

        void NextCard()
        {
            if (cardsUtilsServices.CountSlide < cardsUtilsServices.MarginLeftSlide.Count)
            {
                cardsUtilsServices.MarginLeftSlide[cardsUtilsServices.CountSlide] = $"margin-left:-7%";
            }

            cardsUtilsServices.CountSlide++;
            cardsUtilsServices.Index = cardsUtilsServices.CountSlide;

            if (cardsUtilsServices.CountSlide >= cardsUtilsServices.MarginLeftSlide.Count)
            {
                IsDisabledNext = true;
            }
            else
            {
                IsDisabledNext = false;
            }

            IsDisbledPrevious = false;
        }
    }
}
