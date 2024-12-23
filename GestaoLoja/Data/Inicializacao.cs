using Microsoft.AspNetCore.Identity;

namespace GestaoLoja.Data
{
    public static class Inicializacao
    {
        public static async Task CriaDadosIniciais(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Adicionar três perfis/roles 
            string[] roles = ["Admin", "Gestor", "Cliente"];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    IdentityRole roleRole = new IdentityRole(role);
                    await roleManager.CreateAsync(roleRole);
                }
            }

            //Adicionar Default User - Admin
            var defaultUser = new ApplicationUser
            {
                UserName = "Admin+01@mail.pt",
                Email = "admin01@mail.pt",
                Nome = "Admin",
                Apelido = "01",
                PhoneNumber = "123456789",
                NIF = 123456789,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);

                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Admin+01"); //password
                    await userManager.AddToRoleAsync(defaultUser, "Admin"); //role

                }
            }
        }
    }
}
