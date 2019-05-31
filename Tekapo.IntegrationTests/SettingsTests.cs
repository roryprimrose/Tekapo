namespace Tekapo.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using ModelBuilder;
    using Tekapo.Processing;
    using Xunit;

    public class SettingsTests
    {
        [Fact]
        public void IncrementOnCollisionReadsAndWritesSettingsValue()
        {
            var sut = new Settings();

            var original = sut.IncrementOnCollision;

            var updated = original == false;

            sut.IncrementOnCollision = updated;

            sut.IncrementOnCollision.Should().Be(updated);
        }

        [Fact]
        public void NameFormatListReturnsEmptyWhenEmptyValueStored()
        {
            var values = new List<string>();
            var writer = new SettingsWriter();

            var sut = new Settings();

            writer.WriteNameFormatList(values);

            var actual = sut.NameFormatList;

            actual.Should().BeEmpty();
        }

        [Fact]
        public void NameFormatListReturnsStoredValues()
        {
            var values = Model.Create<List<string>>();
            var writer = new SettingsWriter();

            var sut = new Settings();

            writer.WriteNameFormatList(values);

            var actual = sut.NameFormatList;

            actual.Should().BeEquivalentTo(values);
        }

        [Fact]
        public void NameFormatReadsAndWritesSettingsValue()
        {
            var sut = new Settings();

            var updated = Guid.NewGuid().ToString();

            sut.NameFormat = updated;

            sut.NameFormat.Should().Be(updated);
        }

        [Fact]
        public void RecursiveSearchReadsAndWritesSettingsValue()
        {
            var sut = new Settings();

            var original = sut.RecursiveSearch;

            var updated = original == false;

            sut.RecursiveSearch = updated;

            sut.RecursiveSearch.Should().Be(updated);
        }

        [Fact]
        public void RegularExpressionFilterReadsAndWritesSettingsValue()
        {
            var sut = new Settings();

            var updated = Guid.NewGuid().ToString();

            sut.RegularExpressionFilter = updated;

            sut.RegularExpressionFilter.Should().Be(updated);
        }

        [Fact]
        public void SearchDirectoryListReturnsEmptyWhenEmptyValueStored()
        {
            var values = new List<string>();
            var writer = new SettingsWriter();

            var sut = new Settings();

            writer.WriteSearchDirectoryList(values);

            var actual = sut.SearchDirectoryList;

            actual.Should().BeEmpty();
        }

        [Fact]
        public void SearchDirectoryListReturnsStoredValues()
        {
            var values = Model.Create<List<string>>();
            var writer = new SettingsWriter();

            var sut = new Settings();

            writer.WriteSearchDirectoryList(values);

            var actual = sut.SearchDirectoryList;

            actual.Should().BeEquivalentTo(values);
        }

        [Theory]
        [InlineData(SearchFilterType.None)]
        [InlineData(SearchFilterType.RegularExpression)]
        [InlineData(SearchFilterType.Wildcard)]
        public void SearchFilterTypeReadsAndWritesSettingsValue(SearchFilterType value)
        {
            var sut = new Settings {SearchFilterType = value};


            sut.SearchFilterType.Should().Be(value);
        }

        [Fact]
        public void SearchFilterTypeReturnsNoneWhenInvalidValueStored()
        {
            var sut = new Settings {SearchFilterType = (SearchFilterType) int.MaxValue};


            sut.SearchFilterType.Should().Be(SearchFilterType.None);
        }

        [Fact]
        public void SearchPathReadsAndWritesSettingsValue()
        {
            var sut = new Settings();

            var updated = Guid.NewGuid().ToString();

            sut.SearchPath = updated;

            sut.SearchPath.Should().Be(updated);
        }

        [Fact]
        public void WildcardFilterReadsAndWritesSettingsValue()
        {
            var sut = new Settings();

            var updated = Guid.NewGuid().ToString();

            sut.WildcardFilter = updated;

            sut.WildcardFilter.Should().Be(updated);
        }
    }
}