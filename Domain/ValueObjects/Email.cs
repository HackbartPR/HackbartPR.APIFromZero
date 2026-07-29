using Domain.Exceptions;
using FluentResults;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    /// <summary>
    /// Value Object responsável por representar e validar um endereço de e-mail.
    /// Assim como as entidades, todas as regras de negócio pertencentes ao E-mail, devem ser escritas dentro dessa classe.
    /// </summary>
    public sealed class Email
    {
        public string Value { get; set; } = string.Empty;

        public Result Create(string value)
        {
            Value = value;

            return Validate();
        }

        private Result Validate()
        {
            Result result = new();

            if (string.IsNullOrWhiteSpace(Value))
                result.WithError(DomainError.EmptyEmail);

            if (Value.EndsWith('.') || Value.Contains(' '))
                result.WithError(DomainError.InvalidEmail);

            try
            {
                var mailAddress = new MailAddress(Value);
                string normalizedAddress = NormalizeDomain(mailAddress.Address);

                if (!IsValidEmailPattern(normalizedAddress))
                    result.WithError(DomainError.InvalidEmail);

                Value = normalizedAddress;
            }
            catch (FormatException) { result.WithError(DomainError.InvalidEmail); }
            catch (Exception) { result.WithError(DomainError.TimeOutEmailValidation); }

            return result;
        }

        /// <summary>
        /// Converte o domínio para ASCII para garantir compatibilidade com IDN (Internationalized Domain Names)
        /// </summary>
        private static string NormalizeDomain(string email)
        {
            var parts = email.Split('@');
            if (parts.Length != 2)
                throw new FormatException();

            var idn = new IdnMapping();
            string domainName = idn.GetAscii(parts[1]);

            return $"{parts[0]}@{domainName}";
        }

        /// <summary>
        /// Valida se o e-mail segue um padrão correto segundo a RFC 5322.
        /// </summary>
        private static bool IsValidEmailPattern(string email)
        {
            try
            {
                return Regex.IsMatch(email, @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
                                     RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(100));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
