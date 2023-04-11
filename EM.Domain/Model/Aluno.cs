using EM.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace EM.Domain.Model
{
    public class Aluno : IEntidade
    {
        public int Matricula { get; set; }

		[StringLength(100, MinimumLength = 3)]
		[RegularExpression(@"^(?!\s*$)[a-zA-ZÀ-ÿ\s]*$", ErrorMessage = "Apenas letras.")]
		[Required]
        public string Nome { get; set; }

        [StringLength(11, MinimumLength = 11)]
		[RegularExpression(@"^[0-9]+$", ErrorMessage = "Apenas Numeros.")]
		public string? CPF { get; set; }

		public DateTime Nascimento { get; set; }

		public EnumeradorSexo Sexo { get; set; }

		public string NomeFormatado
		{
			get
			{
				if (!string.IsNullOrEmpty(Nome))
				{
					string[] partesNome = Nome.ToLower().Split(' ');
					for (int i = 0; i < partesNome.Length; i++)
					{
						if (partesNome[i].Length > 0)
						{
							partesNome[i] = partesNome[i].Substring(0, 1).ToUpper() + partesNome[i].Substring(1);
						}
					}
					return string.Join(" ", partesNome);
				}
				else
				{
					return string.Empty;
				}
			}
		}
		public string CPFFormatado
        {
            get
            {
                if (!string.IsNullOrEmpty(CPF))
                {
                    return Convert.ToUInt64(CPF).ToString(@"000\.000\.000\-00");
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string NascimentoFormatado 
        { 
            get { return Nascimento.ToString("dd/MM/yyyy"); }
        }
        
        public override bool Equals(object? obj)
        {
            return Matricula.Equals(obj);
        }
        public override int GetHashCode()
        {
            return Matricula.GetHashCode();
        }
        public override string ToString() => base.ToString();
    }
}

