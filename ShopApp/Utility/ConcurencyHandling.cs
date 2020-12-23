using ShopApp.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace ShopApp.Utility
{
    public class ConcurencyHandling
    {
        public static void SaveChangesWithConcurencyHandling(ShopContext db)
        {
            bool saved = false;
            while (!saved)
            {
                try
                {
                    db.SaveChanges();
                    saved = true;
                }
                catch (DbUpdateException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.PropertyNames)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            proposedValues[property] = proposedValue;
                        }

                        // Refresh original values to bypass next concurrency check
                        entry.OriginalValues.SetValues(databaseValues);
                    }
                }
            }
        }
    }
}