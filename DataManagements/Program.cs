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

                Product product1 = new Product();
                Product product2 = new Product();
                Product product3 = new Product();

                Section section1 = new Section();
                Section section2 = new Section();
                Section section3 = new Section();
                Section section4 = new Section();

                stockpile1.section_id = 1;
                stockpile2.section_id = 2;
                stockpile3.section_id = 3;

                List<Product> list1 = new List<Product>();
                List<Product> list2 = new List<Product>();
                List<Product> list3 = new List<Product>();

                AddProductToList(80, list1, product3);
                AddProductToList(50, list2, product1);
                AddProductToList(60, list3, product3);

                stockpile1.products = list1;
                stockpile2.products = list2;
                stockpile3.products = list3;

                db.Stockpiles.AddRange(new Stockpile[] { stockpile1, stockpile2, stockpile3});
                db.Sections.AddRange(new Section[] { section1, section2, section3, section4 });
                Console.WriteLine("Added To DB, Saving Changes");
                db.SaveChanges();
                Console.WriteLine("Saved Changes, Complete");
            }
        }

        public static void AddProductToList(int quantity, List<Product> list, Product product)
        {
            for (int i = 0; i < quantity; i++)
            { list.Add(product); }
        }
    }
}
