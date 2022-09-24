using Microsoft.Xaml.Behaviors;
using MoveMouseProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MoveMouseProject.CustomBehavior
{
    public class CustomGridBehavior:Behavior<CustomGrid>
    {
        Point currentMousePosOnGrid;
        Point StartPos;
        bool IsInDrag = false;
        bool IsFirstClick = true;
        TranslateTransform transform = new TranslateTransform();
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsInDrag)
            {
                CustomGrid customGrid = sender as CustomGrid;
                Grid parent = (Grid)VisualTreeHelper.GetParent(customGrid);
                var MousePos = Mouse.GetPosition(parent);
                transform.X = MousePos.X - currentMousePosOnGrid.X - StartPos.X;
                transform.Y = MousePos.Y - currentMousePosOnGrid.Y - StartPos.Y;
                customGrid.RenderTransform = transform;
                
            }
        }

        private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsInDrag)
            {
                CustomGrid customGrid = sender as CustomGrid;
                customGrid?.ReleaseMouseCapture();
                IsInDrag = false;   
            }
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsInDrag = true;
                if (IsInDrag)
                {
                    CustomGrid customGrid = sender as CustomGrid;
                    currentMousePosOnGrid = e.GetPosition(customGrid);
                    StartPos = IsFirstClick? transform.Transform(new Point(0, 0)) : new Point(0,0);
                    IsFirstClick = false;
                    customGrid?.CaptureMouse();
                }
            }
        }
    }
}
