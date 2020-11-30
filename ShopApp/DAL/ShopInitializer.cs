using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopApp.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Configuration;
using ShopApp.DAL;
using ShopApp.Migrations;

namespace ShopApp.DAL
{
    public class ShopInitializer : System.Data.Entity.MigrateDatabaseToLatestVersion<ShopContext, ShopApp.Migrations.Configuration>
    {

    }
}