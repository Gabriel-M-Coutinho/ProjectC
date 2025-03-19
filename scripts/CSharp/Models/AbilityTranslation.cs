using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardGame.Database
{
    public class AbilityTranslation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AbilityId { get; set; }

        [Required]
        public string LanguageCode { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        // Relação com a habilidade
        [ForeignKey("AbilityId")]
        public virtual CardAbility Ability { get; set; }
    }
}