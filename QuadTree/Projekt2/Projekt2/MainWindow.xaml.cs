using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Projekt2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Segment hlavnySegment;
        List<Ellipse> zoznamBodov = new List<Ellipse>();
        Rectangle obdlznik = new Rectangle();
        Point bodZoZoznamu = new Point();

        Point suradnicaPrvehoBodu = new Point();
        Point suradnicaDruhehoBodu = new Point();

        public MainWindow()
        {
            InitializeComponent();
            //vykresli prvy hlavny segment
            hlavnySegment = new Segment(canvas, canvas.Width, canvas.Height);
        }

        private void VykresliBod(double x, double y)
        {
            Ellipse kruh = new Ellipse();
            kruh.Fill = new SolidColorBrush(Colors.Red);
            kruh.Width = 5;
            kruh.Height = 5;
            Canvas.SetLeft(kruh, x - (5 / 2.0));
            Canvas.SetTop(kruh, y - (5 / 2.0));
            canvas.Children.Add(kruh);
            zoznamBodov.Add(kruh);
        }

        //ak bolo kliknute na bod ktory je v canvase vrati cestu k bodu
        private void PriradBoduString(Point bodStlacenia)
        {
            for (int i = 0; i < zoznamBodov.Count; i++)
            {
                Point bod = new Point(Canvas.GetLeft(zoznamBodov[i]) + (5 / 2.0), Canvas.GetTop(zoznamBodov[i]) + (5 / 2.0));
                if (PrvyBodPatriDoOkoliaDruhehoBodu(bodStlacenia, bod))
                {
                    ZobrazCestuKBodu(bod);
                    zoznamBodov[i].Fill = Brushes.Orange;
                    return;
                }
            }
            Cesta.Content = "Cesta k vybranemu bodu";
        }

        private bool PrvyBodPatriDoOkoliaDruhehoBodu(Point prvyBod, Point druhyBod)
        {
            int tolerancia = 7;
            if (druhyBod.X - tolerancia < prvyBod.X && druhyBod.X + tolerancia > prvyBod.X &&
                druhyBod.Y - tolerancia < prvyBod.Y && druhyBod.Y + tolerancia > prvyBod.Y)
            {
                return true;
            }
            return false;
        }

        private void ZobrazCestuKBodu(Point bod)
        {
            Cesta.Content = hlavnySegment.DajMiCestu(bod, "koren") + "- BOD";
        }

        private void ZmenFarbuBodovNaPovodnuFarbu()
        {
            for (int i = 0; i < zoznamBodov.Count; i++)
            {
                if (zoznamBodov[i].Fill == Brushes.Orange || zoznamBodov[i].Fill == Brushes.Blue)
                {
                    zoznamBodov[i].Fill = Brushes.Red;
                }
            }
        }

        private void ZmazSa()
        {
            for (int i = 0; i < zoznamBodov.Count; i++)
            {
                canvas.Children.Remove(zoznamBodov[i]);
            }
            zoznamBodov.Clear();
            canvas.Children.Remove(obdlznik);
            hlavnySegment.ZmazSa();
            hlavnySegment = new Segment(canvas, canvas.Width, canvas.Height);
            Cesta.Content = "Cesta k vybranemu bodu";
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ZmazSa();
        }

        /// <summary>
        /// tlacidlo Generuj nahodne body
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ZmazSa();
            Random random = new Random();
            for (int i = 1; i <= 100; i++)
            {
                double surX = Convert.ToInt32(random.Next(1, Convert.ToInt32(canvas.Width)));
                double surY = Convert.ToInt32(random.Next(1, Convert.ToInt32(canvas.Height)));

                //prida bod s vypocitanymi nahodnymi suradnicami
                Point bod = new Point(surX, surY);

                //ak je nejaky bod prilis blizko nejakeho uz vykresleneho tak ho nevykresli (aby segmenty neboli prilis male)
                if(SuDvaBodyPrilisBlizko(bod))
                {
                    // vrati i o -1 aby bol pocet vykreslenych bodov stale 100 
                    i--;
                    continue; ;
                }

                VykresliBod(surX, surY);
                //nad kazdym pridanym bodov sa aktualizuje rozdelnie canvasu
                hlavnySegment.PridajBod(bod);
            }
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            canvas.Children.Remove(obdlznik);
            ZmenFarbuBodovNaPovodnuFarbu();
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //ak je nejaky bod prilis blizko nejakeho uz vykresleneho tak ho nevykresli (aby segmenty neboli prilis male)
                if (SuDvaBodyPrilisBlizko(e.GetPosition(canvas)))
                {
                    return;
                }
                //vykresli prislusny bod
                VykresliBod(e.GetPosition(canvas).X, e.GetPosition(canvas).Y);
                //prerozdeli canvas a priradi bod zodpovedajucemu segmentu/podsegmentu
                hlavnySegment.PridajBod(e.GetPosition(canvas));
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                PriradBoduString(e.GetPosition(canvas));
            }
            else if (e.MiddleButton == MouseButtonState.Pressed)
            {
                //zapamata si prvy bod obdlznika
                suradnicaPrvehoBodu = e.GetPosition(canvas);
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                Cesta.Content = "Cesta k vybranemu bodu";
                suradnicaDruhehoBodu = e.GetPosition(canvas);

                //urcenie laveho horneho a praveho dolneho rohu (koli kresleniu v roznych smeroch)
                double lavyHornyRohX = Math.Min(suradnicaPrvehoBodu.X, suradnicaDruhehoBodu.X);
                double lavyHornyRohY = Math.Min(suradnicaPrvehoBodu.Y, suradnicaDruhehoBodu.Y);
                double pravyDolnyRohX = Math.Max(suradnicaPrvehoBodu.X, suradnicaDruhehoBodu.X);
                double pravyDolnyRohY = Math.Max(suradnicaPrvehoBodu.Y, suradnicaDruhehoBodu.Y);

                double surHornehoRohuX = lavyHornyRohX;
                double surHornehoRohuY = lavyHornyRohY;

                obdlznik.Stroke = Brushes.Black;
                obdlznik.Fill = Brushes.Aqua;
                obdlznik.Opacity = 0.5;
                obdlznik.StrokeThickness = 0.5;
                //kazdym pohnutim mysi sa prepocita sirka a vyska rectangla
                obdlznik.Width = VypocitajSirkuVyskuObdlznika(suradnicaPrvehoBodu.X, suradnicaDruhehoBodu.X);
                obdlznik.Height = VypocitajSirkuVyskuObdlznika(suradnicaPrvehoBodu.Y, suradnicaDruhehoBodu.Y);
                Canvas.SetLeft(obdlznik, surHornehoRohuX);
                Canvas.SetTop(obdlznik, surHornehoRohuY);

                //stary obdlznik vymaze a prida aktualny
                canvas.Children.Remove(obdlznik);
                canvas.Children.Add(obdlznik);

                ZmenFarbuBodovNaPovodnuFarbu();

                //ak nejaky bod lezi v obdlzniku vyfarbi ho na modro
                for (int i = 0; i < zoznamBodov.Count; i++)
                {
                    bodZoZoznamu.X = Canvas.GetLeft(zoznamBodov[i]) + (5 / 2.0);
                    bodZoZoznamu.Y = Canvas.GetTop(zoznamBodov[i]) + (5 / 2.0);
                    if (LeziaBodyVObdlzniku(bodZoZoznamu))
                    {
                        zoznamBodov[i].Fill = Brushes.Blue;
                    }
                }
            }
        }

        private bool SuDvaBodyPrilisBlizko(Point bodStlacenia)
        {
            for (int i = 0; i < zoznamBodov.Count; i++)
            {
                Point bod = new Point(Canvas.GetLeft(zoznamBodov[i]) + (5 / 2.0), Canvas.GetTop(zoznamBodov[i]) + (5 / 2.0));
                if (PrvyBodPatriDoOkoliaDruhehoBodu(bodStlacenia, bod))
                {
                    return true;
                }
            }
            return false;
        }

        private double VypocitajSirkuVyskuObdlznika(double prvyBod, double druhybod)
        {
            return Math.Abs(prvyBod - druhybod);
        }

        private bool LeziaBodyVObdlzniku(Point bod)
        {
            double surX = Canvas.GetLeft(obdlznik);
            double surY = Canvas.GetTop(obdlznik);
            double surXPlusSirka = surX + obdlznik.Width;
            double surYPlusVyska = surY + obdlznik.Height;

            return surX <= bod.X && bod.X < surXPlusSirka && surY <= bod.Y && bod.Y < surYPlusVyska;
        }
    }
}
