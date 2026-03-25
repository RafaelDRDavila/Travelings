using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace Travelings.Model
{
    [Table("vendas")]
    public class vendas
    {
        [Key]

        public int id { get; set; }
        public int idcliente { get; set; }
        public int idvendedor { get; set; }
        public DateTime datavenda { get; set; }



        public vendas(int id, int idcliente, int idvendedor, DateTime datavenda)
        {
            this.id = id;
            this.idcliente = idcliente;
            this.idvendedor = idvendedor;
            this.datavenda  = datavenda;
        }

       
    }
}

