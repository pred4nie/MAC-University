﻿using AutoMapper;
using MAC.Data.UnitOfWork;
using MAC.Services.DataService.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MAC.Services.DataService
{
    public class BaseCrudDataService<TDto, TEntity> : IBaseCrudDataService<TDto,TEntity> where TEntity : class
                                                                                         where TDto : class
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public BaseCrudDataService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<TDto> Get()
        {
            var result = _unitOfWork.GetRepository<TEntity>().GetAll();
            return result.Select(x => _mapper.Map<TDto>(x));
        }

        public TDto GetById(int itemId) 
        {
            var result = _unitOfWork.GetRepository<TEntity>().GetById(itemId);
            return _mapper.Map<TDto>(result);
        }

        public virtual TDto Insert(TDto item)
        {
            TEntity entity = _mapper.Map<TEntity>(item);
            TEntity result = _unitOfWork.GetRepository<TEntity>().Insert(entity);
            _unitOfWork.Save();
            return _mapper.Map<TDto>(result);
        }

        public virtual void Update(TDto item, int itemId)
        {
            TEntity entity = _unitOfWork.GetRepository<TEntity>().GetById(itemId);
            _mapper.Map(item, entity);
            _unitOfWork.GetRepository<TEntity>().Update(entity);
            _unitOfWork.Save();
        }

        public void Delete(int itemId)
        {
            _unitOfWork.GetRepository<TEntity>().Delete(itemId);
            _unitOfWork.Save();
        }
    }
}
