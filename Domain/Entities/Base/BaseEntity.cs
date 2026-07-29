using FluentResults;

namespace Domain.Entities.Base
{
    /// <summary>
    /// Classe base para todas as entidades do domínio.
    /// Responsável por garantir a geração padronizada do identificador e exigir a implementação das regras de validação da entidade.
    /// 
    /// O ID da entidades será gerado com Guid na Versão 7. Essa versão possui um campo vinculado a Timestamp, possibilitando o Guid ser ordenado. Ajudando assim na indexação
    /// na base de dados.
    /// </summary>
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }

        public BaseEntity()
        {
            Id = Guid.CreateVersion7();
        }

        /// <summary>
        /// Contrato garantindo que toda entidade deve implementar um método de validação.
        /// Nenhuma entidade deve ser instanciada sem estar correta.
        /// 
        /// Vamos utilizar Result Pattern erros mapeados.
        /// </summary>
        /// <returns></returns>
        protected abstract Result Validate();
    }
}
