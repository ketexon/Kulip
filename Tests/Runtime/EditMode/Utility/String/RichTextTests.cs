using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Kulip.Utility;

public class RichTextTests
{
    [Test]
    public void RichTextTests_SingleTags()
    {
        LazyRichText rt = new LazyRichText("<a>hi</a> there <b></b>");
        Assert.AreEqual("hi there ", rt.PlainText);
        Assert.AreEqual(2, rt.Tags.Count);

        var aTag = rt.Tags[0].StartText == "<a>" ? rt.Tags[0] : rt.Tags[1];
        var bTag = rt.Tags[0] == aTag ? rt.Tags[1] : rt.Tags[0];

        Assert.AreEqual("<a>", aTag.StartText);
        Assert.AreEqual("</a>", aTag.EndText);
        Assert.AreEqual(0, aTag.StartIndex);
        Assert.AreEqual(2, aTag.EndIndex);

        Assert.AreEqual("<b>", bTag.StartText);
        Assert.AreEqual("</b>", bTag.EndText);
        Assert.AreEqual(9, bTag.StartIndex);
        Assert.AreEqual(9, bTag.EndIndex);
    }


    [Test]
    public void RichTextTests_NestedTags()
    {
        LazyRichText rt = new LazyRichText("<a><b>hello <c>there</c> friend</b></a>");
        Assert.AreEqual("hello there friend", rt.PlainText);

        var tagStarts = new List<string>{ "<a>", "<b>", "<c>" };

        foreach(var tag in rt.Tags)
        {
            Assert.That(tagStarts.Contains(tag.StartText));
            switch (tag.StartText)
            {
                case "<a>":
                    Assert.AreEqual("</a>", tag.EndText);
                    Assert.AreEqual(0, tag.StartIndex);
                    Assert.AreEqual(18, tag.EndIndex);
                    break;
                case "<b>":
                    Assert.AreEqual("</b>", tag.EndText);
                    Assert.AreEqual(0, tag.StartIndex);
                    Assert.AreEqual(18, tag.EndIndex);
                    break;
                case "<c>":
                    Assert.AreEqual("</c>", tag.EndText);
                    Assert.AreEqual(6, tag.StartIndex);
                    Assert.AreEqual(11, tag.EndIndex);
                    break;
            }
        }
        
    }
}
