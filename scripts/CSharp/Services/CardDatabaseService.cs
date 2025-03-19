using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CardGame.Database
{
    public class CardDatabaseService
    {
        private readonly CardGameDbContext _dbContext;

        public CardDatabaseService()
        {
            _dbContext = new CardGameDbContext();
            _dbContext.Database.EnsureCreated();

            // Inicializa idiomas padr�o se necess�rio
            InitializeLanguages();
        }

        private void InitializeLanguages()
        {
            if (!_dbContext.Languages.Any())
            {
                _dbContext.Languages.AddRange(
                    new Language { Code = "pt-BR", Name = "Portugu�s (Brasil)", IsDefault = true },
                    new Language { Code = "en-US", Name = "English (US)" },
                    new Language { Code = "es-ES", Name = "Espa�ol" },
                    new Language { Code = "jp" , Name = "Japanese"}
                );
                _dbContext.SaveChanges();
            }
        }

        // Adicionar uma nova carta com tradu��o no idioma padr�o
        public Card AddCard(string cardKey, string imagePath, int attack, int life,
                          string name, string description, string type = null,
                          string rarity = null, int manaCost = 0)
        {
            // Criar a carta
            var card = new Card
            {
                CardKey = cardKey,
                ImagePath = imagePath,
                Attack = attack,
                Life = life,
                Type = type,
                Rarity = rarity,
                ManaCost = manaCost
            };

            _dbContext.Cards.Add(card);
            _dbContext.SaveChanges();

            // Adicionar tradu��o no idioma padr�o
            var defaultLanguage = _dbContext.Languages.FirstOrDefault(l => l.IsDefault);
            if (defaultLanguage != null)
            {
                AddCardTranslation(card.Id, defaultLanguage.Code, name, description);
            }

            return card;
        }

        // Adicionar tradu��o para uma carta
        public CardTranslation AddCardTranslation(int cardId, string languageCode,
                                               string name, string description)
        {
            var translation = new CardTranslation
            {
                CardId = cardId,
                LanguageCode = languageCode,
                Name = name,
                Description = description
            };

            _dbContext.CardTranslations.Add(translation);
            _dbContext.SaveChanges();

            return translation;
        }

        // Adicionar habilidade a uma carta
        public CardAbility AddCardAbility(int cardId, string abilityKey, string parameters,
                                        string name, string description, string languageCode = null)
        {
            // Se languageCode n�o for fornecido, usa o idioma padr�o
            if (string.IsNullOrEmpty(languageCode))
            {
                var defaultLanguage = _dbContext.Languages.FirstOrDefault(l => l.IsDefault);
                languageCode = defaultLanguage?.Code ?? "pt-BR";
            }

            var ability = new CardAbility
            {
                CardId = cardId,
                AbilityKey = abilityKey,
                Parameters = parameters
            };

            _dbContext.CardAbilities.Add(ability);
            _dbContext.SaveChanges();

            // Adicionar tradu��o da habilidade
            var abilityTranslation = new AbilityTranslation
            {
                AbilityId = ability.Id,
                LanguageCode = languageCode,
                Name = name,
                Description = description
            };

            _dbContext.AbilityTranslations.Add(abilityTranslation);
            _dbContext.SaveChanges();

            return ability;
        }

        // Obter todas as cartas
        public List<Card> GetAllCards()
        {
            return _dbContext.Cards
                .Include(c => c.Translations)
                .Include(c => c.Abilities)
                    .ThenInclude(a => a.Translations)
                .ToList();
        }

        // Obter todas as cartas com tradu��es em um idioma espec�fico
        public List<Card> GetAllCards(string languageCode)
        {
            return _dbContext.Cards
                .Include(c => c.Translations.Where(t => t.LanguageCode == languageCode))
                .Include(c => c.Abilities)
                    .ThenInclude(a => a.Translations.Where(t => t.LanguageCode == languageCode))
                .ToList();
        }

        // Obter uma carta pelo ID
        public Card GetCardById(int id, string languageCode = null)
        {
            // Se languageCode n�o for fornecido, usa o idioma padr�o
            if (string.IsNullOrEmpty(languageCode))
            {
                var defaultLanguage = _dbContext.Languages.FirstOrDefault(l => l.IsDefault);
                languageCode = defaultLanguage?.Code ?? "pt-BR";
            }

            return _dbContext.Cards
                .Include(c => c.Translations.Where(t => t.LanguageCode == languageCode))
                .Include(c => c.Abilities)
                    .ThenInclude(a => a.Translations.Where(t => t.LanguageCode == languageCode))
                .FirstOrDefault(c => c.Id == id);
        }

        // Obter uma carta pela chave
        public Card GetCardByKey(string cardKey, string languageCode = null)
        {
            // Se languageCode n�o for fornecido, usa o idioma padr�o
            if (string.IsNullOrEmpty(languageCode))
            {
                var defaultLanguage = _dbContext.Languages.FirstOrDefault(l => l.IsDefault);
                languageCode = defaultLanguage?.Code ?? "pt-BR";
            }

            return _dbContext.Cards
                .Include(c => c.Translations.Where(t => t.LanguageCode == languageCode))
                .Include(c => c.Abilities)
                    .ThenInclude(a => a.Translations.Where(t => t.LanguageCode == languageCode))
                .FirstOrDefault(c => c.CardKey == cardKey);
        }
    }
}