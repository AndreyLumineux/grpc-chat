using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ChatWPF.Behaviors
{
    public static class FormattedTextBehaviour
    {
        public static string GetFormattedText(DependencyObject obj)
        {
            return (string)obj.GetValue(FormattedTextProperty);
        }

        public static void SetFormattedText(DependencyObject obj, string value)
        {
            obj.SetValue(FormattedTextProperty, value);
        }

        public static readonly DependencyProperty FormattedTextProperty =
            DependencyProperty.RegisterAttached("FormattedText",
            typeof(string),
            typeof(FormattedTextBehaviour),
            new UIPropertyMetadata("", FormattedTextChanged));

        private static Inline Traverse(string value)
        {
            // Get the sections/inlines
            string[] sections = SplitIntoSections(value);

            // Check for grouping
            if (sections.Length.Equals(1))
            {
                string section = sections[0];

                // Check for token
                if (GetTokenInfo(section, out var token, out var tokenStart, out var tokenEnd))
                {
                    // Get the content to further examination
                    string content = token.Length.Equals(tokenEnd - tokenStart) ?
                        null :
                        section.Substring(token.Length, section.Length - 1 - token.Length * 2);

                    switch (token)
                    {
                        case "<Bold>":
                            return new Bold(Traverse(content));
                        case "<Italic>":
                            return new Italic(Traverse(content));
                        case "<Underline>":
                            return new Underline(Traverse(content));
                        case "<Strikethrough>":
                            var run = new Run(content);
                            run.TextDecorations = TextDecorations.Strikethrough;
                            return run;
                        default:
                            return new Run(section);
                    }
                }
                return new Run(section);
            }
            // Group together

            Span span = new Span();

            foreach (string section in sections)
                span.Inlines.Add(Traverse(section));

            return span;

        }

        private static bool GetTokenInfo(string value, out string token, out int startIndex, out int endIndex)
        {
            token = null;
            endIndex = -1;

            startIndex = value.IndexOf("<");
            int startTokenEndIndex = value.IndexOf(">");

            // No token here
            if (startIndex < 0)
                return false;

            // No token here
            if (startTokenEndIndex < 0)
                return false;

            token = value.Substring(startIndex, startTokenEndIndex - startIndex + 1);

            // Check for closed token. E.g. <LineBreak/>
            if (token.EndsWith("/>"))
            {
                endIndex = startIndex + token.Length;
                return true;
            }

            string endToken = token.Insert(1, "/");

            // Detect nesting;
            int nesting = 0;
            int pos = 0;
            do
            {
                var tempStartTokenIndex = value.IndexOf(token, pos);
                var tempEndTokenIndex = value.IndexOf(endToken, pos);

                if (tempStartTokenIndex >= 0 && tempStartTokenIndex < tempEndTokenIndex)
                {
                    nesting++;
                    pos = tempStartTokenIndex + token.Length;
                }
                else if (tempEndTokenIndex >= 0 && nesting > 0)
                {
                    nesting--;
                    pos = tempEndTokenIndex + endToken.Length;
                }
                else // Invalid tokenized string
                    return false;

            } while (nesting > 0);

            endIndex = pos;

            return true;
        }

        private static string[] SplitIntoSections(string value)
        {
            var sections = new List<string>();

            while (!string.IsNullOrEmpty(value))
            {

                // Check if this is a token section
                if (GetTokenInfo(value, out var token, out var tokenStartIndex, out var tokenEndIndex))
                {
                    // Add pretext if the token isn't from the start
                    if (tokenStartIndex > 0)
                        sections.Add(value.Substring(0, tokenStartIndex));

                    sections.Add(value.Substring(tokenStartIndex, tokenEndIndex - tokenStartIndex));
                    value = value.Substring(tokenEndIndex); // Trim away
                }
                else
                { // No tokens, just add the text
                    sections.Add(value);
                    value = null;
                }
            }

            return sections.ToArray();
        }

        private static void FormattedTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var value = e.NewValue as string;

            if (sender is TextBlock textBlock)
                textBlock.Inlines.Add(Traverse(value));
        }
    }
}
