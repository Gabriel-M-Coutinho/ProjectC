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
        public string Code { get; set; } 

        [Required]
        public string Name { get; set; } 

        public bool IsDefault { get; set; }
    }
}