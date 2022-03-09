using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmesAPI.Data.Dtos
{
    public class UpdateFilmeDto
    {
        public string Titulo { get; set; }
        [Required(ErrorMessage = "O nome do diretor do filme é obrigatório.")]
        public string Diretor { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [StringLength(30, ErrorMessage = "O campo gênero tem um limite de 30 caracteres.")]
        public string Genero { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [Range(1, 180, ErrorMessage = "O filme deve ter a duração mínima de 1 minúto e máxima de 180 minutos.")]
        public int Duracao { get; set; }
    }
}
