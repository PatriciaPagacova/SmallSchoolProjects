using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Projekt2
{
    public class Segment
    {
        private Canvas canvas;
        private List<Segment> podSegmenty = new List<Segment>();
        private Point bodPatriaciTomutoSegmentu = new Point(-1, -1);

        //konstruktor hlavneho segmentu
        public Segment(Canvas c, double sirka, double vyska)
        {
            canvas = c;
            VytvorGrafickyStvorec(sirka, vyska);
            VykresliGrafickyStvorec(0, 0);
            Kvadrant = "Koren";
        }

        //konstrukor dalsich segmentov
        public Segment(Canvas c, Segment nadSegment, int kvadrant)
        {
            canvas = c;

            //vypocita sirku a vysku podsegmentu
            double sirka = nadSegment.GrafickyStvorec.Width / 2.0;
            double vyska = nadSegment.GrafickyStvorec.Height / 2.0;

            VytvorGrafickyStvorec(sirka, vyska);

            //urci lavy horny roh nadsegmentu
            double x0 = Canvas.GetLeft(nadSegment.GrafickyStvorec);
            double y0 = Canvas.GetTop(nadSegment.GrafickyStvorec);

            //podla prislusneho kvadrantu vykresli stvorec podsegmentu
            if (kvadrant == 1)
            {
                VykresliGrafickyStvorec(x0 + sirka, y0);
                Kvadrant = "I.";
            }
            else if (kvadrant == 2)
            {
                VykresliGrafickyStvorec(x0 + sirka, y0 + vyska);
                Kvadrant = "II.";
            }
            else if (kvadrant == 3)
            {
                VykresliGrafickyStvorec(x0, y0 + vyska);
                Kvadrant = "III.";
            }
            else
            {
                VykresliGrafickyStvorec(x0, y0);
                Kvadrant = "IV.";
            }
        }

        // stvorec segmentu - pre vykreslovanie
        public Rectangle GrafickyStvorec { get; private set; }

        public string Kvadrant { get; private set; }

        //metoda, ktora pridava body prislusnym segmentom + stara sa o rozdelovanie segmentov ak je to treba
        public void PridajBod(Point bod)
        {
            //ak je uz segment rozdeleny, posuva vyzvu na rozdelenie dalsiemu segmentu
            if (podSegmenty.Count > 0)
            {
                for (int i = 0; i < podSegmenty.Count; i++)
                {
                    if (LeziDanyBodVSegmente(podSegmenty[i], bod))
                    {
                        podSegmenty[i].PridajBod(bod);
                        return;
                    }
                }
            }
            //ak segment nema ziaden bod tak mu bod priradi
            if (!SegmetUzObsahujeBod())
            {
                bodPatriaciTomutoSegmentu = bod;

            }
            //ak segmet obsahuje bod rozdeli sa, 
            //z povodneho segmentu  povodny bod vymaze a priradu ho k prislusmemu podsegmentu
            // aktualne kliknuty bod tiez priradi prislusnemu segmentu                                      
            else if (SegmetUzObsahujeBod())
            {
                RozdelSa();
                PriradBodPodsegmentu(bodPatriaciTomutoSegmentu);
                PriradBodPodsegmentu(bod);

                bodPatriaciTomutoSegmentu.X = -1;
                bodPatriaciTomutoSegmentu.Y = -1;
            }
        }


        public void ZmazSa()
        {
            canvas.Children.Remove(GrafickyStvorec);
            for (int i = 0; i < podSegmenty.Count; i++)
            {
                podSegmenty[i].ZmazSa();
            }
            podSegmenty.Clear();
        }

        // vrati retazec cesty k bodu
        public string DajMiCestu(Point bod, string cesta)
        {
            for (int i = 0; i < podSegmenty.Count; i++)
            {
                if (LeziDanyBodVSegmente(podSegmenty[i], bod))
                {
                    cesta = cesta + "-" + podSegmenty[i].Kvadrant;
                    cesta = podSegmenty[i].DajMiCestu(bod, cesta);
                }
            }
            return cesta;
        }

        private void VytvorGrafickyStvorec(double sirka, double vyska)
        {
            Rectangle stvorec = new Rectangle();
            stvorec.Stroke = Brushes.Black;
            stvorec.StrokeThickness = 0.5;
            stvorec.Width = sirka;
            stvorec.Height = vyska;
            GrafickyStvorec = stvorec;
        }

        private void VykresliGrafickyStvorec(double surX, double surY)
        {
            Canvas.SetLeft(GrafickyStvorec, surX);
            Canvas.SetTop(GrafickyStvorec, surY);
            canvas.Children.Add(GrafickyStvorec);
        }

        private bool SegmetUzObsahujeBod()
        {
            return bodPatriaciTomutoSegmentu.X > 0 && bodPatriaciTomutoSegmentu.Y > 0;
        }

        private void RozdelSa()
        {
            for (int i = 1; i <= 4; i++)
            {
                podSegmenty.Add(new Segment(canvas, this, i));
            }
        }

        private void PriradBodPodsegmentu(Point bod)
        {
            for (int i = 0; i < podSegmenty.Count; i++)
            {
                if (LeziDanyBodVSegmente(podSegmenty[i], bod))
                {
                    podSegmenty[i].PridajBod(bod);
                    return;
                }
            }
        }

        private bool LeziDanyBodVSegmente(Segment segment, Point bod)
        {
            double surX = Canvas.GetLeft(segment.GrafickyStvorec);
            double surY = Canvas.GetTop(segment.GrafickyStvorec);
            double surXPlusSirka = surX + segment.GrafickyStvorec.Width;
            double surYPlusVyska = surY + segment.GrafickyStvorec.Height;

            return surX < bod.X  && bod.X  <= surXPlusSirka && surY < bod.Y  && bod.Y  <= surYPlusVyska;
        }
    }
}
