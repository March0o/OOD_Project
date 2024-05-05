using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.RightsManagement;
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

    public class ProductInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public override string ToString()
        {
            return $"{name} [{id}]";
        }
    }

    public class ProductQuantity
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public DateTime expiry_date { get; set; }
        bool is_expired
        {
            get
            {
                if (expiry_date < DateTime.Now)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public int owning_stockpile { get; set; }

        public override string ToString()
        {
            return "ID: " + product_id + "\tQTY: " + quantity + "\t\tExpiry Date: " + expiry_date + "\tExpired: " + is_expired;
        }
    }

    public class Section
    {
        public int id { get; set; }
        public string name { get; set; }
        public int area_id { get; set; }
    }

    public class Area
    {
        public int id { get; set; }
        public string name { set; get; }
        public override string ToString()
        {
            return "Area ID: " + id + "\tName: " + name;
        }

    }
    public class WarehouseData : DbContext
    {
        public WarehouseData(string dbName) : base(dbName) { }
        public DbSet<Stockpile> Stockpiles { get; set; }
        public DbSet<Section> Sections { get; set; }

        public DbSet<ProductQuantity> ProductQuantities { get; set; }
        public DbSet<ProductInfo> ProductInfos { get; set; }
        public DbSet<Area> Areas { get; set; }
    }
}
