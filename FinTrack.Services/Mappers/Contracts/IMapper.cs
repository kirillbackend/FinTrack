using System.Collections;

namespace FinTrack.Services.Mappers.Contracts
{
    public interface IMapper<TEntity, TDto> : IMapper where TEntity : class where TDto : class
    {
        IEnumerable<TEntity> MapCollectionFromDto<TItem>(IEnumerable<TDto> model, Action<TDto, TItem> extra = null, TItem destination = null) where TItem : class, TEntity;

        IEnumerable<TDto> MapCollectionToDto(IEnumerable<TEntity> source, Action<TEntity, TDto> extra = null);

        IEnumerable<TDto> MapCollectionToDto(IEnumerable source, Action<TEntity, TDto> extra = null);

        IEnumerable<TItem> MapCollectionToDto<TItem>(IEnumerable<TEntity> source, Action<TEntity, TItem> extra = null) where TItem : class, TDto;

        TEntity MapFromDto(TDto model, Action<TDto, TEntity> extra = null, TEntity destination = null);

        TEntity MapFromDto<TItem>(TDto model, Action<TDto, TItem> extra = null, TItem destination = null) where TItem : class, TEntity;

        TDto MapToDto(TEntity source, Action<TEntity, TDto> extra = null);

        TItem MapToDto<TItem>(TEntity source, Action<TEntity, TItem> extra = null, TItem destination = null) where TItem : class, TDto;

        Task<TItem> MapToDtoAsync<TItem>(TEntity source, Func<TEntity, TItem, Task> extra = null, TItem destination = null) where TItem : class, TDto;
    }

    public interface IMapper
    {

    }
}
