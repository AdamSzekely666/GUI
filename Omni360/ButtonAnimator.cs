using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

public static class ButtonAnimator
{
    private static Timer animationTimer;
    private static int colorTransition;
    private static bool goingBlue = true;
    private const int borderSize = 2; // Unified border size

    // Define colors and shimmer frequencies
    private static readonly Color color1Static = Color.Lavender;
    private static readonly Color color1Pressed = Color.Purple;
    private static readonly int purpleFrequency = 300;

    private static readonly Color color2Static = Color.LightGreen;
    private static readonly Color color2Pressed = Color.Green;
    private static readonly int greenFrequency = 300;

    private static readonly Color color3Static = Color.OrangeRed;
    private static readonly Color color3Pressed = Color.Moccasin;
    private static readonly int orangeFrequency = 300;

    private static readonly Color color4Static = Color.DarkGray;
    private static readonly Color color4Pressed = Color.DimGray;
    private static readonly int blueFrequency = 300;

    public static void InitializeAnimation(Button button, string color)
    {
        // Set base and hover colors based on input color
        switch (color)
        {
            case "purple":
                SetButtonColors(button, color1Static, color1Pressed, purpleFrequency);
                break;
            case "green":
                SetButtonColors(button, color2Static, color2Pressed, greenFrequency);
                break;
            case "orange":
                SetButtonColors(button, color3Static, color3Pressed, orangeFrequency);
                break;
            case "blue":
                SetButtonColors(button, color4Static, color4Pressed, blueFrequency);
                break;
        }

        // Attach mouse events for resizing
        button.MouseDown += (sender, e) => ResizeOnMouseDown(button);
        button.MouseUp += (sender, e) => ResizeOnMouseUp(button);

        // Attach paint event to round the border
        button.Paint += (sender, e) => RoundBorder(button, e);
    }

    private static void SetButtonColors(Button button, Color baseColor, Color hoverColor, int shimmerFrequency)
    {
        button.BackColor = baseColor;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.MouseOverBackColor = hoverColor;
        button.FlatAppearance.MouseDownBackColor = hoverColor;
        button.FlatAppearance.BorderSize = borderSize;

        // Initialize shimmer timer with frequency
        animationTimer = new Timer();
        animationTimer.Interval = shimmerFrequency;
        animationTimer.Tick += (sender, e) => ApplyShimmerAnimation(button, baseColor, hoverColor);
       // animationTimer.Start();
    }

    private static void ApplyShimmerAnimation(Button button, Color baseColor, Color hoverColor)
    {
        Color currentColor = button.FlatAppearance.BorderColor;
        Color nextColor = currentColor == baseColor ? hoverColor : baseColor;
        button.FlatAppearance.BorderColor = nextColor;
        button.FlatAppearance.BorderSize = borderSize; // Unified border size
    }

    private static void ResizeOnMouseDown(Button button)
    {
        int widthIncrease = (int)(button.Width * 0.1);
        int heightIncrease = (int)(button.Height * 0.1);
        button.Location = new Point(button.Location.X - (widthIncrease / 2), button.Location.Y - (heightIncrease / 2));
        button.Width += widthIncrease;
        button.Height += heightIncrease;
    }

    private static void ResizeOnMouseUp(Button button)
    {
        int widthIncrease = (int)(button.Width * 0.1);
        int heightIncrease = (int)(button.Height * 0.1);
        button.Location = new Point(button.Location.X + (widthIncrease / 2), button.Location.Y + (heightIncrease / 2));
        button.Width -= widthIncrease;
        button.Height -= heightIncrease;
    }

    private static void RoundBorder(Button button, PaintEventArgs e)
    {
        int borderSize = 3; // Unified border size
        float arcRatio = 0.3f; // 20% of the button size for arcs
        int arcWidth = (int)(button.Width * arcRatio);
        int arcHeight = (int)(button.Height * arcRatio);

        GraphicsPath graphicsPath = new GraphicsPath();
        graphicsPath.AddArc(0, 0, arcWidth, arcHeight, 180, 90);
        graphicsPath.AddArc(button.Width - (arcWidth + 1), 0, arcWidth, arcHeight, 270, 90);
        graphicsPath.AddArc(button.Width - (arcWidth + 1), button.Height - (arcHeight + 1), arcWidth, arcHeight, 0, 90);
        graphicsPath.AddArc(0, button.Height - (arcHeight + 1), arcWidth, arcHeight, 90, 90);
        graphicsPath.CloseAllFigures();
        button.Region = new Region(graphicsPath);

        // Draw border with consistent color
        using (Pen pen = new Pen(button.FlatAppearance.BorderColor, borderSize))
        {
            e.Graphics.DrawPath(pen, graphicsPath);
        }
    }
}
