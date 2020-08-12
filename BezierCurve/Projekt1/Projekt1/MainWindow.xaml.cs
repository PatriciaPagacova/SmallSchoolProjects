using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int indexTahanehoBodu;
        double parameterT = 0;
        int pocitadlo = 0;
        //stav vykreslenia Castelju algoritmu
        bool stavVykreslenia = false;
        //int m = 2;

        // zoznam referencnych bodov
        List<Point> body = new List<Point>();

        //zoznamy ktore sa pouzivaju pre ratanie Subdivision
        List<Point> bodyPrePrerozdelenieLave = new List<Point>();
        List<Point> bodyPrePrerozdeleniePrave = new List<Point>();

        // zoznam vsetkych vykreslenych bodov a usecok
        List<UIElement> vykresleneObjekty = new List<UIElement>();

        public MainWindow()
        {
            InitializeComponent();
            ZrusTahanieBodu();
        }

        private void VykresliBody(List<Point> bodyNaVykreslenie, Color farba, double priemer)
        {
            for (int i = 0; i < bodyNaVykreslenie.Count; i++)
            {
                Ellipse kruh = new Ellipse();
                kruh.Fill = new SolidColorBrush(farba);
                kruh.Width = priemer;
                kruh.Height = priemer;
                Canvas.SetLeft(kruh, bodyNaVykreslenie[i].X - priemer / 2);
                Canvas.SetTop(kruh, bodyNaVykreslenie[i].Y - priemer / 2);
                VykresliElement(kruh);
            }
        }

        private void VykresliUsecky(List<Point> bodyNaVykreslenie, Color farba, double hrubka)
        {
            for (int i = 0; i < bodyNaVykreslenie.Count - 1; i++) //- 1 lebo mame i+1 
            {
                Line usecka = new Line();
                usecka.Stroke = new SolidColorBrush(farba);
                usecka.StrokeThickness = hrubka;
                usecka.X1 = bodyNaVykreslenie[i].X;
                usecka.Y1 = bodyNaVykreslenie[i].Y;
                usecka.X2 = bodyNaVykreslenie[i + 1].X;
                usecka.Y2 = bodyNaVykreslenie[i + 1].Y;
                Canvas.SetZIndex(usecka, 6);
                VykresliElement(usecka);
            }
        }

        private void VykresliElement(UIElement element)
        {
            vykresleneObjekty.Add(element);     //prida element do List<UIElement> vykresleneObjekty
            canvas.Children.Add(element);

            // pri staceni a pustieni mysi priamo na elemente(kruh,usecka) tieto funkcie na canvase neboli zavolane
            element.MouseDown += canvas_MouseDown; //pre kazdy element(kruh,usecka...) sa zavola funkcia Canvas_MouseDown
            element.MouseUp += canvas_MouseUp;     //pre kazdy element(kruh,usecka...) sa zavola funkcia Canvas_MouseUp
        }

        private void ZmazBod(Point bodStlacenia)
        {
            List<Point> kopiaBodov = body.ToList();
            body.Clear();
            //tymto cyklom sa znova pridaju body do povodneho zoznamu s tym ze sa vymazany bod preskoci
            for (int i = 0; i < kopiaBodov.Count; i++)
            {
                if (PrvyBodPatriDoOkoliaDruhehoBodu(bodStlacenia, kopiaBodov[i]))
                {
                    continue;
                }
                body.Add(kopiaBodov[i]);
            }
            PrekresliVsetko();
        }

        private void NastavIndexTahanehoBodu(Point bodStlacenia)
        {
            for (int i = 0; i < body.Count; i++)
            {
                if (PrvyBodPatriDoOkoliaDruhehoBodu(bodStlacenia, body[i]))
                {
                    indexTahanehoBodu = i;
                    return;
                }
            }
            ZrusTahanieBodu();
        }

        private bool PrvyBodPatriDoOkoliaDruhehoBodu(Point prvyBod, Point druhyBod)
        {
            int tolerancia = 15;
            if (druhyBod.X - tolerancia < prvyBod.X && druhyBod.X + tolerancia > prvyBod.X &&
                druhyBod.Y - tolerancia < prvyBod.Y && druhyBod.Y + tolerancia > prvyBod.Y)
            {
                return true;
            }
            return false;
        }

        private void ZrusTahanieBodu()
        {
            indexTahanehoBodu = -1;
        }

        private void PrekresliVsetko()
        {
            ZmazVsetkyVykresleneObjekty();
            VykresliUsecky(body, Colors.Black, 1);

            if (body.Count > 1)
            {
                if (RBPriamyVypocet.IsChecked == true)
                {
                    VykresliPriamyVypocet();
                }
                else if (RBZvysovanieStupna.IsChecked == true)
                {
                    VykresliZvysovanieStupna();
                }
                else if (RBPrerozdelenie.IsChecked == true)
                {
                    VykresliSubdivision(PocetVzorkovacichBodov(HlbkaM));
                }
                if (stavVykreslenia == true)
                {
                    VykreskliCasteljauAlgoritmus();
                }
            }
            VykresliBody(body, Colors.Red, 4);
        }

        private void ZmazVsetkyVykresleneObjekty()
        {
            for (int i = 0; i < vykresleneObjekty.Count; i++)
            {
                canvas.Children.Remove(vykresleneObjekty[i]);
            }
            vykresleneObjekty.Clear();
        }

        private void VykresliSubdivision(int maxHlbka)
        {
            //ak je hlbka 0, vykreslia sa len usecky medzi riadiacimi bodmi a dalej sa uz nevnarame
            if (maxHlbka == 0)
            {
                VykresliUsecky(body, Colors.Purple, 2);
                return;
            }
            //vrati konecky zoznam bodov na vykreslenie
            List<Point> zoznamNaVykreslenie = Subdivision(body, 1, maxHlbka);
            VykresliUsecky(zoznamNaVykreslenie, Colors.Purple, 2);
        }

        //metoda ktora vracia konecny zoznam bodov pre vykreslenie
        private List<Point> Subdivision(List<Point> zoznam, int aktualnaHlbka, int maxHlbka)
        {
            //vyrata sa pravy a lavy zoznam (zoznam prvych a poslednych bodov s casteljua)
            List<Point> lavyZoznam = ZoznamPreSubdivision(zoznam, 0.5, 1).ToList();
            bodyPrePrerozdelenieLave.Clear();
            bodyPrePrerozdeleniePrave.Clear();
            List<Point> pravyZoznam = ZoznamPreSubdivision(zoznam, 0.5, 2).ToList();
            bodyPrePrerozdelenieLave.Clear();
            bodyPrePrerozdeleniePrave.Clear();

            //ak uz sa viac netreba zanarat vrati spojeny zoznam lavych a pravych bodov
            if (aktualnaHlbka == maxHlbka) //maxHlbka ---> v zadani nazvana "m"
            {
                return SpojZoznam(pravyZoznam, lavyZoznam);
            }

            //inak sa vnorime najprv s lavym a potom s pravym 
            List<Point> vnorenyLavy = Subdivision(lavyZoznam, aktualnaHlbka + 1, maxHlbka);
            List<Point> vnorenyPravy = Subdivision(pravyZoznam, aktualnaHlbka + 1, maxHlbka);

            //ak uz su listy vnorenyLavy a vnorenyPravy naplnene znamena to ze sme sa uz z rekurzie vynorili a ze mame uz konecne body ktore treba len spojit
            return SpojZoznam(vnorenyPravy, vnorenyLavy);
        }

        private List<Point> ZoznamPreSubdivision(List<Point> predchadzajuciRiadok, double parameterT, int pravylavy)
        {
            //ak je v predchadzajucom riadku iba jeden bod znamena to ze sme na konci 
            if (predchadzajuciRiadok.Count == 1)
            {
                // tu sa prida este posledny bod (beyierov bod) do zoznamov
                bodyPrePrerozdelenieLave.Add(predchadzajuciRiadok[0]);
                bodyPrePrerozdeleniePrave.Add(predchadzajuciRiadok[0]);

                // podla toho ci chceme lavy alebo pravy zoznam(zoznam prvych alebo poslednych bodov z riadkov) vrati prislusny zoznam
                if (pravylavy == 1) // 1 = lavy , 2 = pravy
                {
                    return bodyPrePrerozdelenieLave;
                }
                else
                {
                    return bodyPrePrerozdeleniePrave;
                }
            }

            List<Point> aktualnyRiadok = new List<Point>();

            //vyrata sa dalsi riadok casteljuovho algoritmu a prida sa postupne do aktualneho riadku
            for (int dolnyIndex = 0; dolnyIndex < predchadzajuciRiadok.Count - 1; dolnyIndex++)
            {
                Point bod = new Point();
                bod.X = (1 - parameterT) * predchadzajuciRiadok[dolnyIndex].X + (parameterT) * predchadzajuciRiadok[dolnyIndex + 1].X;
                bod.Y = (1 - parameterT) * predchadzajuciRiadok[dolnyIndex].Y + (parameterT) * predchadzajuciRiadok[dolnyIndex + 1].Y;
                aktualnyRiadok.Add(bod);
            }

            //do zoznamov sa postupne pridavaju prvy a posledny bod riadku
            bodyPrePrerozdelenieLave.Add(predchadzajuciRiadok[0]);
            bodyPrePrerozdeleniePrave.Add(predchadzajuciRiadok[predchadzajuciRiadok.Count - 1]);

            return ZoznamPreSubdivision(aktualnyRiadok, parameterT, pravylavy);
        }

        // spoji dva zoznami, ale pravy od konca ku zaciatku lebo sme ho naplnali opacnym smerom
        private List<Point> SpojZoznam(List<Point> pravy, List<Point> lavy)
        {
            List<Point> spojenyZoznam = lavy.ToList();

            //-2 lebo posledny bod(bezierov bod) by tam bol dva krat
            for (int i = pravy.Count - 2; i >= 0; i--)
            {
                spojenyZoznam.Add(pravy[i]);
            }
            return spojenyZoznam;
        }

        private void VykresliPriamyVypocet()
        {
            List<Point> bezieroveBody = new List<Point>();
            int pocetCyklov = PocetVzorkovacichBodov(PocetLOD) - 1;
            double parameterS = 0;
            // postupne sa vykreslia vsetky vyratane body na int [0,1] pomocou casteljua
            for (int j = 0; j < pocetCyklov; j++)
            {
                //vyrata sa bod a prida sa do zoznamu na vykreslenie
                Point bodBeziera = BodyPreCasteljau(body, parameterS);
                bezieroveBody.Add(bodBeziera);
                parameterS = parameterS + 1.0 / (double)pocetCyklov;
            }
            //prida sa posledny bod 
            bezieroveBody.Add(body[body.Count - 1]);
            //VykresliBody(bezieroveBody);
            VykresliUsecky(bezieroveBody, Colors.Orange, 2);
        }

        //vrati bod (bezierov bod) pomocou castelju algoritmu
        private Point BodyPreCasteljau(List<Point> predchadzajuciRiadok, double parameterT)
        {
            if (predchadzajuciRiadok.Count == 1)
            {
                return predchadzajuciRiadok[0];
            }

            List<Point> aktualnyRiadok = new List<Point>();

            for (int dolnyIndex = 0; dolnyIndex < predchadzajuciRiadok.Count - 1; dolnyIndex++)
            {
                Point bod = new Point();
                bod.X = (1 - parameterT) * predchadzajuciRiadok[dolnyIndex].X + (parameterT) * predchadzajuciRiadok[dolnyIndex + 1].X;
                bod.Y = (1 - parameterT) * predchadzajuciRiadok[dolnyIndex].Y + (parameterT) * predchadzajuciRiadok[dolnyIndex + 1].Y;
                aktualnyRiadok.Add(bod);

                //ak je zapnute vykreslovanie vykreslia sa aj jednotlive usecky algoritmu            
                if (CheckBoxCasteljau.IsChecked == true && parameterT == slider.Value)
                {
                    //vykresli usecky casteljua
                    VykresliUsecky(aktualnyRiadok, Colors.Gray, 1);
                    if (predchadzajuciRiadok.Count == 2)
                    {
                        // poslednu usecku posunieme do posledneho bodu ----> to je dotykovy vektor
                        List<Point> zoznamPreDotykovyVektor =
                        DajMiZoznamPosunutychBodov(predchadzajuciRiadok[0], predchadzajuciRiadok[1],
                        DajMiVektorPosunutia(predchadzajuciRiadok[0], aktualnyRiadok[0]));

                        //dotykovy vektor zmeni na normalovy
                        List<Point> zoznamPreNormalu = ZoznamPreNormalu(aktualnyRiadok[0], zoznamPreDotykovyVektor[1]);
                        VykresliUsecky(zoznamPreDotykovyVektor, Colors.Green, 2);
                        VykresliUsecky(zoznamPreNormalu, Colors.Red, 2);
                    }
                }
            }
            return BodyPreCasteljau(aktualnyRiadok, parameterT);
        }

        //vrati vektor posunutia 
        private Vector DajMiVektorPosunutia(Point zaciatocnyBod, Point koncovyBod)
        {
            Vector vektorPosunutia = new Vector { X = koncovyBod.X - zaciatocnyBod.X, Y = koncovyBod.Y - zaciatocnyBod.Y };
            return vektorPosunutia;
        }

        private List<Point> DajMiZoznamPosunutychBodov(Point bod1, Point bod2, Vector vektorPosunutia)
        {
            List<Point> zoznamPosunutychBodov = new List<Point>();
            Point posunutyBod1 = new Point { X = bod1.X + vektorPosunutia.X, Y = bod1.Y + vektorPosunutia.Y };
            Point posunutyBod2 = new Point { X = bod2.X + vektorPosunutia.X, Y = bod2.Y + vektorPosunutia.Y };
            zoznamPosunutychBodov.Add(posunutyBod1);
            zoznamPosunutychBodov.Add(posunutyBod2);
            return zoznamPosunutychBodov;
        }

        private List<Point> ZoznamPreNormalu(Point zaciatocnyBod, Point koncovyBod)
        {
            Vector normalovyVektor = DajMiVektorPosunutia(zaciatocnyBod, koncovyBod);
            double pomocnaX = normalovyVektor.X;
            double pomocnaY = normalovyVektor.Y;
            //vyrata normalovy vektor
            normalovyVektor.X = -pomocnaY;
            normalovyVektor.Y = pomocnaX;
            Point bodPreNormalu = new Point { X = zaciatocnyBod.X + normalovyVektor.X, Y = zaciatocnyBod.Y + normalovyVektor.Y };
            List<Point> zoznamPreNormalu = new List<Point>(new Point[] { zaciatocnyBod, bodPreNormalu });
            return zoznamPreNormalu;
        }

        private void VykresliZvysovanieStupna()
        {
            List<Point> bodyBeziera = BodyPreZvysovanieStupna(body, body.Count - 1, PocetVzorkovacichBodov(PocetR));
            VykresliUsecky(bodyBeziera, Colors.Blue, 2);
            //VykresliBody(bodyBeziera);
        }

        private List<Point> BodyPreZvysovanieStupna(List<Point> riadiaceBody, int stupen, int r)
        {
            // ak sme sa zanorili r-krat tak vratime prislusny zoznam
            if (pocitadlo == r)
            {
                pocitadlo = 0;
                return riadiaceBody;
            }

            //potitadlo mame koli tomu aby sme vedeli kedy koncit
            pocitadlo++;
            List<Point> novyZoznamVrcholov = new List<Point>();

            novyZoznamVrcholov.Add(riadiaceBody[0]);
            //pomocou vzorca sa vyrataju nove body pre vykreslenie
            for (int i = 1; i < riadiaceBody.Count; i++)
            {
                Point novyBod = new Point();
                novyBod.X = (double)i / (stupen + 1) * riadiaceBody[i - 1].X + (1 - (double)i / (stupen + 1)) * riadiaceBody[i].X;
                novyBod.Y = (double)i / (stupen + 1) * riadiaceBody[i - 1].Y + (1 - (double)i / (stupen + 1)) * riadiaceBody[i].Y;
                novyZoznamVrcholov.Add(novyBod);
            }
            novyZoznamVrcholov.Add(riadiaceBody[riadiaceBody.Count - 1]);
            return BodyPreZvysovanieStupna(novyZoznamVrcholov, stupen + 1, r);
        }

        private void VykreskliCasteljauAlgoritmus()
        {
            if (body.Count > 0)
            {
                List<Point> BodPreCasteljua = new List<Point>(new Point[] { BodyPreCasteljau(body, slider.Value) });
                VykresliBody(BodPreCasteljua, Colors.Black, 6);
            }
            stavVykreslenia = true;
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (body.Count > 0)
                {
                    NastavIndexTahanehoBodu(e.GetPosition(canvas));
                }
                if (indexTahanehoBodu == -1)
                {
                    //pri kazdom kliknuti prida bod
                    body.Add(e.GetPosition(canvas));
                }
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                ZmazBod(e.GetPosition(canvas));
            }
            PrekresliVsetko();
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (indexTahanehoBodu >= 0)
            {
                body[indexTahanehoBodu] = e.GetPosition(canvas); //nastavenie bodu ktory sa ma tahat(pohybovvat)

                PrekresliVsetko();
                return;
            }
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ZrusTahanieBodu();
        }

        // funkcia na convertovanie hodnot poctu vzorkovacich bodov / poctu zvyseni stupna / hlbky zanorenia m
        private int PocetVzorkovacichBodov(TextBox textBox)
        {
            return Convert.ToInt32(textBox.Text);
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            parameterT = slider.Value;
            if (IsLoaded)
            {
                ParameterT.Content = Convert.ToString(slider.Value);
                //hodnota.Content = Convert.ToString((sl.Value * 100) / 5) + "/ 20";
            }
            PrekresliVsetko();
        }

        private void CheckBoxCasteljau_Checked(object sender, RoutedEventArgs e)
        {
            if (body.Count > 1)
            {
                VykreskliCasteljauAlgoritmus();
            }
            stavVykreslenia = true;
        }

        private void CheckBoxCasteljau_Unchecked(object sender, RoutedEventArgs e)
        {
            if (stavVykreslenia == true)
            {
                stavVykreslenia = false;
                PrekresliVsetko();
            }
        }

        private void MinusPriamyVypocet_Click(object sender, RoutedEventArgs e)
        {
            if (PocetVzorkovacichBodov(PocetLOD) > 2)
            {
                PocetLOD.Text = Convert.ToString(PocetVzorkovacichBodov(PocetLOD) - 1);
            }
        }

        private void PlusPriamyVypocet_Click(object sender, RoutedEventArgs e)
        {
            PocetLOD.Text = Convert.ToString(PocetVzorkovacichBodov(PocetLOD) + 1);
        }

        private void MinusZvysovanieStupna_Click(object sender, RoutedEventArgs e)
        {
            if (PocetVzorkovacichBodov(PocetR) > 0)
            {
                PocetR.Text = Convert.ToString(PocetVzorkovacichBodov(PocetR) - 1);
            }
        }

        private void PlusZvysovanieStupna_Click(object sender, RoutedEventArgs e)
        {
            PocetR.Text = Convert.ToString(PocetVzorkovacichBodov(PocetR) + 1);
        }

        private void MinusPrerozdelenie_Click(object sender, RoutedEventArgs e)
        {
            if (PocetVzorkovacichBodov(HlbkaM) > 0)
            {
                HlbkaM.Text = Convert.ToString(PocetVzorkovacichBodov(HlbkaM) - 1);
            }
        }

        private void PlusPrerozdelenie_Click(object sender, RoutedEventArgs e)
        {
            HlbkaM.Text = Convert.ToString(PocetVzorkovacichBodov(HlbkaM) + 1);
        }

        private void PocetLOD_TextChanged(object sender, TextChangedEventArgs e)
        {
            PrekresliVsetko();
        }

        private void PocetR_TextChanged(object sender, TextChangedEventArgs e)
        {
            PrekresliVsetko();
        }

        private void HlbkaM_TextChanged(object sender, TextChangedEventArgs e)
        {
            PrekresliVsetko();
        }

        private void RBPriamyVypocet_Checked(object sender, RoutedEventArgs e)
        {
            PrekresliVsetko();
        }

        private void RBZvysovanieStupna_Checked(object sender, RoutedEventArgs e)
        {
            PrekresliVsetko();
        }

        private void RBPrerozdelenie_Checked(object sender, RoutedEventArgs e)
        {
            PrekresliVsetko();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ZmazVsetkyVykresleneObjekty();
            body.Clear();
            stavVykreslenia = false;
            CheckBoxCasteljau.IsChecked = false;
            slider.Value = 0.5;
        }
    }
}
