﻿using FeirasLivres.Domain.Entities.Enums;
using FeirasLivres.Domain.Misc;
using FluentValidation;

namespace FeirasLivres.Domain.Entities.FeiraEntity.EditExistingFeiraUseCase;

public class EditExistingFeiraParamsValidator : AbstractValidator<EditExistingFeiraParams>
{
    public EditExistingFeiraParamsValidator()
    {
        RuleFor(p => p.EnderecoNumero      ).MaximumLength(5);
        RuleFor(p => p.EnderecoReferencia  ).MaximumLength(24);
        RuleFor(p => p.AreaDePonderacaoIBGE).Length       (13);
        RuleFor(p => p.Nome                ).Length       (3, 30);
        RuleFor(p => p.EnderecoBairro      ).Length       (2, 30);

        RuleFor(p => p.Regiao5)
            .IsEnumName(typeof(Regiao5), caseSensitive: false)
            .WithMessage(p => $"Regiao5 '{p.Regiao5}' inválida. Os valores possíveis são: {string.Join(", ", Enum.GetValues(typeof(Regiao5)).Cast<Regiao5>())}");

        RuleFor(p => p.Regiao8)
            .IsEnumName(typeof(Regiao8), caseSensitive: false)
            .WithMessage(p => $"Regiao8 '{p.Regiao8}' inválida. Os valores possíveis são: {string.Join(", ", Enum.GetValues(typeof(Regiao8)).Cast<Regiao8>())}");

        RuleFor(p => p.NumeroRegistro).Matches("[0-9]{4}[-][0-9]")
            .WithMessage(p =>
                $"O número do registro da feira deve ser informado no formato ####-#. " +
                $"O valor informado foi: {p.NumeroRegistro}");

        RuleFor(p => p.SetorCensitarioIBGE).Length(15)
            .WithMessage(p =>
                $"O setor censitário deve ser informado com exatamente 15 caracteres. " +
                $"O valor informado foi: {p.SetorCensitarioIBGE} ({p.SetorCensitarioIBGE.Length} caracteres)");

        RuleFor(p => p.EnderecoLogradouro).Length(3, 34)
            .WithMessage(p =>
                $"O logradouro deve ter pelo menos 3 caracteres e no máximo 34. " +
                $"O logradouro informado foi: {p.EnderecoLogradouro}");

        RuleFor(p => p.Latitude).Must(BeAValidLatitude)
            .WithMessage(p =>
                $"Latitude inválida. O valor deve estar compreendido entre -90 e 90." +
                $"O valor informado foi: {p.Latitude}");

        RuleFor(p => p.Longitude).Must(BeAValidLongitude)
            .WithMessage(p =>
                $"Longitude inválida. O valor deve estar compreendido entre -180 e 180." +
                $"O valor informado foi: {p.Longitude}");

        RuleFor(feira => feira.NumeroRegistro).Must((feira, _) =>
            feira.Nome                .IsNotNullOrNotEmpty() ||
            feira.SetorCensitarioIBGE .IsNotNullOrNotEmpty() ||
            feira.AreaDePonderacaoIBGE.IsNotNullOrNotEmpty() ||
            feira.CodDistrito         .IsNotNullOrNotEmpty() ||
            feira.CodSubPrefeitura    .IsNotNullOrNotEmpty() ||
            feira.Regiao5             .IsNotNullOrNotEmpty() ||
            feira.Regiao8             .IsNotNullOrNotEmpty() ||
            feira.EnderecoLogradouro  .IsNotNullOrNotEmpty() ||
            feira.EnderecoBairro      .IsNotNullOrNotEmpty() ||
            feira.EnderecoReferencia  .IsNotNullOrNotEmpty()
        ).WithMessage("É necessário informar pelo menos um parâmetro (além do número de  registro) para atualização da feira.");
    }

    private bool BeAValidLatitude (double? latitude ) => latitude  is null || latitude  is >=  -90 and <=  90;
    private bool BeAValidLongitude(double? longitude) => longitude is null || longitude is >= -180 and <= 180;
}