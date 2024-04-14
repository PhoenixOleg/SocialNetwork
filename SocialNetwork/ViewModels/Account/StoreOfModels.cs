namespace SocialNetwork.ViewModels.Account
{
    public class StoreOfModels
    {
        public RegisterViewModel RegisterView { get; set; }
        public LoginViewModel LoginView { get; set; }

        public StoreOfModels() 
        {
            RegisterView = new RegisterViewModel();
            LoginView = new LoginViewModel();
        }
    }
}
