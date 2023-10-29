using FluentValidation;
using FluentValidation.Results;
using System.Net.NetworkInformation;

namespace NerdStore.Vendas.Domain
{
    public class Voucher
    {
        public string Codigo { get; private set; } = string.Empty;
        public decimal? ValorDesconto { get; private set; }
        public decimal? PercentualDesconto { get; private set; }
        public TipoDescontoVoucher TipoDescontoVoucher { get; private set; }
        public int Quantidade { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Utilizado { get; private set; }

        public Voucher(string codigo, decimal? valorDesconto, decimal? percentualDesconto, TipoDescontoVoucher tipoDescontoVoucher,
            int quantidade, DateTime dataValidade, bool ativo, bool utilizado)
        {
            Codigo = codigo;
            ValorDesconto = valorDesconto;
            PercentualDesconto = percentualDesconto;
            TipoDescontoVoucher = tipoDescontoVoucher;
            Quantidade = quantidade;
            DataValidade = dataValidade;
            Ativo = ativo;
            Utilizado = utilizado;
        }

        public ValidationResult ValidarSeAplicavel()
        {
            return new VoucherAplicavelValidation().Validate(this);
        }
    }

    public class VoucherAplicavelValidation : AbstractValidator<Voucher>
    {
        public static string CodigoErroMsg => "Voucher sem código válido";
        public static string DataValidadeErroMsg => "Este voucher está expirado;";
        public static string AtivoErroMsg => "Este voucher não é mais válido";
        public static string UtilizadoErroMsg => "Este voucher já foi utilizado";
        public static string QuantidadeErroMsg => "Este voucher não está mais disponível";
        public static string ValorDescontoErroMsg => "O valor do desconto precisa ser superior a 0";
        public static string PercentualDescontoErroMsg => "O valor da porcentagem de desconto precisa ser superior a 0";

        public VoucherAplicavelValidation()
        {
            RuleFor(c => c.Codigo)
                .NotEmpty()
                .WithMessage(CodigoErroMsg);

            RuleFor(c => c.DataValidade)
                .Must(DataVencimetnoSuperiorAtual)
                .WithMessage(DataValidadeErroMsg);

            RuleFor(c => c.Ativo)
                .Equal(true)
                .WithMessage(AtivoErroMsg);

            RuleFor(c => c.Utilizado)
                .Equal(false)
                .WithMessage(UtilizadoErroMsg);

            RuleFor(c => c.Quantidade)
                .GreaterThan(0)
                .WithMessage(QuantidadeErroMsg);

            When(f => f.TipoDescontoVoucher == TipoDescontoVoucher.Valor, () =>
            {
                RuleFor(c => c.ValorDesconto)
                .NotNull()
                .WithMessage(ValorDescontoErroMsg)
                .GreaterThan(0)
                .WithMessage(ValorDescontoErroMsg);
            });

            When(f => f.TipoDescontoVoucher == TipoDescontoVoucher.Porcentagem, () =>
            {
                RuleFor(c => c.PercentualDesconto)
                .NotNull()
                .WithMessage(PercentualDescontoErroMsg)
                .GreaterThan(0)
                .WithMessage(PercentualDescontoErroMsg);
            });
        }

        protected static bool DataVencimetnoSuperiorAtual(DateTime dataValidade)
        {
            return dataValidade >= DateTime.Now;
        }
    }
}
