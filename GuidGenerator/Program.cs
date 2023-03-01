using System;
using System.Linq;
using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using TextCopy;

namespace GuidGenerator;

internal abstract class Program
{
    private static ControlsConsole _mainConsole;

    private static void Main()
    {
        const int screenWidth = 38;
        const int screenHeight = 10;

        Settings.WindowTitle = "GUIDGen";
        Settings.UseDefaultExtendedFont = true;

        Game.Create(screenWidth, screenHeight);
        Game.Instance.OnStart = Init;
        Game.Instance.Run();
        Game.Instance.Dispose();
    }

    private static void Init()
    {
        _mainConsole = new ControlsConsole(Game.Instance.ScreenCellsX, Game.Instance.ScreenCellsY);

        var title = new Label("- GUIDGen -")
        {
            Position = (1, 1)
        };
        _mainConsole.Controls.Add(title);

        var value = new Label(new Guid().ToString())
        {
            Name = "Value",
            Position = (1, 3)
        };
        _mainConsole.Controls.Add(value);

        var capitalize = new CheckBox(14, 1)
        {
            Name = "Capitalize",
            Position = (1, 6),
            Text = "Capitalize"
        };
        _mainConsole.Controls.Add(capitalize);

        var gen = new Button(12)
        {
            Position = (1, 8),
            Text = "Generate"
        };
        gen.Click += Generate;
        _mainConsole.Controls.Add(gen);

        var copy = new Button(8)
        {
            Position = (15, 8),
            Text = "Copy"
        };
        copy.Click += Copy;
        _mainConsole.Controls.Add(copy);


        Game.Instance.Screen = _mainConsole;

        // This is needed because we replaced the initial screen object with our own.
        Game.Instance.DestroyDefaultStartingConsole();
    }

    private static void Generate(object sender, EventArgs e)
    {
        var checkbox = (CheckBox) _mainConsole.Controls.First(cb => cb.Name == "Capitalize");
        var label = (Label) _mainConsole.Controls.First(c => c.Name == "Value");
        label.DisplayText = checkbox.IsSelected ? Guid.NewGuid().ToString().ToUpper() : Guid.NewGuid().ToString();
    }

    private static void Copy(object sender, EventArgs e)
    {
        var label = (Label) _mainConsole.Controls.First(c => c.Name == "Value");
        ClipboardService.SetText(label.DisplayText);
    }
}