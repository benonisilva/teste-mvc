using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente, List<DML.Beneficiario> beneficiarios)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            long idCliente = cli.Incluir(cliente);
            if (beneficiarios != null)
            {
                foreach (var item in beneficiarios)
                {
                    item.IdCliente = idCliente;
                    cli.IncluirNovoBeneficiario(item);
                }
            }
            return idCliente;
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Alterar(cliente);
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        public void ExcluirBeneficiario(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.ExcluirBeneficiario(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm,  quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF);
        }

        public List<DML.Beneficiario> PesquisaBeneficiariosByClienteId(long id)
        {
            List<DML.Beneficiario> retVal = new List<DML.Beneficiario>();
            DAL.DaoCliente cli = new DAL.DaoCliente();
            retVal = cli.ListarBeneficiariosByIdCliente(id);
            return retVal;
        }

        public long IncluirNovoBeneficiario(DML.Beneficiario beneficiario)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.IncluirNovoBeneficiario(beneficiario);
        }

        public bool VerificarExistenciaBeneficiario(string cpfBenecifiao)
        {
            // TODO 
            return false;
        }
    }
}
