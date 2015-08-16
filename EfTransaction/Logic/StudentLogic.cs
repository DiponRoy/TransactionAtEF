using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using EfTransaction.Db;
using EfTransaction.Model;

namespace EfTransaction.Logic
{
    public class StudentLogic
    {
        public readonly IUmsDbContext DbContext;

        public StudentLogic(IUmsDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public void AddAllThenSave(List<Student> students)
        {
            try
            {
                foreach (Student student in students)
                {
                    DbContext.Students.Add(student);
                }
                DbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new TransactionException("Error to AddAllThenSave", exception);
            }
        }

        public void AddAndSave(List<Student> students)
        {
            /*read from web, for detail*/
            using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            //or
            //using (var transaction = new TransactionScope())
            {
                try
                {
                    foreach (Student student in students)
                    {
                        DbContext.Students.Add(student);
                        DbContext.SaveChanges();
                    }
                    transaction.Complete();
                }
                catch (Exception exception)
                {
                    throw new TransactionException("Error to AddAndSave", exception);
                }
            }
        }

        public void RemoveAllStudent()
        {
            List<Student> list = DbContext.Students.OrderBy(x => x.Id).ToList();

            if (DbContext.Database.Connection.State == ConnectionState.Open)
            {
                DbContext.Database.Connection.Close();
            }
            DbContext.Database.Connection.Open();

            /*read from web, for detail*/
            using (DbContextTransaction transaction = DbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            //or
            //using (DbContextTransaction transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (Student student in list)
                    {
                        DbContext.Students.Remove(student);
                        DbContext.SaveChanges();
                    }

                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new TransactionException("Error to RemoveAllStudent", exception);
                }
            }
        }
    }
}
