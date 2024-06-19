﻿namespace Pokemons.DataLayer.Database.Models.Entities;

public class ReferralNode
{
    public long Id { get; set; }
    
    public long ReferrerId { get; set; }
    public Player Referrer { get; set; } = null!;
    
    public long ReferralId { get; set; }
    public Player Referral { get; set; } = null!;
}