using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace OffbeatTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly System.Timers.Timer MyTimer = new System.Timers.Timer(1);
        public int CurrentWheelPint, RotateCount, CurrentRotateNo, StopPoint;

        public MainWindow()
        {
            InitializeComponent();

            MyTimer.AutoReset = true;
            MyTimer.Enabled = false;
            MyTimer.Elapsed += new System.Timers.ElapsedEventHandler(MyTimer_Elapsed);
        }

        private void MyTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke((() =>
            {
                if (CurrentRotateNo == RotateCount && CurrentWheelPint == StopPoint)
                {
                    MyTimer.Stop();
                    MessageBox.Show(this, "Congratulations! You won " + GetRewardName() + "!", "You won!", MessageBoxButton.OK, MessageBoxImage.Information);
                    StartBtn.IsEnabled = true;
                    return;
                }

                if (CurrentWheelPint < 360)
                {
                    CurrentWheelPint++;
                }
                else
                {
                    CurrentRotateNo++;
                    CurrentWheelPint = 0;
                }

                int CurrentAngle = CurrentWheelPint;
                WheelTrans.Angle = MyBlender.Angle = CurrentAngle;
                CurrentAngle += 45;
                MyCards.Angle = CurrentAngle;
                CurrentAngle += 45;
                MyFlutes.Angle = CurrentAngle;
                CurrentAngle += 45;
                MyIron.Angle = CurrentAngle;
                CurrentAngle += 45;
                MyStove.Angle = CurrentAngle;
                CurrentAngle += 45;
                MyTV.Angle = CurrentAngle;
                CurrentAngle += 45;
                MyTwix.Angle = CurrentAngle;
                CurrentAngle += 45;
                MyUSB.Angle = CurrentAngle;
            }));
        }

        public string GetRewardName()
        {
            if (StopPoint <= 23)
                return "Blender";
            else if (StopPoint <= 68)
                return "USB";
            else if (StopPoint <= 113)
                return "Twix";
            else if (StopPoint <= 158)
                return "TV";
            else if (StopPoint <= 203)
                return "Stove";
            else if (StopPoint <= 248)
                return "Iron";
            else if (StopPoint <= 293)
                return "Flutes";
            else if (StopPoint <= 338)
                return "Cards";
            else
                return "Blender";
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = false;

            RotateCount = new Random().Next(1, 10);
            StopPoint = new Random().Next(1, 360);
            CurrentRotateNo = CurrentWheelPint = 0;

            MyTimer.Start();
        }
    }
}
