using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Net;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        private readonly static string  KEY_TEMP_DATA_LISTA_BENEFICIARIOS = "ListaBeneficiarios";
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();

            List<string> erros = this.errorsWhenInclusionNewClient(model.CPF);

            if (erros.Count > 0)
            {
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                List<BeneficiarioModel> beneficiariosModel = Session[KEY_TEMP_DATA_LISTA_BENEFICIARIOS] as List<BeneficiarioModel>;
                List<Beneficiario> beneficiarios = mapModelBeneficiario(beneficiariosModel);
                model.Id = bo.Incluir(new Cliente()
                {                    
                    CEP = model.CEP,
                    CPF = model.CPF,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                }, beneficiarios);

           
                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
       
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                });
                               
                return Json("Cadastro alterado com sucesso");
            }
        }
        
        [HttpPost]
        public ActionResult IncluirBeneficiarioTemp(BeneficiarioModel beneficiarioModel)
        {
            if (beneficiarioModel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session[KEY_TEMP_DATA_LISTA_BENEFICIARIOS] == null)
            {
                Session[KEY_TEMP_DATA_LISTA_BENEFICIARIOS] = new List<BeneficiarioModel>();
            }
            List<BeneficiarioModel> beneficiarios = Session[KEY_TEMP_DATA_LISTA_BENEFICIARIOS] as List<BeneficiarioModel>;
            List<string> erros = this.errorsWhenInclusionNewBeneficiario(beneficiarioModel.CPFBeneficiario, beneficiarios);

            if (erros.Count > 0)
            {
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            } else
            {

                beneficiarios.Add(beneficiarioModel);
                return Json(beneficiarioModel);
            }
        }

        private List<string> errorsWhenInclusionNewBeneficiario(string cPFBeneficiario, List<BeneficiarioModel> beneficiarios)
        {
            List<string> erros = new List<string>();
            if (!this.ModelState.IsValid)
            {
                erros = (from item in ModelState.Values
                         from error in item.Errors
                         select error.ErrorMessage).ToList();
            }
            else
            {
                if (cPFBeneficiario != null && beneficiarios.Count > 0 && beneficiarios.Any(b => cPFBeneficiario.Equals(b.CPFBeneficiario)))
                {
                    erros.Add("Cpf já existe");
                }
            }
            return erros;


        }


        [HttpPost]
        public JsonResult Beneficiarios(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 400;
            }
            BoCliente bo = new BoCliente();
            List<Beneficiario> beneficiarios = bo.PesquisaBeneficiariosByClienteId(id.Value);
            List<BeneficiarioModel> retVal = mapToBeneficiarioModel(beneficiarios);
            return Json(new { Result = "OK", Records = retVal, TotalRecordCount = retVal.Count });
        }

        public JsonResult BeneficiariosTemp()
        {
            if (Session[KEY_TEMP_DATA_LISTA_BENEFICIARIOS] == null)
            {
                Session[KEY_TEMP_DATA_LISTA_BENEFICIARIOS] = new List<BeneficiarioModel>();
            }
            List<BeneficiarioModel> beneficiarios = Session[KEY_TEMP_DATA_LISTA_BENEFICIARIOS] as List<BeneficiarioModel>;
            return Json(new { Result = "OK", Records = beneficiarios, TotalRecordCount = beneficiarios.Count });
        }
        
        [HttpPost]
        public JsonResult IncluirBeneficiario(long? idCliente, BeneficiarioModel beneficiarioModel)
        {
            if (idCliente == null)
            {
                Response.StatusCode = 400;
            }

            List<string> erros = this.errorNewBeneficiario(beneficiarioModel.CPFBeneficiario);

            if (erros.Count > 0)
            {
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            BoCliente bo = new BoCliente();
            Beneficiario beneficiario = new Beneficiario { 
                CPF = beneficiarioModel.CPFBeneficiario,
                Nome = beneficiarioModel.NomeBeneficiario,
                IdCliente = idCliente.Value
            };
            long id = bo.IncluirNovoBeneficiario(beneficiario);
            beneficiario.Id = id;
            return Json(beneficiarioModel);
        }

        [HttpPost]
        public JsonResult RemoverBeneficiario(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 400;
            }
            BoCliente bo = new BoCliente();
            bo.ExcluirBeneficiario(id.Value);
            return Json(new { Result = "OK"  });
        }

        [HttpPost]
        public JsonResult RemoverBeneficiarioTemp(int id)
        {
            if (Session[KEY_TEMP_DATA_LISTA_BENEFICIARIOS] == null)
            {
                Session[KEY_TEMP_DATA_LISTA_BENEFICIARIOS] = new List<BeneficiarioModel>();
            }
            List<BeneficiarioModel> beneficiarios = Session[KEY_TEMP_DATA_LISTA_BENEFICIARIOS] as List<BeneficiarioModel>;
           if (beneficiarios.Count -1 >= id)
            {
                beneficiarios.RemoveAt(id);
            }
            return Json(beneficiarios);
        }

        private List<BeneficiarioModel> mapToBeneficiarioModel(List<Beneficiario> listDto)
        {
            List<BeneficiarioModel> retVal = new List<BeneficiarioModel>();
            foreach (var item in listDto)
            {
                retVal.Add(new BeneficiarioModel { 
                    CPFBeneficiario=item.CPF,
                    NomeBeneficiario=item.Nome,
                    IdCliente=item.IdCliente,
                    Id=item.Id
                });
            }

            return retVal;
        }

        private List<Beneficiario> mapModelBeneficiario(List<BeneficiarioModel> listDto)
        {
            List<Beneficiario> retVal = new List<Beneficiario>();
            foreach (var item in listDto)
            {
                retVal.Add(new Beneficiario
                {
                    CPF = item.CPFBeneficiario,
                    Nome = item.NomeBeneficiario,
                    IdCliente = item.IdCliente,
                    Id = item.Id
                });
            }

            return retVal;
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    CPF = cliente.CPF,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone
                };

            
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        private List<string> errorsWhenInclusionNewClient(string cpf)
        {
            List<string> erros = new List<string>();
            if (!this.ModelState.IsValid)
            {
                erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();
            } else
            {
                var existCpfOnDb = new BoCliente().VerificarExistencia(cpf);
                if (existCpfOnDb)
                {
                    erros.Add("Cpf já existe");
                }
            }
            return erros;
        }

        private List<String> errorNewBeneficiario(string cpfBenecifiao)
        {
            List<string> erros = new List<string>();
            if (!this.ModelState.IsValid)
            {
                erros = (from item in ModelState.Values
                         from error in item.Errors
                         select error.ErrorMessage).ToList();
            }
            else
            {
                var existCpfOnDb = new BoCliente().VerificarExistenciaBeneficiario(cpfBenecifiao);
                if (existCpfOnDb)
                {
                    erros.Add("Cpf já existe");
                }
            }

            return erros;
        }
    }
}