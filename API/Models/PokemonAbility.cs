using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class PokemonAbility
    {
        public int PokeId { get; set; }
        public int PokeType { get; set; }
        public string AbilityName { get; set; }
        public virtual Pokemon Pokemon { get; set; }
        public virtual PokemonType PokemonType { get; set; }
    }
}
