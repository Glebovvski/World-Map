using System;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.Generic;

namespace WpfApplication5
{
    public partial class MainWindow : Window
    {
        DbMapEntities map = new DbMapEntities();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            foreach (Control item in info.Children)
            {
                if (item is TextBox) (item as TextBox).IsReadOnly = true;
            }

            foreach (UIElement item in world.Children)
            {
                if (item is Path)
                {
                    ((Path)item).MouseEnter += country_MouseEnter;
                    ((Path)item).MouseLeave += country_MouseLeave;
                }
            }
        }
        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            textBoxTime.Text= DateTime.UtcNow.AddHours(offset()).ToString();
        }
        public int offset()
        {
            var areas = from n in map.MapTables select new { n.CountryName, n.Time };
            foreach (var country in areas)
            {
                if (country.CountryName.Contains(textBoxName.Text.ToLower()))
                {
                    return country.Time;
                }
            }
            return 0;
        }
        void CountryInfo(object sender, MouseEventArgs e)
        {
            var areas = from n in map.MapTables.ToList() select new { n.CountryName, n.Area, n.Citizens, n.Capital, n.Time };
            foreach (var country in areas)
            {
                if (country.CountryName.Contains(((Path)sender).Name.ToLower()))
                {
                    textBoxArea.Text = country.Area;
                    textBoxPop.Text = country.Citizens;
                    textBoxCapital.Text = country.Capital;
                }
            }
        }
        
        private void country_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Path)sender).Fill = Brushes.Green;
            CountryInfo(sender, e);
            textBoxName.Text = ((Path)sender).Name.ToString().Replace("_"," ");
        }
       
        private void country_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Path)sender).Fill = Brushes.Black;
            foreach(Control item in info.Children)
            {
                if (item is TextBox) (item as TextBox).Text = string.Empty;
                if (item.Name=="TextBoxTime") continue;
            }
        }
    }
}
