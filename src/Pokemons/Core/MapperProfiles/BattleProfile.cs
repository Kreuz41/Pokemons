using AutoMapper;
using Pokemons.API.Dto.Responses;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Core.MapperProfiles;

public class BattleProfile : Profile
{
    public BattleProfile()
    {
        CreateMap<Battle, CommitDamageResponseDto>();
    }
}