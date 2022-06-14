using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class PokemonType
    {
        public PokemonType()
        {
            this.PokemonAbilities = new HashSet<PokemonAbility>();
        }
        [Key]
        public int PokeType { get; set; }
        public string Typename { get; set; }
        public virtual ICollection<PokemonAbility> PokemonAbilities { get; set; }
    }
}
