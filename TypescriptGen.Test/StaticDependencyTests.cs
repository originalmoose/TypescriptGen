using Xunit;

namespace TypeGen.Test
{
    public class StaticDependencyTests
    {
        [Fact]
        public void SingleImport()
        {
            var test = new StaticDependency
            {
                ImportPath = "mobx",
                Exports =
                {
                    "observable",
                },
            };
            Assert.Equal("import { observable } from 'mobx';", test.ToString());
        }

        [Fact]
        public void TwoImports()
        {
            var test = new StaticDependency
            {
                ImportPath = "mobx",
                Exports =
                {
                    "observable",
                    "decorate",
                },
            };

            Assert.Equal("import { observable, decorate } from 'mobx';", test.ToString());
        }

        [Fact]
        public void DefaultImport()
        {
            var test = new StaticDependency
            {
                ImportPath = "mobx", 
                DefaultExport = "Mobx",
            };

            Assert.Equal("import Mobx from 'mobx';", test.ToString());
        }

        [Fact]
        public void StarAsImport()
        {
            var test = new StaticDependency
            {
                ImportPath = "mobx", 
                DefaultExport = "Mobx",
                UseStarAs = true,
            };

            Assert.Equal("import * as Mobx from 'mobx';", test.ToString());
        }

        [Fact]
        public void DualImport_Default()
        {
            var test = new StaticDependency
            {
                ImportPath = "react", 
                DefaultExport = "React",
                Exports =
                {
                    "Component",
                },
            };

            Assert.Equal("import React, { Component } from 'react';", test.ToString());
            
        }
    }
}