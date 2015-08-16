using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using EfTransaction.Db;
using EfTransaction.Logic;
using EfTransaction.Model;
using NUnit.Framework;

namespace EfTransaction.Test
{
    [TestFixture]
    public class SimpleTransactionTest
    {

        protected IUmsDbContext DbContext { get; set; }
        protected StudentLogic Logic { get; set; }

        [SetUp]
        public void SetUp()
        {
            var db = new UmsDbBuildContext();
            db.Load();

            DbContext = db;
            Logic = new StudentLogic(DbContext);
        }

        [TearDown]
        public void TearDown()
        {
            if (DbContext != null)
            {
                DbContext.Dispose();
            }
            DbContext = null;
            Logic = null;
        }


        [Test]
        public void AddAllThenSave_Transaction_Success()
        {
            var students = new List<Student>
            {
                new Student {Name = "Han"},
                new Student {Name = "Ben"},
                new Student {Name = "My name is really to big, and could be more than length 10."} /*this will case error*/
            };
            Assert.Catch<Exception>(() => Logic.AddAllThenSave(students));
            Assert.AreEqual(0, DbContext.Students.Count());
        }


        [Test]
        public void AddAndSave_Transaction_Success()
        {
            var students = new List<Student>
            {
                new Student {Name = "Han"},
                new Student {Name = "Ben"},
                new Student {Name = "My name is really to big, and could be more than length 10."} /*this will case error*/
            };
            Assert.Catch<Exception>(() => Logic.AddAndSave(students));
            Assert.AreEqual(0, DbContext.Students.Count());
        }

        [Test]
        public void RemoveAllStudent_Transaction_Success()
        {
            DbContext.Students.Add(new Student {Name = "Han"});
            DbContext.SaveChanges();
            DbContext.Addresses.Add(new Address() { Description = "USA", Student = new Student(){ Name = "Ben"} }); /*student with relational address witll throw error*/
            DbContext.SaveChanges();

            Assert.Catch<TransactionException>(() => Logic.RemoveAllStudent());
            Assert.AreEqual(2, DbContext.Students.Count());
            Assert.AreEqual(1, DbContext.Addresses.Count());
        }
    }
}
