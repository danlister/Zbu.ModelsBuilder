﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Zbu.ModelsBuilder.Building;

namespace Zbu.ModelsBuilder.Tests
{
    [TestFixture]
    public class CompilerTests
    {
        private string _tempDir;

        [SetUp]
        public void Setup()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDir);
        }

        [TearDown]
        public void TearDown()
        {
            if (_tempDir != null && Directory.Exists(_tempDir))
                Directory.Delete(_tempDir, true);
        }

        [Test]
        public void Compile()
        {
            const string code1 = @"
using System;
namespace Whatever
{
    public class Something
    {
        public void DoSomething()
        {
            Console.WriteLine(""Hello!"");
        }
    }
}
";

            var compiler = new Compiler();
            compiler.Compile(_tempDir, "Whatever", new Dictionary<string, string>{{"code", code1}});
            Assert.IsTrue(File.Exists(Path.Combine(_tempDir, "Whatever.dll")));
        }

        [Test]
        public void CompileCSharp6()
        {
            // see https://roslyn.codeplex.com/wikipage?title=Language%20Feature%20Status&referringTitle=Documentation

            const string code1 = @"
using System;
namespace Whatever
{
    public class Something
    {
        // auto-property initializer
        public int Value { get; set; } = 3;

        public void DoSomething()
        {
            Console.WriteLine(""Hello!"");
        }

        public void DoSomething(string[] args)
        {
            // conditional access
            var len = args?.Length ?? 0;
        }
    }
}
";

            var compiler = new Compiler(LanguageVersion.Experimental);
            compiler.Compile(_tempDir, "Whatever", new Dictionary<string, string> { { "code", code1 } });
            Assert.IsTrue(File.Exists(Path.Combine(_tempDir, "Whatever.dll")));
        }
    }
}
