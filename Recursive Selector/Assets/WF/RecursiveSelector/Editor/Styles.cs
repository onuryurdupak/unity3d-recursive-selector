using UnityEngine;

namespace WF.RecursiveSelector
{
    public class Styles
    {
        private static GUIStyle whiteFont;
        private static GUIStyle leftAligned;
        private static GUIStyle leftAlignedWhiteFont;

        public static GUIStyle WhiteFont
        {
            get
            {
                if (whiteFont == null)
                {
                    whiteFont = new();
                    whiteFont.normal.textColor = Color.white;
                }

                return whiteFont;
            }
        }

        public static GUIStyle LeftAligned
        {
            get
            {
                if (leftAligned == null)
                {
                    leftAligned = new();
                    leftAligned.alignment = TextAnchor.MiddleLeft;
                }

                return leftAligned;
            }
        }

        public static GUIStyle LeftAlignedWhiteFont
        {
            get
            {
                if (leftAlignedWhiteFont == null)
                {
                    leftAlignedWhiteFont = new();
                    leftAlignedWhiteFont.alignment = TextAnchor.MiddleLeft;
                    leftAlignedWhiteFont.normal.textColor = Color.white;
                }

                return leftAlignedWhiteFont;
            }
        }
    }
}
