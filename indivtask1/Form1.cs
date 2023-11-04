using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace indivtask1
{
    public partial class Form1 : Form
    {
        private List<PointF> points = new List<PointF>();
        private List<PointF> convexHull = new List<PointF>();

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                points.Add(e.Location);
                Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                FindConvexHull();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (PointF point in points)
            {
                DrawPoint(e.Graphics, point, Brushes.Black);
            }

            for (int i = 0; i < convexHull.Count; i++)
            {
                PointF p1 = convexHull[i];
                PointF p2 = convexHull[(i + 1) % convexHull.Count];
                DrawLine(e.Graphics, p1, p2, Pens.Red);
            }
        }

        private void DrawPoint(Graphics graphics, PointF point, Brush brush)
        {
            float radius = 4f;
            graphics.FillEllipse(brush, point.X - radius, point.Y - radius, radius * 2, radius * 2);
        }

        private void DrawLine(Graphics graphics, PointF p1, PointF p2, Pen pen)
        {
            graphics.DrawLine(pen, p1, p2);
        }

        private void FindConvexHull()
        {
            if (points.Count < 3)
            {
                MessageBox.Show("Добавьте хотя бы 3 точки для построения выпуклой оболочки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            convexHull.Clear();

            PointF leftmostPoint = points.OrderBy(p => p.X).First();
            convexHull.Add(leftmostPoint);

            PointF hullPoint = leftmostPoint;
            PointF endpoint;

            do
            {
                convexHull.Add(hullPoint);
                endpoint = points.First();
                foreach (PointF candidate in points)
                {
                    if (candidate == hullPoint || convexHull.Contains(candidate))
                        continue;

                    if (endpoint == hullPoint || IsCounterClockwise(hullPoint, endpoint, candidate))
                        endpoint = candidate;
                }

                hullPoint = endpoint;
            } while (!endpoint.Equals(leftmostPoint));

            Invalidate();
        }

        private bool IsCounterClockwise(PointF a, PointF b, PointF c)
        {
            float area = (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
            return area > 0;
        }
    }
}
