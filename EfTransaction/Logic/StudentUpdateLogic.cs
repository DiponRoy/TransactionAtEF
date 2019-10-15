using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using EfTransaction.Db;
using EfTransaction.Model;

namespace EfTransaction.Logic
{
    public class StudentUpdateLogic
    {
        public const int CommitSize = 2;           /*must increase in prod*/
        public const string UpdatePart = "-updated";
        public IUmsDbContext DbContext;

        public StudentUpdateLogic(IUmsDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public void UpdateAndSave(List<Student> students)
        {
            using (DbContextTransaction transaction = DbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                int commitCount = 0;
                try
                {
                    foreach (Student student in students)
                    {
                        ++commitCount;
                        student.Name = student.Name + UpdatePart;
                        DbContext.Entry(student).State = EntityState.Modified;
                        DbContext.SaveChanges();
                        if (commitCount % CommitSize == 0)
                        {
                            commitCount = 0;
                            var newDbContext = new UmsDbContext(DbContext.Database.Connection, false);
                            newDbContext.Database.UseTransaction(DbContext.Database.CurrentTransaction.UnderlyingTransaction);
                            DbContext = newDbContext;
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new TransactionException("Error to update all student", exception);
                }
            }
        }
    }
}
