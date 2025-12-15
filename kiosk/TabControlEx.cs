using System;
using System.Collections.Generic;
using System.Windows.Forms;

public class TabControlEx : TabControl
{
    // List of tab pages to hide
    public List<TabPage> HiddenTabs { get; set; } = new List<TabPage>();

    protected override void WndProc(ref Message m)
    {
        // 0x1328 = TCM_ADJUSTRECT message to prevent drawing hidden tab headers
        if (m.Msg == 0x1328 && SelectedTab != null && HiddenTabs.Contains(SelectedTab))
        {
            m.Result = (IntPtr)1;
            return;
        }
        base.WndProc(ref m);
    }

    protected override void OnSelecting(TabControlCancelEventArgs e)
    {
        // Cancel selection if the tab is hidden
        if (HiddenTabs.Contains(e.TabPage))
        {
            e.Cancel = true;
            return;
        }
        base.OnSelecting(e);
    }
}
