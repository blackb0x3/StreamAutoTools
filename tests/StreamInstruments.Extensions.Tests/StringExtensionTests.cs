using FluentAssertions;
using NUnit.Framework;

namespace StreamInstruments.Extensions.Tests;

[TestFixture]
public class ExtractString_Extension_Method
{
    [Test]
    [TestCase("", "", "", "")]
    [TestCase("(123)", "(", ")", "123")]
    [TestCase("(4, 5, 6, 7, 8, 9)", "(", ")", "4, 5, 6, 7, 8, 9")]
    [TestCase("(abcdefghijklmn())", "(", ")", "abcdefghijklmn()")]
    [TestCase("(abcdefghijklmn())", "(", "))", "abcdefghijklmn(")]
    [TestCase("(abcdefghijklmn())", "abc", "ghi", "def")]
    [TestCase("(abcdefghijklmn())", "abcdefghijklmn", "()", "")]
    [TestCase("(abcdefghijklmn())", "", "", "(abcdefghijklmn())")]
    public void Returns_Correct_Substring(string toExtractFrom, string start, string end, string expectedSubstring)
    {
        toExtractFrom.ExtractSubstring(start, end).Should().Be(expectedSubstring);
    }

    [Test]
    public void Throws_ArgumentException_On_Start_Not_Appearing_In_String_ToExtractFrom()
    {
        Action act = () => "def".ExtractSubstring("abc", "def");

        act.Should().Throw<ArgumentException>().WithMessage("'abc' does not appear in string 'def'.");
    }

    [Test]
    public void Throws_ArgumentException_On_End_Not_Appearing_In_String_ToExtractFrom()
    {
        Action act = () => "abc".ExtractSubstring("abc", "def");

        act.Should().Throw<ArgumentException>().WithMessage("'def' does not appear in string 'abc'.");
    }

    [Test]
    public void Throws_ArgumentException_On_Start_String_Located_Ahead_Of_End_String_In_String_ToExtractFrom()
    {
        Action act = () => "abcdefghijklmn".ExtractSubstring("ghi", "abc");

        act.Should().Throw<ArgumentException>().WithMessage("startIndex cannot be ahead of endIndex.");
    }
}