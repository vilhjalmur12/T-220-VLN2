using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext() : base ("CodeEditorApp")       // base hér er NAFN sem notað er í Web.config og VERÐUR að vera eins
        {
            
        }

        /* Við getum hent inn lista af INIT klösum hér fyrir gagnagrunninn
         *      Dæmi:
         *      public DbSet<Computer> Computers { get; set; }  
         */

        //          Klasa INIT byrjar hér


        //          Klasa INIT endar hér

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}