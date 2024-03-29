﻿namespace FeirasLivres.Domain.Entities.FeiraEntity.Common;

public record FeiraResult(
    string  Nome,
    string  NumeroRegistro,
    string  SetorCensitarioIBGE,
    string  AreaDePonderacaoIBGE,
    string  CodDistrito,
    string  Distrito,
    string  CodSubPrefeitura,
    string  SubPrefeitura,
    string  Regiao5,
    string  Regiao8,
    string  EnderecoLogradouro,
    string? EnderecoNumero,
    string  EnderecoBairro,
    string? EnderecoReferencia,
    double  Latitude,
    double  Longitude);