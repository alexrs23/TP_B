using Microsoft.AspNetCore.Components;
using RCLAPI.Services;
using System;

namespace RCLAPI.Shared
{
    public partial class PagRegister : ComponentBase
    {
        [Inject]
        public IApiServices _apiServices { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private string Username { get; set; } = string.Empty;
        private string Email { get; set; } = string.Empty;
        private string Password { get; set; } = string.Empty;
        private string Tel { get; set; } = string.Empty;
        private string ErrorMessage { get; set; } = string.Empty;
        private bool IsSuccess { get; set; } = false;
        private bool IsLoading { get; set; } = false;

        private async Task RegisterUser()
        {
            Console.WriteLine("ENTREI no Reg");
            IsLoading = true;
            ErrorMessage = string.Empty;
            IsSuccess = false;
            try
            {
                var response = await _apiServices.RegistarUtilizador(Username, Email, Password, Tel);
                if (response.Data)
                {
                    IsSuccess = true;
                    NavigationManager.NavigateTo("/login");
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
                StateHasChanged();
            }
        }
    }
}