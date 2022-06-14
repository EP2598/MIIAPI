using API.Context;
using API.Models;
using API.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class PokemonRepository : Repository<MyContext, Pokemon, int>
    {
        private readonly MyContext _context;
        public PokemonRepository(MyContext context) : base(context)
        {
            this._context = context;
        }

        //public List<PokemonVM> GetPokemons()
        //{
        //    List<PokemonVM> listObj = new List<PokemonVM>();

        //    List<Pokemon> listPokemon = (from poke in _context.Pokemons select poke).ToList();

        //    foreach (var poke in listPokemon)
        //    {
        //        PokemonVM obj = new PokemonVM();

        //        obj.PokemonName = poke.Pokename;
                                
        //        if (listPokemon.Where(x => x.Pokename == poke.Pokename).Count() > 1)
        //        {
        //            List<int> PokeIds = (from Poke in _context.Pokemons
        //                                 where Poke.Pokename == poke.Pokename
        //                                 select Poke.PokeId).ToList();

        //            obj.PokemonType = (from PokeType in _context.PokemonTypes
        //                               where PokeType.PokeType == poke.PokeType
        //                               select PokeType.Typename).ToList();

        //            obj.PokemonAbilities = (from PokeAbility in _context.PokemonAbilities
        //                                    where PokeIds.Contains(poke.PokeId)
        //                                    select PokeAbility.AbilityName).ToList();
        //        }
        //        else
        //        {
        //            obj.PokemonType = (from PokeType in _context.PokemonTypes
        //                               where PokeType.PokeType == poke.PokeType
        //                               select PokeType.Typename).ToList();

        //            obj.PokemonAbilities = (from PokeAbility in _context.PokemonAbilities
        //                                    where PokeAbility.PokeId == poke.PokeId
        //                                    select PokeAbility.AbilityName).ToList();
        //        }

                
                


        //    }


        //}
    }
}
