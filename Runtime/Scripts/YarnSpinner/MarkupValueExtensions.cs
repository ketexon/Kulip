using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulip
{
    public static class MarkupValueExtensions
    {
        public static bool IsNumberValue(this Yarn.Markup.MarkupValue markup)
        {
            return markup.Type == Yarn.Markup.MarkupValueType.Integer
                || markup.Type == Yarn.Markup.MarkupValueType.Float;
        }

        public static float GetNumberValue(this Yarn.Markup.MarkupValue markup)
        {
            if(markup.Type == Yarn.Markup.MarkupValueType.Integer)
            {
                return markup.IntegerValue;
            }
            else
            {
                return markup.FloatValue;
            }
        }
    }
}
