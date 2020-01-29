using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PW.DataTransferObjects.Transactions
{
    public class CreateTransactionDto
    {
        [Required]
        public string RecipientEmail { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be between 1 and {int.MaxValue}")]
        public int Amount { get; set; }
    }
}
