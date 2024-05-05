using System;
using System.Collections.Generic;
using WH_App;
namespace DataManagement
{
    public class Program
    {
        static void Main(string[] args)
        {
            WarehouseData db = new WarehouseData("WarehouseDB_v3");

            using (db)
            {
                //  Create stockpiles
                Stockpile stockpile1 = new Stockpile();
                Stockpile stockpile2 = new Stockpile();
                Stockpile stockpile3 = new Stockpile();
                //  Create Products
                ProductInfo product1 = new ProductInfo() { id = 1, name = "First Product", description = "First Product is a versatile consumer electronic device designed to streamline everyday tasks. It boasts a sleek design, intuitive user interface, and advanced features such as voice recognition and seamless connectivity with other devices. Whether you're managing your schedule, controlling smart home appliances, or enjoying multimedia content, First Product enhances convenience and productivity. Its compact size and long-lasting battery make it perfect for on-the-go use. With First Product, experience a new level of efficiency in your daily routine." };
                ProductInfo product2 = new ProductInfo() { id = 2, name = "Second Product", description = "Second Product is an innovative wearable device designed to enhance health and wellness. Featuring cutting-edge sensors and intelligent algorithms, Second Product accurately tracks various health metrics such as heart rate, sleep patterns, and physical activity levels. Its lightweight and ergonomic design ensure comfort during all-day wear. Second Product seamlessly integrates with a dedicated mobile app, providing personalized insights and actionable recommendations to help users achieve their fitness goals. With Second Product, prioritize your well-being and stay motivated on your journey to a healthier lifestyle." };
                ProductInfo product3 = new ProductInfo() { id = 3, name = "Third Product", description = "Third Product revolutionizes home entertainment with its immersive features and stunning visuals. This state-of-the-art multimedia system delivers crystal-clear audio and vibrant 4K resolution, bringing your favorite movies, games, and TV shows to life like never before. With an intuitive interface and seamless connectivity options, Third Product ensures effortless access to a vast library of content from various streaming platforms. Its sleek design and customizable settings cater to the preferences of every user, whether you're a casual viewer or a dedicated cinephile. Elevate your home entertainment experience with Third Product and enjoy endless hours of entertainment in the comfort of your own space." };
                //  Create product quantities
                ProductQuantity productQTY1 = new ProductQuantity() { product_id = 1, quantity = 50, expiry_date = new DateTime(2024,3, 2), owning_stockpile = 1 };
                ProductQuantity productQTY2 = new ProductQuantity() { product_id = 2, quantity = 90, expiry_date = new DateTime(2024, 6, 10), owning_stockpile = 3 };
                ProductQuantity productQTY3 = new ProductQuantity() { product_id = 3, quantity = 40, expiry_date = new DateTime(2024, 1, 10), owning_stockpile = 2 };
                ProductQuantity productQTY4 = new ProductQuantity() { product_id = 1, quantity = 160, expiry_date = new DateTime(2024, 12, 1), owning_stockpile = 1 };
                //  Create sections
                Section section1 = new Section() { area_id = 1 };
                Section section2 = new Section() { area_id = 1 };
                Section section3 = new Section() { area_id = 2 };
                Section section4 = new Section() { area_id = 2 };
                //  Create Areas
                Area area1 = new Area() { id = 1, name = "A" };
                Area area2 = new Area() { id = 2, name = "B" };
                //  Assign section id to stockpiles
                stockpile1.section_id = 1;
                stockpile2.section_id = 2;
                stockpile3.section_id = 3;
                //  Assign product lists to stockpiles
                stockpile1.products = new List<ProductQuantity>() { productQTY1, productQTY4 };
                stockpile2.products = new List<ProductQuantity>() { productQTY3 };
                stockpile3.products = new List<ProductQuantity>() { productQTY2 };
                //  Add to database
                db.Areas.AddRange(new Area[] { area1, area2 });
                db.ProductInfos.AddRange(new ProductInfo[] { product1, product2, product3 });
                db.ProductQuantities.AddRange(new ProductQuantity[] { productQTY1, productQTY2, productQTY3 });
                db.Stockpiles.AddRange(new Stockpile[] { stockpile1, stockpile2, stockpile3});
                db.Sections.AddRange(new Section[] { section1, section2, section3, section4 });
                Console.WriteLine("Added To DB, Saving Changes");
                db.SaveChanges();
                Console.WriteLine("Saved Changes, Complete");
            }
        }
    }
}
