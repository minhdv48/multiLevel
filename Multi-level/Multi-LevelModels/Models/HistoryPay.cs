using K4os.Compression.LZ4.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi_LevelModels.Models
{
    public class HistoryPay
    {
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public int ReferPay { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? ImgPath { get; set; }
        public DateTime PayDate { get; set; }
        public string FullName { get; set; }
    }
}
