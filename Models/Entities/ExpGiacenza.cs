using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Gmax.Models.Entities
{
    [PrimaryKey(nameof(TipoArticolo),nameof(CodiceArticolo),nameof(CodMagazzino))]
    public class ExpGiacenza
    {
        [MaxLength(3)]
        [Required]
        public string TipoArticolo { get; set; }

        [MaxLength(30)]
        [Required]
        public string CodiceArticolo { get; set; }

        [Required]
        public string CodMagazzino { get; set; }
        public int QtaGiacenza { get; set; }
        public DateTime DataInserimento { get; set; }
    }
}
