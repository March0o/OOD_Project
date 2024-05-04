using System;
using System.Collections.Generic;
using WH_App;
namespace DataManagement
{
    public class Program
    {
        static void Main(string[] args)
        {
            WarehouseData db = new WarehouseData("WarehouseDB_v1");

            using (db)
            {
                Stockpile stockpile1 = new Stockpile();
                Stockpile stockpile2 = new Stockpile();
                Stockpile stockpile3 = new Stockpile();

                ProductQuantity product1 = new ProductQuantity() { product_id = 1, quantity = 50, owning_stockpile = 1 };
                ProductQuantity product2 = new ProductQuantity() { product_id = 2, quantity = 90, owning_stockpile = 3 };
                ProductQuantity product3 = new ProductQuantity() { product_id = 3, quantity = 40, owning_stockpile = 2 };
                ProductQuantity product4 = new ProductQuantity() { product_id = 1, quantity = 160, owning_stockpile = 1 };

                Section section1 = new Section();
                Section section2 = new Section();
                Section section3 = new Section();
                Section section4 = new Section();

                stockpile1.section_id = 1;
                stockpile2.section_id = 2;
                stockpile3.section_id = 3;

                stockpile1.products = new List<ProductQuantity>() { product1, product4 };
                stockpile2.products = new List<ProductQuantity>() { product3 };
                stockpile3.products = new List<ProductQuantity>() { product2 };

                db.ProductQuantities.AddRange(new ProductQuantity[] { product1, product2, product3 });
                db.Stockpiles.AddRange(new Stockpile[] { stockpile1, stockpile2, stockpile3});
                db.Sections.AddRange(new Section[] { section1, section2, section3, section4 });
                Console.WriteLine("Added To DB, Saving Changes");
                db.SaveChanges();
                Console.WriteLine("Saved Changes, Complete");
            }
        }
    }
}
