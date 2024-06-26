﻿namespace Pokemons.API.Dto.Responses;

public class TapperConfigResponseDto
{
    public int Balance { get; set; }
    public int Coins { get; set; }
    public int DamagePerClick { get; set; }
    public int Energy { get; set; }
    public int CurrentEnergy { get; set; }
    public int SuperChargeRemaining { get; set; }
    public required CommitDamageResponseDto EntityData { get; set; }
}