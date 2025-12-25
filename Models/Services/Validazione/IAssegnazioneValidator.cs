using Gmax.Models.ViewModels.AssegnazioneModal;

namespace Gmax.Models.Services.Validazione
{
    public interface IAssegnazioneValidator
    {
        Task<IEnumerable<(string MemberName, string ErrorMessage)>> ValidateAsync(AssegnazioneModalViewModel model, CancellationToken cancellationToken = default);
    }
}