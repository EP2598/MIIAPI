using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.API
{
    public class PokemonVM
    {
        public string PokemonName { get; set; }
        public List<string> PokemonType { get; set; }
        public List<string> PokemonAbilities { get; set; }
    }
}
