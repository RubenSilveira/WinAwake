﻿using System;
using System.Drawing;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

class SysTray : Component
{
    private readonly ToolStripMenuItem activeToolStripMenuItem;
    private readonly ToolStripMenuItem activeOnStartToolStripMenuItem;
    private readonly NotifyIcon notifyIcon;

    internal event EventHandler ActiveActivated;
    internal event EventHandler ActiveOnStartActivated;
    internal event EventHandler ExitActivated;

    internal SysTray(string status, bool activeStatus, Icon icon)
    {
        activeToolStripMenuItem = new ToolStripMenuItem("Active", null, OnActiveActivated);

        activeOnStartToolStripMenuItem = new ToolStripMenuItem("Active on Start", null, OnActiveOnStartActivated)
        {
            Checked = Settings.ActiveOnStart
        };

        notifyIcon = new NotifyIcon()
        {
            ContextMenuStrip = new ContextMenuStrip()
            {
                Items =
                {
                activeToolStripMenuItem,
                    new ToolStripSeparator(),
                    activeOnStartToolStripMenuItem,
                    new ToolStripSeparator(),
                    new ToolStripMenuItem("Exit", null, OnExitActivated)
                }
            },
            Visible = true
        };

        notifyIcon.Click += OnActiveActivated;

        SetStatus(status, activeStatus, icon);
    }

    internal void SetStatus(string status, bool activeStatus, Icon icon)
    {
        notifyIcon.Text = Assembly.GetExecutingAssembly().GetName().Name;
        if (status?.Trim().Length > 0)
        {
            notifyIcon.Text += "\n" + status.Trim();
        }

        activeToolStripMenuItem.Checked = activeStatus;

        notifyIcon.Icon = icon;
    }

    internal bool ActiveChecked
    {
        get => activeToolStripMenuItem.Checked;
    }

    internal bool ActiveOnStartChecked
    {
        get => activeOnStartToolStripMenuItem.Checked;
        set => activeOnStartToolStripMenuItem.Checked = value;
    }

    internal void OnActiveActivated(object sender, EventArgs e)
    {
        if (e is MouseEventArgs args && MouseButtons.Right == args.Button)
        {
            return;
        }

        ActiveActivated?.Invoke(this, EventArgs.Empty);
    }

    internal void OnActiveOnStartActivated(object sender, EventArgs e)
    {
        ActiveOnStartActivated?.Invoke(this, EventArgs.Empty);
    }

    internal void OnExitActivated(object sender, EventArgs e)
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