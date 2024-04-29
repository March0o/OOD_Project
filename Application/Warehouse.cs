using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WH_Application
{
        public class Stockpile
        {
            public int stockpile_id { get; set; }
            public int section_id { get; set; }
            public virtual Section assigned_section {  get; set; }
        }
        public class Section
        {
            public int section_id { get; set; }
            public int stockpile_id { get; set; }
            public virtual Stockpile assigned_stockpile { get; set; }
        }

        public class WarehouseData : DbContext
        {
            public WarehouseData(string dbName) : base(dbName) { }
            public DbSet<Stockpile> Stockpiles { get; set; }
            public DbSet<Section> Sections { get; set; }
        }
}
