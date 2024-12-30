
using Microsoft.AspNetCore.Components;
using RCLAPI.DTO;
using RCLAPI.Services;
using RCLProdutos.Services.Interfaces;

namespace RCLProdutos.Shared.Slider
{
    public partial class SliderComponent
    {
        [SupplyParameterFromQuery]
        public string nomeCat { get; set; }

        [SupplyParameterFromQuery]
        public int Id { get; set; }

        [SupplyParameterFromQuery]
        private int compraSugerida { get; set; }

        [Parameter]
        public int? initProd { get; set; }

        [Inject]
        public IApiServices? _apiServices { get; set; }
       
        [Inject]
        public ISliderUtilsServices sliderUtilsService { get; set; }
        private List<ProdutoDTO>? produtos { get; set; }
        private List<ProdutoFavorito>? userFavoritos { get; set; }

        public ProdutoDTO sugestaoProduto = new ProdutoDTO();
        private int witdthPerc { get; set; } = 0;
        private bool IsDisabledNext { get; set; } = false;
        private bool IsDisbledPrevious { get; set; } = false;

        public static int? actualProd=0;
        protected override async Task OnInitializedAsync()
        {
            int? categoriasenviadaID;
            string? produtosEspecificos;

            if (Id == 0 && actualProd == 0 || nomeCat == "Todos")
            {
                produtosEspecificos = "todos";
                categoriasenviadaID = null;
            }
            else if (actualProd == Id)
            {
                categoriasenviadaID = Id;
                produtosEspecificos = "categoria";
            }
            else 
            {
                if (Id > 0)
                {
                    categoriasenviadaID = Id;
                    actualProd = Id;
                    produtosEspecificos = "categoria";
                }

                else
                {
                    categoriasenviadaID = actualProd;
                    produtosEspecificos = "categoria";
                }

            }

            try
            {
                produtos = await _apiServices!.GetProdutosEspecificos(produtosEspecificos, categoriasenviadaID);

                userFavoritos = await _apiServices!.GetFavoritos("Jorge");

                for (int i = 0; i < userFavoritos.Count; i++)
                    for (int j = 0; j < produtos.Count; j++)
                        if (produtos[j].Id == userFavoritos[i].ProdutoId)
                            produtos[j].Favorito = userFavoritos[i].Efavorito;

                Random random = new Random();

                int[]? indices = produtos
                                       .Where(item => item is not null)
                                       .Select(item => item.Id)
                                       .ToArray();

                int sugestaoProdutoId = random.Next(0, produtos.Count - 1);

                sugestaoProduto = produtos[indices[sugestaoProdutoId] - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

                await LoadMarginsLeft();
                if(produtos != null)
                {
                    int qtdProd = produtos.Count;

                    witdthPerc = qtdProd * 100;

                    sliderUtilsService.WidthSlide2 = 100f / qtdProd;

                    sliderUtilsService.OnChange += StateHasChanged;
                }
                
        }

        async Task LoadMarginsLeft()
        {
            if(produtos != null)
            {
                foreach (var produto in produtos)
                {
                    sliderUtilsService.MarginLeftSlide.Add("margin-left:0%");
                }
            }
            
        }

        void PreviousSlide()
        {
            if (sliderUtilsService.CountSlide > 0)
            {
                sliderUtilsService.CountSlide--; // Decrementa CountSlide primeiro
                sliderUtilsService.MarginLeftSlide[sliderUtilsService.CountSlide] = "margin-left:0%"; // Usa o CountSlide depois de decrementar
                IsDisabledNext = false;
                IsDisbledPrevious = false;
            }
            else
            {
                if (sliderUtilsService.MarginLeftSlide.Count > 0) // Verifica se a lista contém pelo menos 1 elemento
                {
                    sliderUtilsService.MarginLeftSlide[0] = "margin-lef:0%";
                    IsDisbledPrevious = true;
                }
            }
            sliderUtilsService.Index = sliderUtilsService.CountSlide;
        }


        void NextSlide()
        {
            if (sliderUtilsService.CountSlide < sliderUtilsService.MarginLeftSlide.Count) // Verifica se o indice é valido
            {

                string WidthSlideS = (Convert.ToString(sliderUtilsService.WidthSlide2));
                WidthSlideS = WidthSlideS.Replace(",", ".");

                //sliderUtilsService.MarginLeftSlide[sliderUtilsService.CountSlide - 1] = $"margin-left:-{WidthSlide}%";
                sliderUtilsService.MarginLeftSlide[sliderUtilsService.CountSlide] = $"margin-left:-{sliderUtilsService.WidthSlide2}%"; // Altera o indice aqui
            }


            sliderUtilsService.CountSlide++;
            sliderUtilsService.Index = sliderUtilsService.CountSlide;


            if (sliderUtilsService.CountSlide >= sliderUtilsService.MarginLeftSlide.Count) // Verifica depois do incremento
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
