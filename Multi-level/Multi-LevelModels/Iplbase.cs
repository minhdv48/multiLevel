using BaseRepo.Interfaces;
using BaseRepo.Repositories;
using Microsoft.Extensions.Configuration;
using Multi_LevelModels.Implements;
using Multi_LevelModels.Interfaces;
using Multi_LevelModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi_LevelModels
{
    public class Iplbase : Ibase
    {
        private MultiLevelContext _context;
        public IConfiguration _Configuration { get; }
        private Func<EnCache, ICacheService> _cacheService;
        public Iplbase(MultiLevelContext Context, IConfiguration config)
        {
            _context = Context;
            _Configuration = config;
        }

        public IAccount accountRepo => new IplAccount(_context, _Configuration);

        public IInvoice invoiceRepo => new IplInvoice(_context, _Configuration);

        public IProfile profileRepo => new IplProfile(_context, _Configuration);

        public ISetting settingRepo => new IplSetting(_context, _Configuration);

        public ITreeLevel treeLevelRepo => new IplTreeLevel(_context, _Configuration);
        public IHistoryPay historyPayRepo => new IplHistoryPay(_context, _Configuration);
        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
