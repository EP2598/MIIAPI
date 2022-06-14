using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Pokemon
    {
        public Pokemon()
        {
            this.PokemonAbilities = new HashSet<PokemonAbility>();
        }
        [Key]
        public int PokeId { get; set; }
        public string Pokename { get; set; }
        public int PokeType { get; set; }
        public virtual ICollection<PokemonAbility> PokemonAbilities {get; set;}
    }
}
