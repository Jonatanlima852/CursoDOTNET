
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ApiCatalogo.Validations
{
    public class PrimeiraLetraMaiusculaAttribute : ValidationAttribute
    {
        // value é a entrada a ser validada
        // Validation context oferece infos adicionais, como instância de modelo criada por model binding
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) 
        {
            // Aqui é onde vai a lógica da validação. Observe que temos que considerar as validações feitas por outros atributos e bypassar essas lógicas

            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            } // se não passar pela required já termina validação

            var PrimeiraLetra = value.ToString()[0].ToString();  // pegando primeira letra do nome
            if(PrimeiraLetra != PrimeiraLetra.ToUpper())
            {
                return new ValidationResult("A primeira letra do nome do produto deve ser maiúscula");
            }

            return ValidationResult.Success;


        }
    }


}