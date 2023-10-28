using System.Collections.Generic;
using UnityEngine;
using System;

namespace Kulip.Utility
{
    public class RichTextInvalidTagException : Exception
    {
        public RichTextInvalidTagException() { }
        public RichTextInvalidTagException(string m) : base(m) { }
        public RichTextInvalidTagException(string m, Exception inner) : base(m, inner) { }
    }

    public class LazyRichText
    {
        public class Tag
        {
            public string StartText;

            public string EndText;

            /// <summary>
            /// Inclusive
            /// </summary>
            public int StartIndex;

            /// <summary>
            /// Exclusive
            /// </summary>
            public int EndIndex;

            public Tag(string startText, string endText, int startIndex, int endIndex)
            {
                StartText = startText;
                EndText = endText;
                StartIndex = startIndex;
                EndIndex = endIndex;
            }
        }

        public readonly string Text;
        public readonly string PlainText = "";
        public readonly List<Tag> Tags = new List<Tag>();


        public static int GetCharIndexFromRichTextIndex(string richText, int index)
        {
            int charIndex = 0;
            bool inPotentialTag = false;
            string potentialTag = "";
            for (int i = 0; i < index; ++i)
            {
                char c = richText[i];

                if (!inPotentialTag)
                {
                    if (c == '<')
                    {
                        inPotentialTag = true;
                        potentialTag = "<";
                    }
                    else
                    {
                        charIndex++;
                    }
                }
                else
                {
                    // Not a real tag
                    if (c == '<')
                    {
                        potentialTag = "";
                        charIndex += potentialTag.Length;
                        inPotentialTag = false;
                        i--; // we dont parse the next tag, so redo this iter
                    }
                    // Real tag
                    else if (c == '>')
                    {
                        potentialTag = "";
                        inPotentialTag = false;
                    }
                    else
                    {
                        potentialTag += c;
                    }
                }
            }
            // if potentialTag is not "", then it is an unclosed tag, so it is not a real tag
            charIndex += potentialTag.Length;
            return charIndex;
        }

        public static int GetPlainTextLength(string richText) =>
            GetCharIndexFromRichTextIndex(richText, richText.Length - 1) + 1;

        public LazyRichText(string richText)
        {
            Text = richText;
            
            // stack of (plain text index, tag string)
            Stack<(int, string)> tagStarts = new Stack<(int, string)>();

            int plainTextIndex = 0;

            for (int i = 0; i < richText.Length; i++)
            {
                var c = Text[i];
                if(c == '<')
                {
                    i++;
                    c = Text[i];
                    if(c != '/') // open tag
                    {
                        var potentialTag = "<";
                        var tagValid = false;

                        while (i < richText.Length)
                        {
                            c = Text[i];
                            if (c == '>') // found closing bracket
                            {
                                potentialTag += '>';
                                if(potentialTag.Length > 2)
                                {
                                    tagValid = true;
                                    // hi <b>there</b> friend
                                    // 012   3456789...
                                    //       ^ plaintext index here
                                    tagStarts.Push((plainTextIndex, potentialTag));
                                    break;
                                }
                                else
                                {
                                    tagValid = false;
                                    break;
                                }
                            }
                            else if(c == '<') // found another open bracket before closing
                            {
                                tagValid = false;
                                i--;
                                break;
                            }
                            else
                            {
                                potentialTag += c;
                            }
                            i++;
                        }
                        if (!tagValid)
                        {
                            PlainText += potentialTag;
                        }
                    }
                    else // closing tag
                    {
                        i++;
                        var potentialTag = "</";
                        var tagValid = false;

                        while (i < richText.Length)
                        {
                            c = Text[i];
                            if (c == '>') // found closing bracket
                            {
                                potentialTag += '>';
                                if (potentialTag.Length > 3)
                                {
                                    (int, string) startTag;
                                    if (tagStarts.TryPop(out startTag))
                                    {
                                        // hi <b>there</b> friend
                                        // 012   345678   9...
                                        //                ^ plaintext index here
                                        tagValid = true;
                                        Tags.Add(
                                            new Tag(
                                                startText: startTag.Item2,
                                                endText: potentialTag,
                                                startIndex: startTag.Item1,
                                                endIndex: plainTextIndex
                                            )
                                        );
                                    }
                                    else // no corresponding starttag
                                    {
                                        tagValid = false;
                                    }
                                    break;
                                }
                                else
                                {
                                    tagValid = false;
                                    break;
                                }
                            }
                            else if (c == '<') // found another open bracket before closing
                            {
                                tagValid = false;
                                i--;
                                break;
                            }
                            else
                            {
                                potentialTag += c;
                            }
                            i++;
                        }
                        if (!tagValid)
                        {
                            PlainText += potentialTag;
                        }
                    }
                }
                else
                {
                    PlainText += c;
                    plainTextIndex++;
                }
            }
        }

