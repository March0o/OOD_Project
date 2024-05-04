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

                Product product1 = new Product() { id = 1, name = "First Product"};
                Product product2 = new Product() { id = 2, name = "First Product" };
                Product product3 = new Product() { id = 3, name = "First Product" };

                Section section1 = new Section();
                Section section2 = new Section();
                Section section3 = new Section();
                Section section4 = new Section();

                stockpile1.section_id = 1;
                stockpile2.section_id = 2;
                stockpile3.section_id = 3;

                List<Product> list1 = AddProductToList(80, product3);
                List<Product> list2 = AddProductToList(50, product1);
                List<Product> list3 = AddProductToList(60, product2);

                stockpile1.products = list1;
                stockpile2.products = list2;
                stockpile3.products = list3;

                db.Products.AddRange(new Product[] { product1, product2, product3 });
                db.Stockpiles.AddRange(new Stockpile[] { stockpile1, stockpile2, stockpile3});
                db.Sections.AddRange(new Section[] { section1, section2, section3, section4 });
                Console.WriteLine("Added To DB, Saving Changes");
                db.SaveChanges();
                Console.WriteLine("Saved Changes, Complete");
            }
        }

        public static List<Product> AddProductToList(int quantity, Product product)
        {
            List<Product> list = new List<Product>();
            for (int i = 0; i < quantity; i++)
            { list.Add(product); }
            return list;
        }
    }
}
