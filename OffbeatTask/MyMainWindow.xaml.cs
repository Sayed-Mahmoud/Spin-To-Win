using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    public partial class MyMainWindow : Window
    {
        public SpinToWinEntities context;
        private readonly Random random = new Random();

        public MyMainWindow()
        {
            InitializeComponent();
        }

        private void MyMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var create = new System.Data.Entity.CreateDatabaseIfNotExists<SpinToWinEntities>();
            System.Data.Entity.Database.SetInitializer(create);
            context = new SpinToWinEntities();
            //context.Database.Initialize(true);

            context.Rewards.Load();

            if (context.Rewards.Count() == 0)
            {
                context.Rewards.Add(new Reward() { ItemName = "Blender", Posetion = 1, Quantity = 1 });
                context.Rewards.Add(new Reward() { ItemName = "USB", Posetion = 2, Quantity = 1 });
                context.Rewards.Add(new Reward() { ItemName = "Twix", Posetion = 3, Quantity = 1 });
                context.Rewards.Add(new Reward() { ItemName = "TV", Posetion = 4, Quantity = 1 });
                context.Rewards.Add(new Reward() { ItemName = "Stove", Posetion = 5, Quantity = 1 });
                context.Rewards.Add(new Reward() { ItemName = "Iron", Posetion = 6, Quantity = 1 });
                context.Rewards.Add(new Reward() { ItemName = "Flutes", Posetion = 7, Quantity = 1 });
                context.Rewards.Add(new Reward() { ItemName = "Cards", Posetion = 8, Quantity = 1 });
                context.SaveChanges();
            }

            SetRewards();
        }

        #region MenuItems

        private void Exit_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            this.Close();
        }
        private void Settings_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var appSettings = new AppSettings(this)
            {
                Icon = this.Icon
            };
            var win = appSettings.ShowDialog();
            if (win != null && win == true)
            {
                SetRewards();
            }
        }

        #endregion

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!StartBtn.IsEnabled)
            {
                e.Cancel = true;
                return;
            }

            if (MessageBox.Show("Are you sure! you want to exit?", "Exit Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        public Reward GetReward(int StopPoint)
        {
            var rewards = context.Rewards.OrderBy(x => x.Posetion).ToArray();
            if (StopPoint <= 23)
                return rewards[0];
            else if (StopPoint <= 68)
                return rewards[7];
            else if (StopPoint <= 113)
                return rewards[6];
            else if (StopPoint <= 158)
                return rewards[5];
            else if (StopPoint <= 203)
                return rewards[4];
            else if (StopPoint <= 248)
                return rewards[3];
            else if (StopPoint <= 293)
                return rewards[2];
            else if (StopPoint <= 338)
                return rewards[1];
            else
                return rewards[0];
        }
        public void GetRewardPosetions(Reward reward, out int Min, out int Max)
        {
            switch (reward.Posetion)
            {
                case 1:
                    {
                        Min = 23;
                        Max = 360;
                        break;
                    }
                case 8:
                    {
                        Min = 24;
                        Max = 68;
                        break;
                    }
                case 7:
                    {
                        Min = 69;
                        Max = 113;
                        break;
                    }
                case 6:
                    {
                        Min = 114;
                        Max = 158;
                        break;
                    }
                case 5:
                    {
                        Min = 159;
                        Max = 203;
                        break;
                    }
                case 4:
                    {
                        Min = 203;
                        Max = 248;
                        break;
                    }
                case 3:
                    {
                        Min = 249;
                        Max = 293;
                        break;
                    }
                case 2:
                    {
                        Min = 294;
                        Max = 338;
                        break;
                    }
                default:
                    {
                        Min = 0;
                        Max = 360;
                        break;
                    }
            }
        }
        public void SetRewards()
        {
            var rewards = context.Rewards.OrderBy(x => x.Posetion).ToArray();
            string FolderName = "Resources/";
            string ImageFormat = ".png";
            ImageReward1.Source = new BitmapImage(new Uri(FolderName + rewards[0].ItemName + ImageFormat, UriKind.RelativeOrAbsolute));
            ImageReward2.Source = new BitmapImage(new Uri(FolderName + rewards[1].ItemName + ImageFormat, UriKind.RelativeOrAbsolute));
            ImageReward3.Source = new BitmapImage(new Uri(FolderName + rewards[2].ItemName + ImageFormat, UriKind.RelativeOrAbsolute));
            ImageReward4.Source = new BitmapImage(new Uri(FolderName + rewards[3].ItemName + ImageFormat, UriKind.RelativeOrAbsolute));
            ImageReward5.Source = new BitmapImage(new Uri(FolderName + rewards[4].ItemName + ImageFormat, UriKind.RelativeOrAbsolute));
            ImageReward6.Source = new BitmapImage(new Uri(FolderName + rewards[5].ItemName + ImageFormat, UriKind.RelativeOrAbsolute));
            ImageReward7.Source = new BitmapImage(new Uri(FolderName + rewards[6].ItemName + ImageFormat, UriKind.RelativeOrAbsolute));
            ImageReward8.Source = new BitmapImage(new Uri(FolderName + rewards[7].ItemName + ImageFormat, UriKind.RelativeOrAbsolute));
            this.UpdateLayout();
        }

        public Reward GetRwardByChance(List<Reward> items)
        {
            int poolSize = 0;
            for (int i = 0; i < items.Count; i++)
            {
                poolSize += items[i].Quantity;
            }

            int randomNumber = random.Next(0, poolSize) + 1;

            int accumulatedProbability = 0;
            for (int i = 0; i < items.Count; i++)
            {
                accumulatedProbability += items[i].Quantity;
                if (randomNumber <= accumulatedProbability && items[i].Quantity > 0)
                    return items[i];
            }
            return GetRwardByChance(items);
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (context.Rewards.Max(x => x.Quantity) <= 0)
            {
                MessageBox.Show(this, "Sorry! there are no remaining rewards.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int CurrentWheelPint, RotateCount, StopPoint = 0;
            SettingsBtn.IsEnabled = StartBtn.IsEnabled = false;
            RotateCount = random.Next(1, 50);

            Reward reward = null;
            while (reward == null)
            {
                reward = GetRwardByChance(context.Rewards.ToList());
                GetRewardPosetions(reward, out int Min, out int Max);
                if (reward.Posetion == 1)
                {
                    StopPoint = random.Next(random.Next(0, Min) + 1, random.Next(339, Max) + 1) + 1;
                    while (this.GetReward(StopPoint) != reward)
                    {
                        StopPoint = random.Next(random.Next(0, Min) + 1, random.Next(339, Max) + 1) + 1;
                    }
                }
                else
                    StopPoint = random.Next(Min, Max) + 1;
            }

            reward.Quantity--;
            context.SaveChanges();

            CurrentWheelPint = 0;
            int StopTotalPoints = (360 * RotateCount) + StopPoint + RotateCount;

            for (int i = 0; i < StopTotalPoints; i++)
            {
                var par = Dispatcher.Invoke((Action<int>)((CurI) =>
                {
                    if (CurrentWheelPint < 360)
                    {
                        CurrentWheelPint++;
                    }
                    else
                    {
                        //CurrentRotateNo++;
                        CurrentWheelPint = 0;
                    }

                    int CurrentAngle = CurrentWheelPint;
                    WheelTrans.Angle = Reward1.Angle = CurrentAngle;
                    CurrentAngle += 45;
                    Reward2.Angle = CurrentAngle;
                    CurrentAngle += 45;
                    Reward3.Angle = CurrentAngle;
                    CurrentAngle += 45;
                    Reward4.Angle = CurrentAngle;
                    CurrentAngle += 45;
                    Reward5.Angle = CurrentAngle;
                    CurrentAngle += 45;
                    Reward6.Angle = CurrentAngle;
                    CurrentAngle += 45;
                    Reward7.Angle = CurrentAngle;
                    CurrentAngle += 45;
                    Reward8.Angle = CurrentAngle;
                    this.UpdateLayout();
                }), DispatcherPriority.Background, i);//Background ContextIdle

                /*
                var task = Task.Factory.StartNew(() =>//o =>
                {
                    
                    //Thread.Sleep(10);
                });
                */
            }

            this.UpdateLayout();
            SettingsBtn.IsEnabled = StartBtn.IsEnabled = true;
            MessageBox.Show(this, "Congratulations! You won " + reward.ItemName + "!", "You won!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}