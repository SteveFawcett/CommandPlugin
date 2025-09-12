using BroadcastPluginSDK.Classes;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace CommandPlugin.Classes
{
    public class JobCardRenderer
    {
        // Fonts
        private readonly Font _repoFont = new("Segoe UI", 10, FontStyle.Bold);
        private readonly Font _metaFont = new("Segoe UI", 8, FontStyle.Regular);

        // Brushes & Pens
        private readonly Brush _cardBrush = new SolidBrush(Color.WhiteSmoke);
        private readonly Pen _borderPen = new(Color.LightGray);
        private readonly Brush _highlightBrush = new SolidBrush(Color.LightSteelBlue);
        private readonly Brush _repoTextBrush = new SolidBrush(Color.Black);
        private readonly Brush _metaTextBrush = new SolidBrush(Color.Gray);

        // Layout constants
        private const int Padding = 10;
        private const int LineHeight = 20;
        private const int CornerRadius = 8;
        private const int BadgeHeight = 22;

        public void Draw(Graphics g, Rectangle bounds, CommandItem item, bool isSelected)
        {
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var cardRect = new Rectangle(bounds.X + 5, bounds.Y + 4, bounds.Width - 10, bounds.Height - 8);

            // Shadow rendering
            var shadowOffset = 3;
            var shadowRect = new Rectangle(cardRect.X + shadowOffset, cardRect.Y + shadowOffset, cardRect.Width, cardRect.Height);
            using var shadowPath = CreateRoundedRect(shadowRect, CornerRadius);
            using var shadowBrush = new SolidBrush(Color.FromArgb(60, 0, 0, 0)); // Semi-transparent black
            g.FillPath(shadowBrush, shadowPath);

            // Card background
            using var cardPath = CreateRoundedRect(cardRect, CornerRadius);
            g.FillPath(isSelected ? _highlightBrush : _cardBrush, cardPath);
            g.DrawPath(_borderPen, cardPath);

            // Text layout
            int textX = cardRect.X + Padding;
            int textY = cardRect.Y + Padding;

            g.DrawString(item.Name, _repoFont, _repoTextBrush, textX, textY);
            g.DrawString(item.Description, _metaFont, _metaTextBrush, textX, textY + LineHeight);
            g.DrawString(item.Id, _metaFont, _metaTextBrush, textX, textY + ( LineHeight *2 ));

            // Badge layout
            int badgeX = cardRect.Right - Padding;
            int badgeY = cardRect.Y + Padding + 20;

            var (label, backColor) = item.Status switch
            {
                CommandStatus.New => ("New", Color.Gray),
                CommandStatus.Queued => ("Queued", Color.ForestGreen),
                CommandStatus.InProgress => ("Running", Color.Orange),
                CommandStatus.Completed => ("Completed", Color.ForestGreen),
                CommandStatus.Failed => ("Failed", Color.Red),
                _ => ("Unknown", Color.DarkGray)
            };

            DrawBadge(g, ref badgeY, badgeX, label, backColor, Color.White);
        }

        private void DrawBadge(Graphics g, ref int badgeY, int badgeRightX, string text, Color bgColor, Color fgColor)
        {
            var textSize = g.MeasureString(text, _metaFont);
            int badgeWidth = (int)textSize.Width + 20;
            int badgeX = badgeRightX - badgeWidth;

            var badgeRect = new Rectangle(badgeX, badgeY, badgeWidth, BadgeHeight);
            using var badgePath = CreateRoundedRect(badgeRect, 10);

            g.FillPath(new SolidBrush(bgColor), badgePath);

            float textX = badgeRect.X + (badgeRect.Width - textSize.Width) / 2;
            float textY = badgeRect.Y + (badgeRect.Height - textSize.Height) / 2;

            g.DrawString(text, _metaFont, new SolidBrush(fgColor), textX, textY);

            badgeY += BadgeHeight + 6; // Move down for next badge
        }

        private GraphicsPath CreateRoundedRect(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
