using System.Diagnostics;
using Blackguard.UI.Scenes;

namespace Blackguard.Tests;

public class GameTests {
    private class TestScene : Scene {

        public override void Finish() {
            return;
        }

        public override void Render() {
            return;
        }

        int tick = 0;
        public override bool RunTick() {
            tick++;

            // Ensures that the loop runs for 10 seconds
            if (tick >= 600) {
                return false;
            }

            return true;
        }
    }

    private Scene scene = new TestScene();

    [Test]
    public void TestMainLoop() {
        Game game = new Game();
        game.SwitchScene(scene);

        Stopwatch stopwatch = Stopwatch.StartNew();
        game.Run();
        stopwatch.Stop();

        // A tenth of a second variance over 10 seconds is good enough. Especially on school computers
        Assert.That(stopwatch.ElapsedMilliseconds, Is.AtLeast(9900).And.AtMost(10100));
    }
}