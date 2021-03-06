﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LiteDB;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace LiteDB.Tests
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public UserDomain Domain { get; set; }
    }

    public class UserDomain
    {
        public string DomainName { get; set; }
    }

    [TestClass]
    public class LinqTest
    {
        [TestMethod]
        public void Linq_Test()
        {
            using (var db = new LiteDatabase(new MemoryStream()))
            {
                var c1 = new User { Id = 1, Name = "Mauricio", Active = true, Domain = new UserDomain { DomainName = "Numeria" } };
                var c2 = new User { Id = 2, Name = "Malatruco", Active = false, Domain = new UserDomain { DomainName = "Numeria" } };
                var c3 = new User { Id = 3, Name = "Chris", Domain = new UserDomain { DomainName = "Numeria" } };
                var c4 = new User { Id = 4, Name = "Juliane" };

                var col = db.GetCollection<User>("Customer");

                col.EnsureIndex(x => x.Name, true);

                col.Insert(new User[] { c1, c2, c3, c4 });

                // sub-class
                Assert.AreEqual(3, col.Count(x => x.Domain.DomainName == "Numeria"));

                // == !=
                Assert.AreEqual(1, col.Count(x => x.Id == 1));
                Assert.AreEqual(3, col.Count(x => x.Id != 1));

                // member booleans
                Assert.AreEqual(3, col.Count(x => !x.Active));
                Assert.AreEqual(1, col.Count(x => x.Active));

                // methods
                Assert.AreEqual(1, col.Count(x => x.Name.StartsWith("mal")));
                Assert.AreEqual(1, col.Count(x => x.Name.Equals("Mauricio")));
                Assert.AreEqual(1, col.Count(x => x.Name.Contains("cio")));

                // > >= < <=
                Assert.AreEqual(1, col.Count(x => x.Id > 3));
                Assert.AreEqual(1, col.Count(x => x.Id >= 4));
                Assert.AreEqual(1, col.Count(x => x.Id < 2));
                Assert.AreEqual(1, col.Count(x => x.Id <= 1));

                // and/or
                Assert.AreEqual(1, col.Count(x => x.Id > 0 && x.Name == "MAURICIO"));
                Assert.AreEqual(2, col.Count(x => x.Name == "malatruco" || x.Name == "MAURICIO"));
            }
        }
    }
}
