using System;
using MySql.Data.Types;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace APInancy
{
    public class ClientPostData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }


    public class FacturePostData
    {
        public int? Client_id { get; set; }
        public string FactureDate { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
    }
}
