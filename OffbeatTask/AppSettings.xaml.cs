using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OffbeatTask
{
    public partial class AppSettings : Window
    {
        public MyMainWindow mainWindow;
        public Reward[] rewards = new Reward[8];
        public int LastSelectedRowIndex = 0;

        public AppSettings(MyMainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        private void MyAppSettings_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindow.context.Rewards.OrderBy(x => x.Posetion).ToArray().CopyTo(rewards, 0);
            MyGrid.ItemsSource = rewards;
            MyGrid.CurrentCellChanged += MyGrid_CurrentCellChanged;
        }


        private void MyGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (HasError())
                SaveBtn.IsEnabled = DownBtn.IsEnabled = UpBtn.IsEnabled = false;
            else
                SaveBtn.IsEnabled = DownBtn.IsEnabled = UpBtn.IsEnabled = true;

            if (!HasFocus(MyGrid, true))
                return;

            LastSelectedRowIndex = MyGrid.Items.IndexOf(MyGrid.CurrentItem);
        }

        public bool HasFocus(Control aControl, bool aCheckChildren)
        {
            var oFocused = System.Windows.Input.FocusManager.GetFocusedElement(this) as DependencyObject;
            if (!aCheckChildren)
                return oFocused == aControl;
            while (oFocused != null)
            {
                if (oFocused == aControl)
                    return true;
                oFocused = System.Windows.Media.VisualTreeHelper.GetParent(oFocused);
            }
            return false;
        }
        public bool HasError()
        {
            return (from c in
                   (from object i in MyGrid.ItemsSource
                    select MyGrid.ItemContainerGenerator.ContainerFromItem(i))
                    where c != null
                    select Validation.GetHasError(c))
              .FirstOrDefault(x => x);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpBtn_Click(object sender, RoutedEventArgs e)
        {
            MyGrid.Focus();
            UP_Down(true);
        }

        private void DownBtn_Click(object sender, RoutedEventArgs e)
        {
            MyGrid.Focus();
            UP_Down(false);
        }

        public void UP_Down(bool Up)
        {
            var res = rewards.OrderBy(x => x.Posetion).ToArray();
            if (Up)
            {
                if (LastSelectedRowIndex == 0)
                    return;
                res[LastSelectedRowIndex - 1].Posetion++;
                res[LastSelectedRowIndex].Posetion--;
                LastSelectedRowIndex--;
            }
            else
            {
                if (LastSelectedRowIndex == 7)
                    return;
                res[LastSelectedRowIndex].Posetion++;
                res[LastSelectedRowIndex + 1].Posetion--;
                LastSelectedRowIndex++;
            }

            MyGrid.Items.SortDescriptions.Clear();
            MyGrid.Items.SortDescriptions.Add(new SortDescription("Posetion", ListSortDirection.Ascending));
            MyGrid.Items.Refresh();
            MyGrid.UpdateLayout();
            MyGrid.SelectedIndex = LastSelectedRowIndex;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.context.SaveChanges();
            this.DialogResult = true;
        }

        private void MyAppSettings_Closing(object sender, CancelEventArgs e)
        {
            RollBack();
        }

        public void RollBack()
        {
            var context = mainWindow.context;
            var changedEntries = context.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        private void MyAppSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            else if (e.Key == Key.S && (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl)))
            {
                SaveBtn_Click(sender, new RoutedEventArgs());
            }
        }
    }
}
