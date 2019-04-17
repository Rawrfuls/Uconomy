using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fr34kyn01535.Uconomy.Models
{
    [Table("economy")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "varchar(32)")]
        public string Id { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal Balance { get; set; }

        public DateTime LastUpdated { get; set; }

        public Account(string id, decimal balance)
        {
            Id = id;
            Balance = balance;
            LastUpdated = DateTime.Now;
        }
    }
}
