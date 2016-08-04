using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.SQLite
{
    public class SwampLoveContext : DbContext
    {
        public DbSet<Frog> Frogs { get; set; }
        public DbSet<Bug> Bugs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./swamplove.db");
            //optionsBuilder.UseSqlServer(@"Data Source=.\;Initial Catalog=SwampLove;Integrated Security=True;");
        }
    }


    public interface ISwampAnimal
    {
        string Name { get; set; }
    }


    [Flags]
    public enum ColorFlags
    {
        Unknown = 0,
        Green = 1,
        Brown = 2,
        White = 4,
        Pink = 8
    }

    public class Frog : ISwampAnimal
    {
        public int FrogId { get; set; }

        [Required]
        public ColorFlags Colors { get; set; }

        [Required]
        public string Name { get; set; }

        public string SweetHeart { get; set; }

        // The bugs I had for dinner
        public List<Bug> Dinner { get; set; }
    }

    public enum BugType
    {
        Unknown = 0,
        Fly,
        Spider,
        Beetle
    }

    public class Bug : ISwampAnimal
    {
        public int BugId { get; set; }
        [Required]
        public BugType BugType { get; set; }
        public string Name { get; set; }

        // The frog who had me for dinner
        public int FrogId { get; set; }
        public Frog Frog { get; set; }
    }
}
