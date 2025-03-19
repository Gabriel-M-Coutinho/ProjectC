using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CardGame.Database
{
    public class CardAbility
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CardId { get; set; }

        [Required]
        public string AbilityKey { get; set; } // Identificador da habilidade (ex: "first_strike")

        public string Parameters { get; set; } // Parâmetros da habilidade, pode ser em formato JSON

        // Relação com a carta
        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }

        // Relação com as traduções da habilidade
        public virtual ICollection<AbilityTranslation> Translations { get; set; }

        // Construtor para inicializar as coleções
        public CardAbility()
        {
            Translations = new List<AbilityTranslation>();
        }
    }
}