using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CardGame.Database
{
    public class CardGameDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardTranslation> CardTranslations { get; set; }
        public DbSet<CardAbility> CardAbilities { get; set; }
        public DbSet<AbilityTranslation> AbilityTranslations { get; set; }
        public DbSet<Language> Languages { get; set; }

        // Construtor sem parâmetros para uso simples
        public CardGameDbContext() { }

        // Construtor para receber opções de configuração
        public CardGameDbContext(DbContextOptions<CardGameDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configuração padrão se não for fornecida externamente
                optionsBuilder.UseSqlite("Data Source=cardgame.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações dos relacionamentos

            // Card e suas traduções (1:N)
            modelBuilder.Entity<Card>()
                .HasMany(c => c.Translations)
                .WithOne(t => t.Card)
                .HasForeignKey(t => t.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            // Card e suas habilidades (1:N)
            modelBuilder.Entity<Card>()
                .HasMany(c => c.Abilities)
                .WithOne(a => a.Card)
                .HasForeignKey(a => a.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            // Habilidade e suas traduções (1:N)
            modelBuilder.Entity<CardAbility>()
                .HasMany(a => a.Translations)
                .WithOne(t => t.Ability)
                .HasForeignKey(t => t.AbilityId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices para melhorar a performance
            modelBuilder.Entity<Card>()
                .HasIndex(c => c.CardKey)
                .IsUnique();

            modelBuilder.Entity<CardTranslation>()
                .HasIndex(t => new { t.CardId, t.LanguageCode })
                .IsUnique();

            modelBuilder.Entity<Language>()
                .HasIndex(l => l.Code)
                .IsUnique();
        }
    }
}