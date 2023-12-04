using BaseRepo.Repositories;
using Microsoft.Extensions.Configuration;
using Multi_LevelModels.Interfaces;
using Multi_LevelModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi_LevelModels.Implements
{
    public class IplHistoryPay: Repository<HistoryPay>, IHistoryPay
    {
        #region Default and Contructor
        public IConfiguration _Configuration { get; }
        internal string _cnnString;
        public MultiLevelContext _context;
        public IplHistoryPay(MultiLevelContext Context, IConfiguration configuration) : base(Context)
        {
            _Configuration = configuration;
            _context = Context;
        }
        #endregion
    }
}
