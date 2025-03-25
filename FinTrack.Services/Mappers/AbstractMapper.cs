using FinTrack.Services.Mappers.Contracts;
using System.Collections;

namespace FinTrack.Services.Mappers
{
    public abstract class AbstractMapper<S, T> : IMapper<S, T>
       where S : class
       where T : class
    {
        public virtual T MapToDto(S source, Action<S, T> extra = null)
        {
            return MapToDto<T>(source, extra);
        }

        public virtual S MapFromDto(T model, Action<T, S> extra = null, S source = null)
        {
            return MapFromDto<S>(model, extra, source);
        }

        public virtual IEnumerable<T> MapCollectionToDto(IEnumerable<S> source, Action<S, T> extra = null)
        {
            return source.Select(x => MapToDto<T>(x, extra)).ToList();
        }

        public virtual TItem MapToDto<TItem>(S source, Action<S, TItem> extra = null, TItem destination = null)
            where TItem : class, T
        {
            TItem result = null;

            if (destination == null)
            {
                result = Mapper.Map<TItem>(source);
            }
            else
            {
                result = Mapper.Map<S, TItem>(source, destination);
            }

            extra?.Invoke(source, result);

            return result;
        }

        public virtual async Task<TItem> MapToDtoAsync<TItem>(S source, Func<S, TItem, Task> extra = null, TItem destination = null)
            where TItem : class, T
        {
            TItem result = null;

            if (destination == null)
            {
                result = Mapper.Map<TItem>(source);
            }
            else
            {
                result = Mapper.Map<S, TItem>(source, destination);
            }

            await extra?.Invoke(source, result);

            return result;
        }

        public virtual S MapFromDto<TItem>(T source, Action<T, TItem> extra = null, TItem destination = null)
            where TItem : class, S
        {
            TItem result = null;

            if (destination == null)
            {
                result = Mapper.Map<TItem>(source);
            }
            else
            {
                result = Mapper.Map<T, TItem>(source, destination);
            }

            extra?.Invoke(source, result);

            return result;
        }

        public virtual IEnumerable<TItem> MapCollectionToDto<TItem>(IEnumerable<S> source, Action<S, TItem> extra = null)
            where TItem : class, T
        {
            return source.Select(x => MapToDto<TItem>(x, extra)).ToList();
        }

        public virtual IEnumerable<S> MapCollectionFromDto<TItem>(IEnumerable<T> model, Action<T, TItem> extra = null, TItem source = null)
            where TItem : class, S
        {
            return model.Select(x => MapFromDto<TItem>(x, extra)).ToList();
        }

        protected abstract AutoMapper.IMapper Configure();

        public IEnumerable<T> MapCollectionToDto(IEnumerable source, Action<S, T> extra = null)
        {
            var result = new List<T>();
            foreach (var item in source)
            {
                result.Add(MapToDto<T>((S)item, extra));
            }

            return result;
        }

        protected AutoMapper.IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    _mapper = Configure();
                }

                return _mapper;
            }
        }

        private AutoMapper.IMapper _mapper;
    }
}
