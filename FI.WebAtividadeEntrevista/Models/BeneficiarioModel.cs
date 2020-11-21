using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        public long Id { get; set; }


        [Required]
        public long IdCliente { get; set; }
  
        [Required]
        public string NomeBeneficiario { get; set; }

        [Required(ErrorMessage = "CPF Beneficiario obrigatório")]
        [CPFCustomValidation(ErrorMessage = "CPF inválido")]
        public string CPFBeneficiario { get; set; }
    }
}