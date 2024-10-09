using Microsoft.EntityFrameworkCore;

namespace Gmax.Models.Entities
{
    [PrimaryKey(nameof(CodiceArticolo), nameof(TipoArticolo))]
    public class Articolo
    {
        //public int Id { get; set; }
        public string CodiceArticolo { get; set; }
        public string TipoArticolo { get; set; }
        public string? Descrizione { get; set; }
        public string? UnitaMisura { get; set; }
        public int QtaScortaMin { get; set; }
        public int QtaImpCliente { get; set; }

    }
}
