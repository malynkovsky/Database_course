using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DB_Store
{
    class Animation
    {
        public void MoveLoginWindow(LoginWindow window)
        {
            DoubleAnimation da = new DoubleAnimation(SystemParameters.PrimaryScreenWidth, (SystemParameters.PrimaryScreenWidth - window.Width) / 2, TimeSpan.FromMilliseconds(300));
            window.BeginAnimation(Canvas.LeftProperty, da);
        }

        public void MoveMainWindow(MainWindow window)
        {
            DoubleAnimation da = new DoubleAnimation(SystemParameters.PrimaryScreenWidth, (SystemParameters.PrimaryScreenWidth - window.Width) / 2, TimeSpan.FromMilliseconds(300));
            window.BeginAnimation(Canvas.LeftProperty, da);
        }

        public void MoveNewClientWindow(NewClientWindow window)
        {
            DoubleAnimation da = new DoubleAnimation(SystemParameters.PrimaryScreenWidth, (SystemParameters.PrimaryScreenWidth - window.Width) / 2, TimeSpan.FromMilliseconds(300));
            window.BeginAnimation(Canvas.LeftProperty, da);
        }
    }
}