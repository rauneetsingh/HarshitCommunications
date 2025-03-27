using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarshitCommunications.Models
{
    public class PaytmConfig
    {
        public string MerchantId { get; set; }
        public string MerchantKey { get; set; }
        public string Website { get; set; }
        public string IndustryTypeId { get; set; }
        public string ChannelId { get; set; }
        public string CallbackUrl { get; set; }
    }

}
