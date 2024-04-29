using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WH_App
{
    public class Stockpile
    {
        public int id { get; set; }
        public int section_id { get; set; }

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

    public class Section
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class WarehouseData : DbContext
    {
        public WarehouseData(string dbName) : base(dbName) { }
        public DbSet<Stockpile> Stockpiles { get; set; }
        public DbSet<Section> Sections { get; set; }
    }
}
