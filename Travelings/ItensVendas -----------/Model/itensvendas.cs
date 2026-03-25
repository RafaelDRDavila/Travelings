using Microsoft.EntityFrameworkCore.Query;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace Travelings.Model
{
    [Table("itensvendas")]
    public class itensvendas
    {
        [Key]

        public int id { get; set; }
        public int idvenda { get; set; }
        public int idproduto { get; set; }
        public int quantidade { get; set; }
        public int precounitario { get; set; }



        public itensvendas(int id, int idvenda, int idproduto, int quantidade, int precounitario)
        {
            this.id = id;
            this.idvenda = idvenda;
            this.idproduto = idproduto;
            this.quantidade = quantidade;
            this.precounitario = precounitario;
        }


    }
}

