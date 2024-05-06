using FluentValidation;

namespace FastFoodUserManagement.Application.UseCases.CreateUser;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 255)
            .WithMessage("O nome deve ter no minimo 3 e no máximo 255 caracteres.")
            .NotEmpty()
            .WithMessage("O nome deve estar preenchido.");

        RuleFor(dto => dto.Email)
            .NotEmpty()
            .WithMessage("O e-mail deve estar preenchido.")
            .EmailAddress()
            .WithMessage("O e-mail fornecido não é válido.");

        RuleFor(dto => dto.Identification)
            .NotEmpty()
            .WithMessage("O cpf deve estar preenchido.")
            .Must(BeValidCpf)
            .WithMessage("O cpf deve ser válido");
    }

    public bool BeValidCpf(string cpf)
    {
        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCpf;
        string digito;
        int soma;
        int resto;
        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "");
        if (cpf.Length != 11)
            return false;
        tempCpf = cpf.Substring(0, 9);
        soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;
        digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
        resto = soma % 11;
        if (resto < 2)
            resto = 0;
        else
            resto = 11 - resto;
        digito = digito + resto.ToString();

        return cpf.EndsWith(digito);
    }
}
