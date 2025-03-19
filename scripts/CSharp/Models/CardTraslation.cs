using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardGame.Database
{
    public class CardTranslation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CardId { get; set; }

        [Required]
        public string LanguageCode { get; set; }  // ex: "pt-BR", "en-US", "es-ES"

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        // Relação com a carta
        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }
    }
}