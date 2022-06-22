using FluentAssertions;
using NUnit.Framework;

namespace StreamInstruments.Extensions.Tests;

[TestFixture]
public class ExtractString_Extension_Method
{
    [Test]
    [TestCase("(123)", "(", ")", "123")]
    [TestCase("(4, 5, 6, 7, 8, 9)", "(", ")", "4, 5, 6, 7, 8, 9")]
    [TestCase("(abcdefghijklmn())", "(", ")", "abcdefghijklmn()")]
    [TestCase("(abcdefghijklmn())", "(", "))", "abcdefghijklmn(")]
    [TestCase("(abcdefghijklmn())", "abc", "ghi", "def")]
    [TestCase("(abcdefghijklmn())", "abcdefghijklmn", "()", "")]
    [TestCase("(abcdefghijklmn())", "", "", "(abcdefghijklmn())")]
    [TestCase("(abcdefghijklmn())", null, null, "(abcdefghijklmn())")]
    public void Returns_Correct_Substring(string toExtractFrom, string start, string end, string expectedSubstring)
    {
        toExtractFrom.ExtractSubstringBetween(start, end).Should().Be(expectedSubstring);
    }

    [Test]
    public void Throws_ArgumentException_On_Start_Not_Appearing_In_String_ToExtractFrom()
    {
        Action act = () => "def".ExtractSubstringBetween("abc", "def");

        act.Should().Throw<ArgumentException>().WithMessage("'abc' does not appear in string 'def'.");
    }

    [Test]
    public void Throws_ArgumentException_On_End_Not_Appearing_In_String_ToExtractFrom()
    {
        Action act = () => "abc".ExtractSubstringBetween("abc", "def");

        act.Should().Throw<ArgumentException>().WithMessage("'def' does not appear in string 'abc'.");
    }

    [Test]
    public void Throws_ArgumentException_On_Start_String_Located_Ahead_Of_End_String_In_String_ToExtractFrom()
    {
        Action act = () => "abcdefghijklmn".ExtractSubstringBetween("ghi", "abc");

        act.Should().Throw<ArgumentException>().WithMessage("startIndex cannot be ahead of endIndex.");
    }
}