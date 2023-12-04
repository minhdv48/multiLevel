using Multi_LevelModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi_LevelModels
{
    public interface Ibase
    {
        IAccount accountRepo { get; }
        IInvoice invoiceRepo { get; }
        IProfile profileRepo { get; }
        ISetting settingRepo { get; }
        ITreeLevel treeLevelRepo { get; }
        IHistoryPay historyPayRepo { get; }
        void Commit();
    }
}
