using FeirasLivres.Domain.Entities.Common;
using FeirasLivres.Domain.Entities.FeiraEntity.FindFeiraUseCase;
using FeirasLivres.Domain.Misc;

namespace FeirasLivres.Domain.Test.UseCases.FeiraEntity;

public class FindFeiraUseCaseTest
{
    private FindFeira _testTarget;
    private FindFeiraParams _useCaseParamObj = new FindFeiraParams(
        Nome       : "PIRASSUNUNGA",
        Bairro     : "MOOCA",
        CodDistrito: "01",
        Regiao5    : "Leste");

    public FindFeiraUseCaseTest(FindFeira findFeira) => _testTarget = findFeira;

    [Fact]
    public async Task MustReturnErrorWhenTryingToToFindWithoutAnyInformation()
    {
        var useCaseParamObj = new FindFeiraParams(null, null, null, null);

        var findFeiraResult = await _testTarget.Execute(useCaseParamObj);

        Assert.True(findFeiraResult.HasErrors());
    }

    [Fact]
    public async Task MustReturnEmptyListWhenDontFoundFeirasByFiltersParams()
    {
        var findFeiraResult = await _testTarget.Execute(_useCaseParamObj with { Regiao5 = "Centro" });

        Assert.True(findFeiraResult.IsSuccess() && findFeiraResult.Value is not null && findFeiraResult.Value.None());
    }
}