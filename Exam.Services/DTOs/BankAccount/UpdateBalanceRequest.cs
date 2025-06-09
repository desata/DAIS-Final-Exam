using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Services.DTOs.BankAccount
{
    public class UpdateBalanceRequest
    {
        public int BankAccountId { get; set; }
        public decimal NewBalance { get; set; }


    }
}
