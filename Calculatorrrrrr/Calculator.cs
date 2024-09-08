using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Numerics;

namespace Calculatorrrrrr
{
    public partial class Calculator : Form
    {
        List<char> display = [];

        int op = 0;
        double? nr, nr2 = 0;
        double memory = 0;
        bool isEquals = false;

        public Calculator()
        {   //to make it legible
            // screen.Font = new Font("Bahnschrift Light SemiCondensed", 24F); 
            // Font = new Font("Perpetua Tilting MT", 12F, FontStyle.Regular, GraphicsUnit.Point, 2);
            InitializeComponent();
            LabelMem.Hide();
        }

        // Simple buttons
        private void button0_Click(object sender, EventArgs e) => Display_Add('0');
        private void button1_Click(object sender, EventArgs e) => Display_Add('1');
        private void button2_Click(object sender, EventArgs e) => Display_Add('2');
        private void button3_Click(object sender, EventArgs e) => Display_Add('3');
        private void button4_Click(object sender, EventArgs e) => Display_Add('4');
        private void button5_Click(object sender, EventArgs e) => Display_Add('5');
        private void button6_Click(object sender, EventArgs e) => Display_Add('6');
        private void button7_Click(object sender, EventArgs e) => Display_Add('7');
        private void button8_Click(object sender, EventArgs e) => Display_Add('8');
        private void button9_Click(object sender, EventArgs e) => Display_Add('9');
        private void buttonComma_Click(object sender, EventArgs e)
        {
            if (display.Count == 0) Display_Add('0');
            Display_Add('.');
        }


        // Memory management
        private void buttonMPlus_Click(object sender, EventArgs e)
        {
            memory += Display_toValue(false);
            LabelMem.Show();
        }
        private void buttonMMinus_Click(object sender, EventArgs e)
        {
            memory -= Display_toValue(false);
            LabelMem.Show();
        }
        private void buttonMC_Click(object sender, EventArgs e)
        {
            memory = 0;
            LabelMem.Hide();
        }

        private void buttonMR_Click(object sender, EventArgs e) => Display_fromValue(memory);


        private void buttonPlus_Click(object sender, EventArgs e) => Operation(1);


        private void buttonMinus_Click(object sender, EventArgs e) => Operation(2);


        private void buttonMult_Click(object sender, EventArgs e) => Operation(3);


        private void buttonDiv_Click(object sender, EventArgs e) => Operation(4);


        //special OPs

        private void buttonSQRT_Click(object sender, EventArgs e)
        {
            if (Display_toValue(false) >= 0)
                Display_fromValue(Math.Sqrt(Display_toValue()));

            else Calc_Clear("ERROR: negative sqrt"); //negative sqrt
        }

        private void buttonPerc_Click(object sender, EventArgs e)
        {
            if (nr.HasValue)
            {
                nr2 = Display_toValue(false);
                switch (op)
                {
                    case 0: // noop 
                        //nothing to do
                        break;
                    case 1: // +
                        nr += (nr * nr2) / 100;
                        break;
                    case 2: // -
                        nr -= (nr * nr2) / 100;
                        break;
                    case 3: // *
                        nr *= nr2 / 100;
                        break;
                    case 4: // /
                        if (nr2 != 0) nr /= (nr2 / 100);
                        else { Calc_Clear("ERROR: divide by zero"); return; } //divide by zero

                        break;
                    default:
                        throw new InvalidOperationException();
                }
                Display_fromValue((double)nr);

                op = 0;
            }
            display.Clear();
        }
        private void buttonEquals_Click(object sender, EventArgs e)
        {
            if (nr.HasValue)
            {
                if (!isEquals)
                {
                    nr2 = Display_toValue(false);
                }

                switch (op)
                {
                    case 0: // noop
                            // nothing to do
                        break;
                    case 1: // +
                        nr += nr2;
                        break;
                    case 2: // -
                        nr -= nr2;
                        break;
                    case 3: // *
                        nr *= nr2;
                        break;
                    case 4: // /
                        if (nr2 != 0) nr /= nr2;
                        else { Calc_Clear("ERROR: divide by zero"); return; }  //divide by zero
                        break;
                    default:
                        throw new InvalidOperationException();

                }
                screen.Text = nr.ToString();
                isEquals = true;
            }
            display.Clear();

        }

        private void buttonCE_Click(object sender, EventArgs e)
        {
            display.Clear();
            screen.Text = "0";
        }

        private void buttonAC_Click(object sender, EventArgs e) => Calc_Clear();


        private void Operation(int op_new)
        {
            if (isEquals) isEquals = false;
            //if (nr.HasValue && op != 0) screen.Text = nr.ToString();

            if (!nr.HasValue) nr = Display_toValue();
            else if (display.Count != 0) switch (op)
                {
                    case 1:
                        nr += Display_toValue();
                        break;
                    case 2:
                        nr -= Display_toValue();
                        break;
                    case 3:
                        nr *= Display_toValue();
                        break;
                    case 4:
                        if (Display_toValue(false) == 0)
                        { Calc_Clear("ERROR: divide by zero"); return; }
                        nr /= Display_toValue();
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            if (op != 0) screen.Text = nr.ToString();
            op = op_new;

        }
        private void Calc_Clear(string? err = null)
        {
            display.Clear();
            //display.Add('0');
            op = 0;
            nr = null;
            isEquals = false;
            if (err != null) screen.Text = err;
            else screen.Text = "0";
        }

        private double Display_toValue(bool clear = true)
        {
            double val;
            try
            {
                val = double.Parse(string.Join("", display),
                            System.Globalization.CultureInfo.InvariantCulture);

                /* if (val == double.PositiveInfinity || val == double.NegativeInfinity) 
                 {
                     Calc_Clear("ERROR: number overflow");
                     return val; */
            }
            catch (FormatException)
            {
                if (nr.HasValue) val = (double)nr;
                else val = 0;

            }
            if (clear) display.Clear();
            return val;
        }
        private void Display_fromValue(double value, bool upd = true)
        {
            display = [.. value.ToString().ToCharArray()];
            if (upd) screen.Text = string.Join("", display);
        }

        private void buttonSign_Click(object sender, EventArgs e)
        {
            double t = Display_toValue();
            t -= (t * 2);
            Display_fromValue(t);
            if (isEquals) nr -= (nr * 2);
        }

        private void Display_Add(char c)
        {
            if (display.Count != 0 && display[0] == 0) display.RemoveAt(0);
            display.Add(c);
            screen.Text = string.Join("", display);
            if (isEquals)
            {
                isEquals = false;
                op = 0;
                nr = null;
            }
        }

        
    }
}
