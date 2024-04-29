using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace WH_App
{
    public class Stockpile
    {
        public int stockpileId { get; set; }
        public int section_number { get; set; }

        public List<Product> products { get; set; }

        public Stockpile() { 
            products = new List<Product>();
        }
    }

    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
    }


    public class WarehouseData : DbContext
    {
        public WarehouseData(string dbName) : base(dbName) { }
        public DbSet<Stockpile> Stockpiles { get; set; }
    }
}
