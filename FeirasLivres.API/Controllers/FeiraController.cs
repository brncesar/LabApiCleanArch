using FeirasLivres.Api.Controllers;
using FeirasLivres.Domain.Entities.Common;
using FeirasLivres.Domain.Entities.FeiraEntity;
using FeirasLivres.Domain.Entities.FeiraEntity.AddNewFeiraUseCase;
using FeirasLivres.Domain.Entities.FeiraEntity.EditExistingFeiraUseCase;
using FeirasLivres.Domain.Entities.FeiraEntity.FindFeiraUseCase;
using FeirasLivres.Domain.Entities.FeiraEntity.RemoveExistingFeiraUseCase;
using Microsoft.AspNetCore.Mvc;

namespace Feira.Api.Controllers
{
    public class FeiraController : BaseController
    {
        private readonly ILogger<FeiraController> _logger;
        private readonly IFeiraRepository         _feiraRepository;
        private readonly FindFeira                _findFeiraUseCase;
        private readonly AddNewFeira              _addNewFeiraUseCase;
        private readonly EditExistingFeira        _editFeiraUseCase;
        private readonly RemoveExistingFeira      _removeFeiraUseCase;

        public FeiraController(
            ILogger<FeiraController> logger,
            IFeiraRepository         feiraRepository,
            FindFeira                findFeiraUseCase,
            AddNewFeira              addNewFeiraUseCase,
            EditExistingFeira        editFeiraUseCase,
            RemoveExistingFeira      removeFeiraUseCase)
        {
            _logger             = logger;
            _feiraRepository    = feiraRepository;
            _findFeiraUseCase   = findFeiraUseCase;
            _addNewFeiraUseCase = addNewFeiraUseCase;
            _editFeiraUseCase   = editFeiraUseCase;
            _removeFeiraUseCase = removeFeiraUseCase;
        }

        [HttpPost("Find")]
        public async Task<ActionResult> Find(FindFeiraParams findParams)
        {
            var findResult = await _findFeiraUseCase.Execute(findParams);

            return findResult.IsSuccess()
                ? Ok(findResult.Value)
                : Error(findResult);
        }
    }
}