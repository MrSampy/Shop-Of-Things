using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Data.Data;

namespace ShopOfThings.Tests.UnitTestHelpers
{
    public class ConnectionFactory : IDisposable
    {
        private bool disposedValue = false; // To detect redundant calls


        public ShopOfThingsDBContext CreateContextForInMemory()
        {
            var option = new DbContextOptionsBuilder<ShopOfThingsDBContext>().UseInMemoryDatabase(databaseName: "Test_Database").Options;

            var context = new ShopOfThingsDBContext(option);
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context!;
        }

        public ShopOfThingsDBContext CreateContextForSQLite()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var option = new DbContextOptionsBuilder<ShopOfThingsDBContext>().UseSqlite(connection).Options;

            var context = new ShopOfThingsDBContext(option);

            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context!;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }

    }
}
