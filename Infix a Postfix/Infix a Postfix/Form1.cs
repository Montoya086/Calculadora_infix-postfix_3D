// Andrés Montoya
// C5E
// Infix a Postfix
// Programa para la conversion de operaciones de tipo infix a postfix
// Convertir a infix a postfix por medio de ciclos, funciones e if's, Tambien la utilizacion de picturebox para la graficación
// 28/04/2020
// 12/05/2020
// C#
// Visual Studio
// .NET Framework
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Infix_a_Postfix
{
    public partial class Form1 : Form
    {
        postfix ex;
        Graphics Canvas;
        Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
            Canvas = pictureBox1.CreateGraphics();
        }
        
        
        private void button1_Click(object sender, EventArgs e)
        {
            ex = new postfix(textBox1.Text);
            if (ex.error == "")
                textBox2.Text = ex.ep;
            else
                MessageBox.Show(ex.error);
        }
        
        
        class postfix
        {
            public string ei, ep, error;
            double[] num = new double[200];
            public postfix(string s)
            {
                int i;
                char c;
                bool ts;                
                const string signos = "+-*/^sct";
                Stack<char> st = new Stack<char>();
                int jer(char sig)
                {
                    switch (sig)
                    {
                        case '+': case '-': return 1;
                        case '*': case '/': return 2;
                        case '^': return 3;
                        case 's':
                        case 'c':
                        case 't':
                        case '~': return 4;
                        default: return 0;
                    }
                }

                void signo(char sig)
                {
                    while (st.Count > 0 && jer(sig) <= jer(st.Peek()))
                    {
                        ep += st.Pop();
                    }
                    st.Push(sig);
                }
               
                error = "";
                ep = "";
                ei = s;
                s = s.ToLower();
                ts = false;
                for (i = 0; i < s.Length; i++)
                {
                    c = s[i];
                    if (char.IsDigit(c))
                    {
                        if (ts) signo('*');
                        string temp = c.ToString();
                        while (i+1 < s.Length && char.IsDigit(s[i + 1]))
                            temp = temp + s[++i].ToString();
                        if(i+1<s.Length && s[i+1]=='.')
                        {
                            temp += '.';
                            i++;
                            while (i + 1 < s.Length && char.IsDigit(s[i + 1]))
                                temp = temp + s[++i].ToString();
                        }
                        if (i + 1 < s.Length && s[i + 1] == 'e')
                        {
                            temp += 'E';
                            i++;
                            if (i + 1 < s.Length && (s[i + 1] == '+' || s[i + 1]=='-'))
                            {
                                temp += s[++i].ToString();
                            }
                            while (i + 1 < s.Length && char.IsDigit(s[i + 1]))
                                temp = temp + s[++i].ToString();
                        }
                        num[ep.Length] = Convert.ToDouble(temp);
                        ep = ep + "#";
                        ts = true;
                    }
                    if (c == '(')
                    {
                        st.Push('(');
                    }
                    else if (c == ')')
                    {
                        while (st.Count > 0 && st.Peek() != '(')
                            ep += st.Pop();
                        if (st.Count > 0)
                            st.Pop(); 
                    }
                    
                    else if (signos.IndexOf(c) >= 0)
                    {
                        if (c == 's' || c == 'c' || c == 't')
                        {
                            if (ts) signo('*');
                            signo(c);
                        }
                        else if (!ts)
                            if (c == '-')
                                signo('~');
                            else
                                error = "Hay dos signos juntos";
                        else
                            signo(c);
                        ts = false; 
                        
                    }
                    else if (char.IsLetter(c))
                    {
                        if (ts)
                            signo('*');
                        ep += c;
                        ts = true;

                    }
                }

                while (st.Count > 0)
                {
                    ep += st.Pop();
                }
            }

            public double eval(double x = 0, double y = 0, double z = 0)
            {
                Stack<double> st = new Stack<double>();
                for (int i = 0; i < ep.Length; i++)
                {
                    switch(ep[i])
                    {
                        case '#': st.Push(num[i]); break;
                        case 'x': st.Push(x); break;
                        case 'y': st.Push(y); break;
                        case 'z': st.Push(z); break;
                        case 's': st.Push(Math.Sin(st.Pop())); break;
                        case 'c': st.Push(Math.Cos(st.Pop())); break;
                        case 't': st.Push(Math.Tan(st.Pop())); break;
                        case '~': st.Push(-st.Pop()); break;
                        case '+': st.Push(st.Pop() + st.Pop()); break;
                        case '*': st.Push(st.Pop() * st.Pop()); break;
                        case '-': st.Push(-st.Pop() + st.Pop()); break; 
                        case '/': st.Push(1 / st.Pop() * st.Pop()); break;
                        case '^':
                            double b = st.Pop();
                            double a = st.Pop();
                            st.Push(Math.Pow(a, b));
                            break;
                    }
                }
                return st.Pop();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double x, y, z;
            ex = new postfix(textBox1.Text);

            double.TryParse(textBox3.Text, out x);
            double.TryParse(textBox4.Text, out y);
            double.TryParse(textBox5.Text, out z);

            textBox2.Text = ex.eval(x, y, z).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Canvas.Clear(Color.White);
                double lix, lsx, lsy, liy, x, y, paso;
                int ax, ay, nx, ny;
                postfix fx = new postfix(textBox1.Text);
                lix = Convert.ToDouble(textBox7.Text);
                lsx = Convert.ToDouble(textBox8.Text);
                int xp(double equis)
                {
                    return (int)((equis - lix) * pictureBox1.Width / (lsx - lix));
                }
                int yp(double ye)
                {
                    return (int)((lsy - ye) * pictureBox1.Height / (lsy - liy));
                }
                paso = (lsx - lix) / 1000;
                x = lix; y = fx.eval(x);
                liy = lsy = y;
                while (x < lsx)
                {
                    x = x + paso;
                    y = fx.eval(x);
                    if (y < liy) liy = y;
                    if (y > lsy) lsy = y;
                }
                if (liy == lsy)
                {
                    lsy += 10;
                    liy -= 10;
                }
                x = lix; y = fx.eval(x);
                ax = xp(x); ay = yp(y);
                while (x < lsx)
                {
                    x = x + paso;
                    y = fx.eval(x);
                    nx = xp(x);
                    ny = yp(y);
                    Canvas.DrawLine(Pens.Navy, ax, ay, nx, ny);
                    ax = nx;
                    ay = ny;
                }
                int pe = (liy < 0 && lsy > 0) ? yp(0) : (liy > 0) ? pictureBox1.Height - 5 : 5;
                Canvas.DrawLine(Pens.Red, 0, yp(0), pictureBox1.Width, yp(0));
                pe = (lix < 0 && lsx > 0) ? xp(0) : (lsx < 0) ? pictureBox1.Width - 5 : 5;
                Canvas.DrawLine(Pens.Red, pe, 0, pe, pictureBox1.Height);
                if (lix < 0 && lsx > 0)
                {
                    double k = Math.Pow(10, Math.Truncate(Math.Log10(lsx - lix))) / 10.0;
                    x = 0;
                    while (x <= lsx)
                    {
                        Canvas.DrawLine(Pens.Red, xp(x), yp(0) - 5, xp(x), yp(0) + 5);
                        Canvas.DrawString(x.ToString(), new Font("Arial", 10), Brushes.Black, xp(x) - 3, yp(0));
                        x = x + k;
                    }
                    while (x > lix)
                    {
                        Canvas.DrawLine(Pens.Red, xp(x), yp(0) - 5, xp(x), yp(0) + 5);
                        Canvas.DrawString(x.ToString(), new Font("Arial", 10), Brushes.Black, xp(x) - 3, yp(0));
                        x = x - k;
                    }
                }
            }
            catch
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            postfix fx = new postfix(textBox1.Text);
            double x, y, z, lix, lsx, liy, lsy, pasox, pasoy;
            double a,liz,lsz;
            double x1, y1, lix1, lsx1, liy1, lsy1;
            int ax, ay, nx, ny;
            int margen;
            Pen c1;
            int xp(double v)
            {
                return (int)((v - lix1) * (pictureBox1.Width-margen*2) / (lsx1 - lix1)+margen);
            }
            int yp(double c)
            {
                return (int)((lsy1 - c) * (pictureBox1.Height-margen*2) / (lsy1 - liy1)+margen);
            }
            Canvas.Clear(Color.White);
            Double.TryParse(textBox8.Text, out lsx);
            Double.TryParse(textBox7.Text, out lix);
            Double.TryParse(textBox9.Text, out liy);
            Double.TryParse(textBox6.Text, out lsy);
            a = (double)numericUpDown1.Value;
            margen = (int)numericUpDown2.Value;
            pasox = (lsx - lix) / 500;
            pasoy = (lsy - liy) / 500;
            x = lix; y = liy;
            z = fx.eval(x, y);
            x1 = x + y * Math.Cos(a);
            y1 = z + y * Math.Sin(a);
            lix1 = lsx1 = x1;
            liy1 = lsy1 = y1;
            liz = lsz = z;
            while (y<lsy)
            {
                y+=pasoy;
                x = lix;
                while(x<lsx)
                {
                    x += pasox;
                    z = fx.eval(x, y);
                    x1 = x + y * Math.Cos(a);
                    y1 = z + y * Math.Sin(a);
                    if (x1 < lix1) lix1 = x1;
                    if (x1 > lsx1) lsx1 = x1;
                    if (y1 < liy1) liy1 = y1;
                    if (y1 > lsy1) lsy1 = y1;
                    if (z > lsz) lsz = z;
                    if (z < liz) liz = z;
                }
            }
            ejes();
            c1 = Pens.Navy;
            pasox = (lsx - lix) / 500;
            pasoy = (lsy - liy) / 50;
            y = liy;
            while(y<=lsy)
            {
                x = lix;
                z = fx.eval(x, y);
                x1 = x + y * Math.Cos(a);
                y1 = z + y * Math.Sin(a);
                ax = xp(x1);
                ay = yp(y1);
                c1 = new Pen(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)), 1);
                while (x<=lsx)
                {
                    
                    x = x + pasox;
                    z = fx.eval(x, y);
                    x1 = x + y * Math.Cos(a);
                    y1 = z + y * Math.Sin(a);
                    nx = xp(x1);
                    ny = yp(y1);
                    Canvas.DrawLine(c1, ax, ay, nx, ny);
                    ax = nx;
                    ay = ny;
                }
                y = y + pasoy;
            }
            pasox = (lsx - lix) / 50;
            pasoy = (lsy - lix) / 500;
            x = lix;
            while (x <= lsx)
            {
                y = liy;
                z = fx.eval(x, y);
                x1 = x + y * Math.Cos(a);
                y1 = z + y * Math.Sin(a);
                ax = xp(x1);
                ay = yp(y1);
                c1 = new Pen(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)), 1);
                while (y <= lsy)
                {
                    y = y + pasoy;
                    z = fx.eval(x, y);
                    x1 = x + y * Math.Cos(a);
                    y1 = z + y * Math.Sin(a);
                    nx = xp(x1);
                    ny = yp(y1);
                    Canvas.DrawLine(c1, ax, ay, nx, ny);
                    ax = nx;
                    ay = ny;
                }
                x = x + pasox;
            }
            void ejes()
            {
                linea(lix, 0, 0, lsx, 0, 0, Color.Red, 4, DashStyle.Solid);
                linea(0, liy, 0, 0, lsy, 0, Color.Red, 4, DashStyle.Solid);
                linea(0, 0, liz,0, 0, lsz, Color.Red, 4, DashStyle.Solid);

                linea(lix, liy, liz, lix, liy, lsz, Color.Black, 2, DashStyle.Dash);
                linea(lix, lsy, liz, lix, lsy, lsz, Color.Black, 2, DashStyle.Dash);
                linea(lix, liy, liz, lix, lsy, liz, Color.Black, 2, DashStyle.Dash);
                linea(lix, liy, lsz, lix, lsy, lsz, Color.Black, 2, DashStyle.Dash);
                linea(lix, lsy, lsz, lsx, lsy, lsz, Color.Black, 2, DashStyle.Dash);
                linea(lix, lsy, liz, lsx, lsy, liz, Color.Black, 2, DashStyle.Dash);
                linea(lsx, lsy, lsz, lsx, lsy, liz, Color.Black, 2, DashStyle.Dash);
                linea(lix, liy, liz, lsx, liy, liz, Color.Black, 2, DashStyle.Dash);
                linea(lsx, lix, liz, lsx, lsy, liz, Color.Black, 2, DashStyle.Dash);

                linea(lix, liy, 0, lix, lsy, 0, Color.Red, 1, DashStyle.Dash);
                linea(lix, lsy, 0, lsx, lsy, 0, Color.Red, 1, DashStyle.Dash);
                linea(lix, 0, lsz, lix, 0, liz, Color.Red, 1, DashStyle.Dash);
                linea(0, lsy, lsz, 0, lsy, liz, Color.Red, 1, DashStyle.Dash);
                linea(0, liy, liz, 0, lsy, liz, Color.Red, 1, DashStyle.Dash);
                linea(lix, 0, liz, lsx, 0, liz, Color.Red, 1, DashStyle.Dash);
            }
            void linea(double xi, double yi, double zi, double xf, double yf, double zf, Color c, int w, DashStyle d)
            {
                Pen p = new Pen(c, w);
                p.DashStyle = d;
                double px, py;
                px = xi + yi * Math.Cos(a); 
                py = zi + yi * Math.Sin(a);
                ax = xp(px);
                ay = yp(py);
                px = xf + yf * Math.Cos(a);
                py = zf + yf * Math.Sin(a);
                nx = xp(px);
                ny = yp(py);
                Canvas.DrawLine(p, ax, ay, nx, ny);

            }
        }
    }
}