        public LazyRichText Substring(int startIndex = 0, int length = -1)
        {
            int endIndex;
            if(length < 0){
                endIndex = PlainText.Length;
            }
            else
            {
                endIndex = startIndex + length;
                if (endIndex > PlainText.Length)
                {
                    throw new System.ArgumentException("Length exceeds length of PlainText");
                }
            }

            string outText = "";
            var activeTags = new List<Tag>(Tags);
            var startedTags = new Stack<Tag>();

            var tagsBeforeStart = new List<Tag>();
            foreach(var tag in activeTags)
            {
                if(tag.StartIndex < startIndex && tag.EndIndex > startIndex)
                {
                    tagsBeforeStart.Add(tag);
                }
            }
            SortTags(tagsBeforeStart);
            foreach(var tag in tagsBeforeStart)
            {
                outText += tag.StartText;
                startedTags.Push(tag);
            }

            for(int i = startIndex; i < endIndex; i++)
            {
                while (startedTags.TryPeek(out Tag topStartedTag) && topStartedTag.EndIndex == i)
                {
                    outText += topStartedTag.EndText;
                    activeTags.Remove(startedTags.Pop());
                }

                var startTagsAtIndex = new List<Tag>();
                foreach(var tag in activeTags)
                {
                    if(tag.StartIndex == i)
                    {
                        startTagsAtIndex.Add(tag);
                    }
                }
                SortTagsSameStart(startTagsAtIndex);
                foreach(var tag in startTagsAtIndex)
                {
                    //Debug.Log(tag);
                    outText += tag.StartText;
                    startedTags.Push(tag);
                }

                outText += PlainText[i];
            }
            foreach(var tag in startedTags)
            {
                outText += tag.EndText;
            }
            return new LazyRichText(outText);
        }

        public LazyRichText AddTags(List<Tag> newTags)
        {
            string outText = "";
            var activeTags = new List<Tag>(Tags);
            activeTags.AddRange(newTags);
            var startedTags = new Stack<Tag>();

            for (int i = 0; i < PlainText.Length; i++)
            {
                while (startedTags.TryPeek(out Tag topStartedTag) && topStartedTag.EndIndex == i)
                {
                    outText += topStartedTag.EndText;
                    activeTags.Remove(startedTags.Pop());
                }

                var startTagsAtIndex = new List<Tag>();
                foreach (var tag in activeTags)
                {
                    if (tag.StartIndex == i)
                    {
                        startTagsAtIndex.Add(tag);
                        activeTags.Remove(tag);
                    }
                }
                SortTagsSameStart(startTagsAtIndex);
                foreach (var tag in startTagsAtIndex)
                {
                    Debug.LogFormat("Tag: {0}, from: {1}, to: {2}", tag.StartText, tag.StartIndex, tag.EndIndex);
                    outText += tag.StartText;
                    startedTags.Push(tag);
                }
                outText += PlainText[i];
            }
            foreach (var tag in startedTags)
            {
                outText += tag.EndText;
            }
            var outRichText = new LazyRichText(outText);
            return outRichText;
        }


        private void SortTags(List<Tag> tags)
        {
            tags.Sort(
                (t1, t2) =>
                {
                    var startCompare = Comparer<int>.Default.Compare(t1.StartIndex, t2.StartIndex);
                    if (startCompare == 0)
                    {
                        return -Comparer<int>.Default.Compare(t1.EndIndex, t2.EndIndex);
                    }
                    else
                    {
                        return startCompare;
                    }
                }
            );
        }

        private void SortTagsSameStart(List<Tag> tags)
        {
            tags.Sort((t1, t2) => -Comparer<int>.Default.Compare(t1.EndIndex, t2.EndIndex));
        }
    }
}