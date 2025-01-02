using Microsoft.AspNetCore.Components;
using RCLAPI.Services;

namespace RCLAPI.Shared
{
    public partial class PagLogin : ComponentBase
    {
        [Inject]
        public IApiServices _apiServices { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private string Email { get; set; } = string.Empty;
        private string Password { get; set; } = string.Empty;
        private string ErrorMessage { get; set; } = string.Empty;
        private bool IsLoading { get; set; } = false;


        private async Task LoginUser()
        {
            Console.WriteLine("ENTREI no Log");
            IsLoading = true;
            ErrorMessage = string.Empty; // Resetar o erro
            try
            {
                var response = await _apiServices.Login(Email, Password);

                if (response.Data)
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    ErrorMessage = response.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ocorreu um erro: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}