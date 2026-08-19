namespace DoWork.Services.KeycloakClientValidationService;

using System.Threading.Tasks;

public interface IKeycloakClientValidationService
{
    Task ValidateClientsAsync();
}
