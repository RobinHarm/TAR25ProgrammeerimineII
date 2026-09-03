using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopTAR25.Data
{
    //teha sellest classist DbContext, et saaks andmebaasi kasutada
    public class ShopTAR25Context : DbContext
    {
        public ShopTAR25Context(DbContextOptions<ShopTAR25Context> options) : base(options)
        { }
        //teha DbSet, et saaks andmebaasi kasutada
        
    }
}
