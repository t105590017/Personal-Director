using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Production.Enum
{
    public enum VideoPosition
    {
        [Description("")]
        TopLeft,
        [Description("x=(w-text_w)/2:")]
        TopCenter,
        [Description("x=(w-text_w):")]
        TopRight,
        [Description("y=(h-text_h)/2:")]
        CenterLeft,
        [Description("x=(w-text_w)/2:y=(h-text_h)/2:")]
        Center,
        [Description("x=(w-text_w):y=(h-text_h)/2:")]
        CenterRight,
        [Description("y=(h-text_h):")]
        ButtomLeft,
        [Description("x=(w-text_w)/2:y=(h-text_h):")]
        ButtomCenter,
        [Description("x=(w-text_w):y=(h-text_h):")]
        ButtomRight
    }

}
