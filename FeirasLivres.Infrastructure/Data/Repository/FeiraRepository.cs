﻿using ErrorOr;
using FeirasLivres.Domain.Common;
using FeirasLivres.Domain.Entities.Common;
using FeirasLivres.Domain.Entities.FeiraEntity;
using FeirasLivres.Domain.Entities.FeiraEntity.AddNewFeiraUseCase;
using FeirasLivres.Domain.Entities.FeiraEntity.FindFeiraUseCase;
using FeirasLivres.Domain.Misc;
using FeirasLivres.Infrastructure.Data.DbCtx;
using Microsoft.EntityFrameworkCore;

namespace FeirasLivres.Infrastructure.Data.Repository
{
    public class FeiraRepository : BaseRepository<FeirasLivresDbContext, Feira>, IFeiraRepository
    {
        public FeiraRepository(FeirasLivresDbContext dbCtx) : base(dbCtx) {}

        private async Task<Feira?> GetFeiraByNumeroRegistroAsync(string numeroRegistro)
            => await _dbSet.SingleOrDefaultAsync(x => x.NumeroRegistro == numeroRegistro.Trim());

        public async Task<IDomainActionResult<Feira>> GetByNumeroRegistroAsync(string numeroRegistro)
        {
            var domainActionResult = new DomainActionResult<Feira>();
            try
            {
                var feira = await GetFeiraByNumeroRegistroAsync(numeroRegistro);

                return feira is not null
                    ? domainActionResult.SetValue(feira)
                    : domainActionResult.AddError(ErrorHelpers.GetError(ErrorType.NotFound, "Feira not found"));
            }
            catch (Exception ex)
            {
                return domainActionResult.AddError(ErrorHelpers.GetError(ErrorType.Unexpected, ex.Message));
            }
        }

        public async Task<IDomainActionResult<bool>> RemoveByNumeroRegistroAsync(string numeroRegistro)
        {
            var domainActionResult = new DomainActionResult<bool>();
            try
            {
                var feira = await GetFeiraByNumeroRegistroAsync(numeroRegistro);

                if (feira is null)
                    return domainActionResult.AddError(ErrorHelpers.GetError(ErrorType.NotFound, "Feira not found"));

                _dbSet.Remove(feira);
                await _dbCtx.SaveChangesAsync();

                return domainActionResult.SetValue(true);
            }
            catch (Exception ex)
            {
                return domainActionResult.AddError(ErrorHelpers.GetError(ErrorType.Unexpected, ex.Message));
            }
        }

        public async Task<IDomainActionResult<bool>> UpdateByNumeroRegistroAsync(EditExistingFeiraParams paramFeiraToUpdate)
        {
            var domainActionResult = new DomainActionResult<bool>(false);

            try
            {
                var repositoryFeiraToUpdate = await GetFeiraByNumeroRegistroAsync(paramFeiraToUpdate.NumeroRegistro);

                if (repositoryFeiraToUpdate is null)
                    return domainActionResult.AddError(ErrorHelpers.GetError(ErrorType.NotFound, "Feira not found"));

                paramFeiraToUpdate.MapValuesTo(ref repositoryFeiraToUpdate);
                await _dbCtx.SaveChangesAsync();

                return domainActionResult.SetValue(true);
            }
            catch (Exception ex)
            {
                return domainActionResult.AddError(ErrorHelpers.GetError(ErrorType.Unexpected, ex.Message));
            }
        }

        public async Task<IDomainActionResult<List<FindDistritoResult>>> FindFeirasAsync(FindFeiraParams findParams)
        {
            var domainActionResult = new DomainActionResult<List<FindDistritoResult>>();
            try
            {
                var feirasResult = new List<FindDistritoResult>();
                var listResult = _dbSet.AsQueryable().AsNoTracking();

                if (findParams.Nome.IsNotNullOrNotEmpty())
                    listResult = listResult.Where(db => db.Nome.Contains(findParams.Nome.Trim()));

                if (findParams.Bairro.IsNotNullOrNotEmpty())
                    listResult = listResult.Where(db => db.EnderecoBairro.Contains(findParams.Bairro.Trim()));

                if (findParams.CodDistrito.IsNotNullOrNotEmpty())
                    listResult = listResult.Where(db => db.Distrito.Codigo == findParams.CodDistrito);

                if (findParams.Regiao5 is not null)
                    listResult = listResult.Where(db => db.Regiao5 == findParams.Regiao5);

                var feirasFound = await listResult
                    .Include(f => f.Distrito)
                    .Include(f => f.SubPrefeitura)
                    .ToListAsync();

                feirasFound.ForEach(feiraEntity => feirasResult.Add(new(
                    Nome                : feiraEntity.Nome,
                    NumeroRegistro      : feiraEntity.NumeroRegistro,
                    SetorCensitarioIBGE : feiraEntity.SetorCensitarioIBGE,
                    AreaDePonderacaoIBGE: feiraEntity.AreaDePonderacaoIBGE,
                    CodDistrito         : feiraEntity.Distrito.Codigo,
                    Distrito            : feiraEntity.Distrito.Nome,
                    CodSubPrefeitura    : feiraEntity.SubPrefeitura.Codigo,
                    SubPrefeitura       : feiraEntity.SubPrefeitura.Nome,
                    Regiao5             : feiraEntity.Regiao5.ToDescription(),
                    Regiao8             : feiraEntity.Regiao8.ToDescription(),
                    EnderecoLogradouro  : feiraEntity.EnderecoLogradouro,
                    EnderecoNumero      : feiraEntity.EnderecoNumero,
                    EnderecoBairro      : feiraEntity.EnderecoBairro,
                    EnderecoReferencia  : feiraEntity.EnderecoReferencia,
                    Latitude            : feiraEntity.Latitude,
                    Longitude           : feiraEntity.Longitude)));

                return domainActionResult.SetValue(feirasResult);
            }
            catch (Exception ex)
            {
                return domainActionResult.AddError(ErrorHelpers.GetError(ErrorType.Unexpected, ex.Message));
            }
        }
    }
}
