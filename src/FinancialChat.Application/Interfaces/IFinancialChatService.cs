using FinancialChat.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Application.Interfaces
{
    public interface IFinancialChatService
    {
        Task SendRequestStockByCode(Message message);
        Task SendResponseStockByCode(Message message);
    }
}
