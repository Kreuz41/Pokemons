﻿using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Cache.Models;

public class ReferralList
{
    public IEnumerable<Player> Referrals { get; set; } = [];
}