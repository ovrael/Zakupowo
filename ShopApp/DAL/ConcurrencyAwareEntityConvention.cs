using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ShopApp.DAL
{
    public interface IConcurrencyAwareEntity
    {
        byte[] RowVersion { get; set; }
    }
    public class ConcurrencyAwareEntityConvention : Convention
    {
        public ConcurrencyAwareEntityConvention()
        {
            this.Types<IConcurrencyAwareEntity>().Configure(
                config => config.Property(e => e.RowVersion).IsRowVersion()
                );
        }
    }
}