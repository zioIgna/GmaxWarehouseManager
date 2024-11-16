





namespace Gmax.Models.Services.ArticoloCK
{
    public interface IArticoloCKService
    {
        Task<Entities.ArticoloCK?> GetArticoloCKByKeyAsync(string tipoArticolo, string codiceArticolo);
    }
}