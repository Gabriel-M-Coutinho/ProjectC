using Godot;
using System;

using System.ComponentModel.DataAnnotations;

namespace CardGame.Database
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; } // ex: "pt-BR"

        [Required]
        public string Name { get; set; } // ex: "Português (Brasil)"

        public bool IsDefault { get; set; }
    }
}