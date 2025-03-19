using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CardGame.Database
{
    public class Card
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CardKey { get; set; } // Identificador único para a carta (ex: "black_mage")

        [Required]
        public string ImagePath { get; set; }

        public int Attack { get; set; }

        public int Life { get; set; }

        // Propriedades adicionais
        public string Type { get; set; } // Tipo da carta: Monstro, Mágica, etc.
        public string Rarity { get; set; } // Raridade: Comum, Rara, Lendária, etc.
        public int ManaCost { get; set; } // Custo de mana para jogar

        // Relacionamentos - serão carregados via navegação do EF Core
        public virtual ICollection<CardTranslation> Translations { get; set; }
        public virtual ICollection<CardAbility> Abilities { get; set; }

        // Construtor para inicializar as coleções
        public Card()
        {
            Translations = new List<CardTranslation>();
            Abilities = new List<CardAbility>();
        }
    }
}