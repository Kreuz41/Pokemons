using System.Text.Json.Serialization;
using Pokemons.Core.Enums;

namespace Pokemons.DataLayer.Database.Models.Entities;

public class Player
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Username { get; set; }
    public string? PhotoUrl { get; set; }
    public long Balance { get; set; }
    public long GoldBalance { get; set; }
    public int DamagePerClick { get; set; } = 1;
    public bool IsFirstEntry { get; set; } = true;

    public int Level { get; set; }
    public int Exp { get; set; }
    
    public int Energy { get; set; } = 2000;
    public int CurrentEnergy { get; set; }
    public DateTime LastCommitDamageTime { get; set; } 
    public decimal EnergyCharge { get; set; } = 1.8m;

    public int SuperCharge { get; set; }
    public decimal SuperChargeCooldown { get; set; } = 7.8m;
    public DateTime LastSuperChargeActivatedTime { get; set; }
    
    public int Taps { get; set; }
    public int TotalDamage { get; set; }
    public int DefeatedEntities { get; set; }

    public bool IsPremium { get; set; } = false;
    public decimal CryptoBalance { get; set; }
    
    public MemberGuildStatus GuildStatus { get; set; } = null!;
    public Rating Rating { get; set; } = null!;
    [JsonIgnore] public Market Market { get; set; } = null!;
    [JsonIgnore] public ICollection<ReferralNode> ReferrerInfo { get; set; } = [];
    [JsonIgnore] public ICollection<Battle> Battles { get; set; } = [];
    [JsonIgnore] public ICollection<ReferralNode> Referrals { get; set; } = [];
    [JsonIgnore] public ICollection<Mission> Missions { get; set; } = [];
    [JsonIgnore] public ICollection<News> News { get; set; } = [];
    [JsonIgnore] public ICollection<Notification> Notifications { get; set; } = [];
}