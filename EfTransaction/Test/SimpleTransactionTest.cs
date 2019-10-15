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
            var db = new UmsDbContext();
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
                new Student {Name = "My name is really to big, and could be more than length 50."} /*this will case error*/
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


        [Test]
        public void UpdateAndSave_Success()
        {
            var students = new List<Student>
            {
                new Student {Name = "Han"},
                new Student {Name = "Ben"},
                new Student {Name = "Dan"},
                new Student {Name = "Mos"},
            };
            students.ForEach(x => { DbContext.Students.Add(x); });
            DbContext.SaveChanges();

            Assert.DoesNotThrow(() => new StudentUpdateLogic(DbContext).UpdateAndSave(students));
            students = DbContext.Students.ToList();
            Assert.IsNotNullOrEmpty(StudentUpdateLogic.UpdatePart);
            Assert.IsTrue(students[0].Name.Contains(StudentUpdateLogic.UpdatePart));
            Assert.IsTrue(students[1].Name.Contains(StudentUpdateLogic.UpdatePart));
            Assert.IsTrue(students[2].Name.Contains(StudentUpdateLogic.UpdatePart));
            Assert.IsTrue(students[3].Name.Contains(StudentUpdateLogic.UpdatePart));
        }

        [Test]
        public void UpdateAndSave_Fail()
        {
            var students = new List<Student>
            {
                new Student {Name = "Han"},
                new Student {Name = "Ben"},
                new Student {Name = "Dan"},
                new Student {Name = "Mos"},
                new Student {Name = "Jeff"},
                /*during update this will case error*/
                new Student {Name = "49 length text, during update this will cross 50."},
            };
            students.ForEach(x => { DbContext.Students.Add(x); });
            DbContext.SaveChanges();

            Assert.Catch<Exception>(() => new StudentUpdateLogic(new UmsDbContext()).UpdateAndSave(students));
            var dbStudents = new UmsDbContext().Students.ToList();
            Assert.False(dbStudents[0].Name.Contains(StudentUpdateLogic.UpdatePart));
            Assert.False(dbStudents[1].Name.Contains(StudentUpdateLogic.UpdatePart));
            Assert.False(dbStudents[2].Name.Contains(StudentUpdateLogic.UpdatePart));
            Assert.False(dbStudents[3].Name.Contains(StudentUpdateLogic.UpdatePart));
            Assert.False(dbStudents[4].Name.Contains(StudentUpdateLogic.UpdatePart));
        }
    }
}