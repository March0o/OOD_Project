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

        public List<ProductQuantity> products { get; set; }

        public Stockpile() { 
            products = new List<ProductQuantity>();
        }

        public string ListProducts()
        {
            string list = "";
            for (int i = 0; i < products.Count; i++)
            {
                ProductQuantity product = products[i];
                list += $"{product.quantity}";
            }
            return list;
        }
    }

    public class ProductQuantity
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public int owning_stockpile { get; set; }

        public override string ToString()
        {
            return "ID: " + id + "\tQTY: " + quantity;
        }
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

        public DbSet<ProductQuantity> ProductQuantities { get; set; }
    }
}
