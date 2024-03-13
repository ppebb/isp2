using System;
using System.Collections;
using System.Linq;
using Blackguard.Utilities;

namespace Blackguard.UI.Popups;

public class DebugPopup : Popup {
    private const int WIDTH = 40;

    private static string FormatIEnumerable(string start, bool cond, IEnumerable objects) {
        if (!cond)
            return start;

        int wNoBorder = WIDTH - 2;
        string line = start + string.Join(' ', objects.Cast<object>().Select(o => o.ToString()));

        if (line.Length > wNoBorder)
            line = line[..wNoBorder];

        if (line.Length < wNoBorder)
            line += new string(' ', wNoBorder - line.Length);

        return line;

    }

    private static readonly (Highlight h, Func<Game, string> f)[] segments = [
        (Highlight.Text, (state) => $"Ticks: {state.ticks}"),
        (Highlight.Text, (state) => $"Seconds (from ticks): {state.ticks / 60}"),
        (Highlight.Text, (state) => $"Elapsed Time: {state.totalElapsedTime}"),
        (Highlight.Text, (state) => $"Seconds (from time): {state.totalElapsedTime.Seconds}"),
        (Highlight.Text, (state) => FormatIEnumerable("Keycodes: ", state.Input.HasInputThisTick(), state.Input.Keycodes())),
        (Highlight.Text, (state) => FormatIEnumerable("Keynames: ", state.Input.HasInputThisTick(), state.Input.Keynames())),
    ];

    public DebugPopup() : base("Debug", Highlight.Text, 2, 2, WIDTH, segments.Length + 2) { }

    public override bool RunTick(Game state) => true;

    public override void Render(Game state) {
        Panel.DrawBorder(Highlight.Text);

        (Highlight h, int x, int y, string)[] processedSegments = segments.Select((s, i) => (s.h, 1, i + 1, s.f(state))).ToArray();

        Panel.AddLinesWithHighlight(processedSegments);
    }
}
