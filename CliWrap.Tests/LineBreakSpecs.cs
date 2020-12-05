﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace CliWrap.Tests
{
    public class LineBreakSpecs
    {
        [Fact]
        public async Task Newline_char_is_treated_as_a_line_break()
        {
            // Arrange
            const string content = "Foo\nBar\nBaz";

            var cmd = content | Cli.Wrap("dotnet")
                .WithArguments(a => a
                    .Add(Dummy.Program.FilePath)
                    .Add(Dummy.Program.EchoStdInToStdOut));

            var stdOutLines = new List<string>();

            // Act
            await (cmd | stdOutLines.Add).ExecuteAsync();

            // Assert
            stdOutLines.Should().HaveCount(3);
        }

        [Fact]
        public async Task Caret_return_char_is_treated_as_a_line_break()
        {
            // Arrange
            const string content = "Foo\rBar\rBaz";

            var cmd = content | Cli.Wrap("dotnet")
                .WithArguments(a => a
                    .Add(Dummy.Program.FilePath)
                    .Add(Dummy.Program.EchoStdInToStdOut));

            var stdOutLines = new List<string>();

            // Act
            await (cmd | stdOutLines.Add).ExecuteAsync();

            // Assert
            stdOutLines.Should().HaveCount(3);
        }

        [Fact]
        public async Task Caret_return_char_followed_by_newline_char_is_treated_as_a_single_line_break()
        {
            // Arrange
            const string content = "Foo\r\nBar\r\nBaz";

            var cmd = content | Cli.Wrap("dotnet")
                .WithArguments(a => a
                    .Add(Dummy.Program.FilePath)
                    .Add(Dummy.Program.EchoStdInToStdOut));

            var stdOutLines = new List<string>();

            // Act
            await (cmd | stdOutLines.Add).ExecuteAsync();

            // Assert
            stdOutLines.Should().HaveCount(3);
        }

        [Fact]
        public async Task Multiple_consecutive_line_breaks_result_in_empty_lines()
        {
            // Arrange
            const string content = "Foo\r\rBar\n\nBaz";

            var cmd = content | Cli.Wrap("dotnet")
                .WithArguments(a => a
                    .Add(Dummy.Program.FilePath)
                    .Add(Dummy.Program.EchoStdInToStdOut));

            var stdOutLines = new List<string>();

            // Act
            await (cmd | stdOutLines.Add).ExecuteAsync();

            // Assert
            stdOutLines.Should().HaveCount(5);
            stdOutLines.Where(string.IsNullOrEmpty).Should().HaveCount(2);
        }
    }
}