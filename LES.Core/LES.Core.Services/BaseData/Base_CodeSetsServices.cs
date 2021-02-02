﻿
using LES.Core.IServices;
using LES.Core.Model.Models;
using LES.Core.Services.BASE;
using LES.Core.IRepository.Base;

namespace LES.Core.Services
{
    public class Base_CodeSetsServices : BaseServices<Base_CodeSets>, IBase_CodeSetsServices
    {
        private readonly IBaseRepository<Base_CodeSets> _dal;
        public Base_CodeSetsServices(IBaseRepository<Base_CodeSets> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}