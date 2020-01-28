using System;
using System.Collections.Generic;
using System.Text;

namespace PW.DataTransferObjects
{
    public class TransactionDto
    {
        public string DateTime { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
    }
}
