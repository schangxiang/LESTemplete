using LES.Core.IRepository.Base;
using LES.Core.IServices;
using LES.Core.Model.Models;
using LES.Core.Services.BASE;

namespace LES.Core.Services
{
    public partial class PasswordLibServices : BaseServices<PasswordLib>, IPasswordLibServices
    {
        IBaseRepository<PasswordLib> _dal;
        public PasswordLibServices(IBaseRepository<PasswordLib> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

    }
}
