using EM.Domain.Model;
using EM.Repository.Repository;
using EM.Web.Models;
using EM.Web.Util;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EM.Web.Controllers
{
	public class AlunoController : Controller
	{
		private readonly ILogger<AlunoController> _logger;
		public AlunoController(ILogger<AlunoController> logger)
		{
			_logger = logger;
		}
		private readonly RepositorioAluno repositorio = new();
		const int semMatricula = 0;

		public IActionResult Index(string opcao, string buscar)
		{
			IEnumerable<Aluno> aluno = repositorio.GetAll();
			if (opcao == null)
			{
				return View(aluno);
			}
			if (opcao == "matricula")
			{
				if (!int.TryParse(buscar, out int busca))
				{
					ViewBag.Erro = "Matricula não encontrada";
					return View(aluno);
				}
				else
				{
					Aluno alunoMatricula = repositorio.GetByMatricula(busca);
					if (alunoMatricula.Matricula == semMatricula)
					{
						ViewBag.Erro = "Matricula não encontrada";
						return View(aluno);
					}
					IEnumerable<Aluno> AlunoMatriculaConvertida = Enumerable.Repeat(alunoMatricula, 1);
					return View(AlunoMatriculaConvertida);
				}
			}
			if (opcao == "nome")
			{
				if (buscar != null)
				{
					IEnumerable<Aluno> alunoNome = repositorio.GetByContendoNoNome(Convert.ToString(buscar));
					if(alunoNome.Count() != semMatricula)
					{
                        return View(alunoNome);
                    }
					else 
					{ 
						ViewBag.Erro = "Nome não encontrado";
						return View(aluno);
					}
				}
				else
				{
					ViewBag.Erro = "Nome não encontrado";
					return View(aluno);
				}
			}
			return View();
		}
		public IActionResult CadastreEdite(int id)
		{
			if (id == semMatricula)
			{
				return View();
			}
			else
			{
				var aluno = repositorio.GetByMatricula(id);
				return View(aluno);
			}
		}

		[HttpPost]
		public ActionResult CadastreEdite(Aluno aluno)
		{
			if (aluno.Matricula == semMatricula)
			{
				if (aluno.CPF == null)
				{
					repositorio.Add(aluno);
					return RedirectToAction("Index");
				}
				if (ValidacaoDeCpf.IsCpf(aluno.CPF))
				{
					repositorio.Add(aluno);
					return RedirectToAction("Index");
				}
				else
				{
					ViewBag.Erro = "CPF invalido";
					return View(aluno);
				}
			}
			else
			{
				if (aluno.CPF == null)
				{
					repositorio.Update(aluno);
					return RedirectToAction("Index");
				}
				if (ValidacaoDeCpf.IsCpf(aluno.CPF))
				{
					repositorio.Update(aluno);
					return RedirectToAction("Index");
				}
				else
				{
					ViewBag.Erro = "CPF invalido";
					return View(aluno);
				}
			}
		}

		public ActionResult Delete(int id)
		{
			var aluno = repositorio.GetByMatricula(id);
			repositorio.Remove(aluno);
			return RedirectToAction("Index");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}