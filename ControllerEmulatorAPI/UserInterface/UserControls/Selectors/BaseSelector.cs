using System.Windows.Controls;

namespace ControllerEmulatorAPI.UserInterface.UserControls.Selectors
{
    public class BaseSelector : Grid
    {
        public BaseSelector()
        {
            SetResourceReference(BackgroundProperty, "BackgroundSelectors");
            MouseEnter += MainGrid_MouseEnter;
            MouseLeave += MainGrid_MouseLeave;
        }

        private void MainGrid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SetResourceReference(BackgroundProperty, "BackgroundSelectors");
        }

        private void MainGrid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SetResourceReference(BackgroundProperty, "HoverSelectors");
        }
    }
}