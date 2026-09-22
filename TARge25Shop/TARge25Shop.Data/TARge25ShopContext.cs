using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core.Domain;


namespace TARge25Shop.Data
{
    //nimetasime classi TARge25ShopContext, mis pärib DbContext klassi
    public class TARge25ShopContext : DbContext
    {
        //tegime konteksti, mis pärib DbContext klassi
        public TARge25ShopContext(DbContextOptions<TARge25ShopContext> options)
            : base(options) { }


        //vaja lisada dbSet, mis on seotud meie domain klassiga Spaceship
        public DbSet<Spaceship> Spaceships { get; set; }
        public DbSet<FileToApi> FileToApis { get; set; }
    }
}
