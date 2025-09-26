using BroadcastPluginSDK.Classes;
using CyberDog.Interfaces;
using CyberDog.Renderers;
using System.Windows.Forms;

namespace Command.Renderers
{
    public class CommandRenderer<T> : DefaultItemRenderer<T> where T : class, IUpdatableItem
    {
        public override void Draw(Graphics g, T item, Rectangle bounds)
        {
            //g.TextRenderingHint = TextRenderingHint.ClearTypeGridFi
            // Optional: use item.Value or item.Key to customize visuals
            bounds.Inflate(-5, -4); // Padding inside the panel
            DrawOutline(g, bounds);

            int H = (bounds.Height / 2);
            int Y = H - 8;

            if (item is CommandItem CmdItem)
            {
                using Brush brush = CmdItem.Status switch
                {
                    CommandStatus.New => new SolidBrush(Color.LightSteelBlue),
                    CommandStatus.Failed => new SolidBrush(Color.IndianRed),
                    CommandStatus.Queued => new SolidBrush(Color.LightYellow),
                    CommandStatus.InProgress => new SolidBrush(Color.LightGreen),
                    CommandStatus.Completed => new SolidBrush(Color.LightGray),
                    _ => new SolidBrush(Color.PaleVioletRed)
                };

                using var path = CreateRoundedRect(bounds, CornerRadius);
                g.FillPath(brush, path);

                var ValueRect = new Rectangle(10 , Y - 10, 250 , H);
                var KeyRect = new Rectangle(15, Y + 22, 250 , H);
                var DateRect = new Rectangle(280, Y + 22, 150, H);
                var CmdRect = new Rectangle( 280, Y , 500, H);

                DrawText(g, CmdItem.Value, ValueRect , new("Segoe UI", 18, FontStyle.Regular));
                DrawText(g, CmdItem.FullComand ?? string.Empty, CmdRect , new("Segoe UI", 10, FontStyle.Regular) );
                DrawText(g, CmdItem.Key, KeyRect , new("Segoe UI", 8, FontStyle.Regular) , new SolidBrush( Color.Green)  );
                DrawText(g, CmdItem.CreatedAt.ToString(), DateRect, new("Segoe UI", 8, FontStyle.Regular), new SolidBrush(Color.Green));
            }
        }

    }

}
