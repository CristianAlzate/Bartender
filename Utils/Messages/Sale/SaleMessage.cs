using BartenderApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BartenderApp.Utils.Messages.Sale
{
    public class SaleMessage
    {
        public bool IdAdd { get; set; }
        public SaleDTO SaleDto { get; set; }
        public SaleDetailDTO SaleDetailDto { get; set; }
    }
}
