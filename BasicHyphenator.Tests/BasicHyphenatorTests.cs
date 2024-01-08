namespace Morpher.Russian;

public class BasicHyphenatorTests
{
    [Test]
    [TestCase("в")]
    [TestCase("РСДРП")]
    [TestCase("ия")]
    [TestCase("азия")]
    [TestCase("фойе")]
    [TestCase("ба-ня")]
    [TestCase("аня")]
    [TestCase("кот")]
    [TestCase("кий")]
    [TestCase("ки-с-ка")]
    [TestCase("ях-та")]
    [TestCase("взвод")]
    [TestCase("мкртчан")]
    [TestCase("мо-л-ния")]
    [TestCase("алоэ")]
    [TestCase("бу-ль-он")]
    [TestCase("съём")]
    [TestCase("по-дъ-ём")]
    [TestCase("се-мья")]
    [TestCase("зме-е-ед")]
    [TestCase("адъ-ю-тант")]
    [TestCase("ра-с-с-чёт")]
    [TestCase("ма-с-ки")]
    [TestCase("ру-с-с-кий")]
    [TestCase("ру-с-с-кие")]
    [TestCase("ру-с-с-ки-ми")]
    [TestCase("ра-до-с-т-ный")]
    [TestCase("уи-ль-ям")]
    [TestCase("майя")]
    [TestCase("майор")]
    [TestCase("йог")]
    [TestCase("ли-с-т-ва")]
    [TestCase("вы-к-ри-с-та-л-ли-зо-ва-в-ши-е-ся")]
    public void HyphenateRussianWord(string expectedOutput)
    {
        string input = expectedOutput.Replace("-", "");
        string actualOutput = BasicHyphenator.Hyphenate(input);
        Assert.That(actualOutput, Is.EqualTo(expectedOutput));
    }
    
    [TestCase(" ")]
    [TestCase("<p style='font-size:10px'>")]
    public void LeaveNonRussianContentAsIs(string input)
    {
        string actualOutput = BasicHyphenator.Hyphenate(input);
        Assert.That(actualOutput, Is.EqualTo(input));
    }
}