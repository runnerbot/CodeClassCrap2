using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

// Run each of the following CLI Commands:
//   dotnet restore
//   dotnet build
//   dotnet ef migrations add FirstSwampMigration
//   dotnet ef database update
//   dotnet run

namespace ConsoleApp.SQLite
{
    public class Program
    {
        public static void Main()
        {
            // Create a couple frogs (attaching the bugs they ate for dinner).
            var Kermit = new Frog
            {
                Name = "Kermit",
                Colors = ColorFlags.Green,
                SweetHeart = "Miss Piggy",
                Dinner = new List<Bug> {
                    new Bug { BugType = BugType.Fly, Name = "Marsh" },
                    new Bug { BugType = BugType.Beetle, Name = "Leaf" }
                }
            };

            var Naveen = new Frog
            {
                Name = "Prince Naveen of Maldonia",
                Colors = ColorFlags.Green | ColorFlags.White | ColorFlags.Brown,
                SweetHeart = "Tiana",
                Dinner = new List<Bug> {
                    new Bug { BugType = BugType.Fly, Name = "Tsetse" },
                    new Bug { BugType = BugType.Fly, Name = "Horse" }
                }
            };

            var Treefrog = new Frog
            {
                Name = "Treefrog",
                Colors = ColorFlags.Green | ColorFlags.White | ColorFlags.Pink,
                // No SweetHeart
                // Haven't yet had dinner.
            };

            Console.WriteLine("Swamp Love. . .");
            using (var db = new SwampLoveContext())
            {
                db.Frogs.Add(Kermit);
                db.Frogs.Add(Naveen);
                db.Frogs.Add(Treefrog);
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                var numberOfFrogs = db.Frogs.CountAsync().Result;
                Console.WriteLine("{0} frogs in database:", numberOfFrogs);
                Console.WriteLine();
                // See: https://docs.efproject.net/en/latest/querying/related-data.html
                //foreach (var frog in db.Frogs)
                foreach (var frog in db.Frogs.Include(frog => frog.Dinner))
                {
                    Console.WriteLine(" Frog Id: {0}", frog.FrogId);
                    Console.WriteLine(" Frog Name: {0}", frog.Name);
                    Console.WriteLine(" It's not easy being: {0}", frog.Colors);
                    if (frog.SweetHeart != null)
                        Console.WriteLine(" Frog's SweetHeart: {0}", frog.SweetHeart);
                    else
                        Console.WriteLine(" This frog is alone.");

                    // If the frog had dinner, navigate down into the children (here a list of bugs).
                    var numberOfBugsForDinner = frog.Dinner == null
                     ? 0
                     : frog.Dinner.Count;
                    if (numberOfBugsForDinner > 0)
                    {
                        if (numberOfBugsForDinner == 1)
                            Console.WriteLine(" {0} had 1 bug for dinner: ", frog.Name);
                        else
                            Console.WriteLine(" {0} had {1} bug for dinner: ", frog.Name, numberOfBugsForDinner);
                        foreach (var bug in frog.Dinner)
                        {
                            Console.WriteLine("    Bug Id: {0}", bug.BugId);
                            Console.WriteLine("    Bug Type: {0}", bug.BugType);
                            Console.WriteLine("    Bug Name: {0}", bug.Name);
                            // Notice how you can navigate from the child (here a bug) back up to the parent object (here a frog)
                            Console.WriteLine("    {0} bit me.", bug.Frog.Name);
                        }
                    }
                    else
                    {
                        Console.WriteLine(" {0} didn't have any bugs for dinner.", frog.Name);
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
