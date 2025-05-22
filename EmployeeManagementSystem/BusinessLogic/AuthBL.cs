using EmployeeManagementSystem.Models.Auth;
using EmployeeManagementSystem.Network;
using EmployeeManagementSystem.Services;
using Firebase.Auth;

namespace EmployeeManagementSystem.BusinessLogic
{
    public class AuthBL
    {
        private readonly FirebaseAuthService _firebaseService;
        private readonly AuthNetworkService _networkService;

        public AuthBL(FirebaseAuthService firebaseService, AuthNetworkService networkService)
        {
            _firebaseService = firebaseService;
            _networkService = networkService;
        }

        public async Task<(bool Success, string ErrorMessage)> RegisterUserAsync(RegisterViewModel model)
        {
            try
            {
                var firebaseUser = await _firebaseService.SignUp(model.Email!, model.Password!);
                if (firebaseUser == null) return (false, "Registration failed with Firebase.");

                var token = await firebaseUser.GetIdTokenAsync();
                model.Password = null; // Exclude password

                var response = await _networkService.RegisterUserAsync(model, token);
                return response.IsSuccessStatusCode
                    ? (true, string.Empty)
                    : (false, "Failed to register on backend.");
            }
            catch (FirebaseAuthException ex)
            {
                var status = FirebaseAuthExceptionHandler.HandleException(ex);
                return (false, FirebaseAuthExceptionHandler.GenerateExceptionMessage(status));
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}