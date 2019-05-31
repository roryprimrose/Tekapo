namespace Tekapo.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using ModelBuilder;
    using Xunit;

    public class SettingsWriterTests
    {
        [Fact]
        public void SaveWriteDoesNotThrowException()
        {
            var sut = new SettingsWriter();

            Action action = () => sut.Save();

            action.Should().NotThrow();
        }

        [Fact]
        public void WriteNameFormatListWritesEmptyListWhenValueIsEmpty()
        {
            var values = new List<string>();
            var settings = new Settings();

            var sut = new SettingsWriter();

            sut.WriteNameFormatList(values);

            settings.NameFormatList.Should().BeEmpty();
        }

        [Fact]
        public void WriteNameFormatListWritesEmptyListWhenValueIsNull()
        {
            var settings = new Settings();

            var sut = new SettingsWriter();

            sut.WriteNameFormatList(null);

            settings.NameFormatList.Should().BeEmpty();
        }

        [Fact]
        public void WriteNameFormatListWritesList()
        {
            var values = Model.Create<List<string>>();
            var settings = new Settings();

            var sut = new SettingsWriter();

            sut.WriteNameFormatList(values);

            settings.NameFormatList.Should().BeEquivalentTo(values);
        }

        [Fact]
        public void WriteSearchDirectoryListWritesEmptyListWhenValueIsEmpty()
        {
            var values = new List<string>();
            var settings = new Settings();

            var sut = new SettingsWriter();

            sut.WriteSearchDirectoryList(values);

            settings.SearchDirectoryList.Should().BeEmpty();
        }

        [Fact]
        public void WriteSearchDirectoryListWritesEmptyListWhenValueIsNull()
        {
            var settings = new Settings();

            var sut = new SettingsWriter();

            sut.WriteSearchDirectoryList(null);

            settings.SearchDirectoryList.Should().BeEmpty();
        }

        [Fact]
        public void WriteSearchDirectoryListWritesList()
        {
            var values = Model.Create<List<string>>();
            var settings = new Settings();

            var sut = new SettingsWriter();

            sut.WriteSearchDirectoryList(values);

            settings.SearchDirectoryList.Should().BeEquivalentTo(values);
        }
    }
}