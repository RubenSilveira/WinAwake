using System;
using System.Drawing;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

class SysTray : Component
{
    private readonly NotifyIcon notifyIcon;

    internal event EventHandler ExitActivated;
    internal event EventHandler DoubleClicked;

    internal SysTray(string initialAppendedTitleText, Icon initialIcon)
    {
        notifyIcon = new NotifyIcon()
        {
            ContextMenuStrip = new ContextMenuStrip()
            {
                Items =
                {
                    new ToolStripMenuItem("Exit", null, new EventHandler(OnExitActivated))
                }
            },
            Visible = true
        };

        AppendedTitleText = initialAppendedTitleText;

        notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

        Icon = initialIcon;
    }

    internal string AppendedTitleText
    {
        set
        {
            notifyIcon.Text = Assembly.GetExecutingAssembly().GetName().Name;

            if (value?.Trim().Length > 0)
            {
                notifyIcon.Text += "\n" + value.Trim();
            }
        }
    }

    internal Icon Icon
    {
        get => notifyIcon.Icon;
        set => notifyIcon.Icon = value;
    }

    private void NotifyIcon_DoubleClick(object sender, EventArgs e)
    {
        DoubleClicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnExitActivated(object sender, EventArgs e)
    {
        ExitActivated?.Invoke(this, EventArgs.Empty);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            notifyIcon?.Dispose();
        }
    }
}