﻿using ErrorOr;
using FeirasLivres.Domain.Common;
using FeirasLivres.Domain.Entities.FeiraEntity.AddNewFeiraUseCase;

namespace FeirasLivres.Domain.Entities.FeiraEntity.FindFeiraUseCase;

public sealed class FindFeiraErrors
{
    private static string className = nameof(AddNewFeiraErrors).Replace("Errors", "");

    public static Error DistritoNotFound() => ErrorHelpers.GetError(ErrorType.NotFound, "The related Distrito informed was not found"/*, className, "DistritoNotFound"*/);
}