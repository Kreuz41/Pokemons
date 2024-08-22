namespace Pokemons.DataLayer.Database.Models.Entities
{
    public class Wallet
    {
        public long Id { get; set; }
        public long PlayerId { get; set; }
        public decimal TRXBalance { get; set; } = 0;
        public decimal USDTBalance { get; set; }
        public Player Player { get; set; } = null!;// Навигационное свойство

    }
}
