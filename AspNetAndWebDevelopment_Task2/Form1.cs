using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace AspNetAndWebDevelopment_Task2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DrawField();
            Draw_x();
            Draw_o();

            field_info.AutoSize = false;
            field_info.Width = 396;
            field_info.TextAlign = ContentAlignment.MiddleCenter;
            field_info.Location = new Point(33, field_info.Location.Y);
        }

        private Bitmap _imgX;
        private Bitmap _imgO;
        private short[,] _cells = new short[3, 3];
        private short _xOrO; //1 - x, 2 - o
        private short _opposite;
        private short _movesNumber = 1;
        private readonly Random _rand = new Random();

        private void DrawField()
        {
            Bitmap bmp = new Bitmap(field.Width, field.Height);
            Graphics graph = Graphics.FromImage(bmp);
            SolidBrush sb = new SolidBrush(Color.Black);
            graph.FillRectangle(sb, 133, 1, 3, field.Height);
            graph.FillRectangle(sb, 266, 1, 3, field.Height);
            graph.FillRectangle(sb, 1, 133, field.Width, 3);
            graph.FillRectangle(sb, 1, 266, field.Width, 3);
            field.Image = bmp;
        }

        private void Draw_x()
        {
            _imgX = new Bitmap(Cell_0_0.Width, Cell_0_0.Height);
            Graphics graph = Graphics.FromImage(_imgX);
            graph.SmoothingMode = SmoothingMode.HighQuality;
            Pen pen = new Pen(Color.Blue, 4);
            graph.DrawLine(pen, 5, 5, 119, 119);
            graph.DrawLine(pen, 5, 119, 119, 5);
        }

        private void Draw_o()
        {
            _imgO = new Bitmap(Cell_0_0.Width, Cell_0_0.Height);
            Graphics graph = Graphics.FromImage(_imgO);
            graph.SmoothingMode = SmoothingMode.HighQuality;
            Pen pen = new Pen(Color.Red, 4);
            graph.DrawEllipse(pen, 6, 6, 114, 114);
        }

        //------------------------------------------------------------------------------
        // Menu
        //------------------------------------------------------------------------------
        private void x_1pl_Click(object sender, EventArgs e)
        {
            _xOrO = 1;
            _opposite = 2;
            x_or_o_1pl.Visible = false;
            FirstTic();
        }

        private void o_1pl_Click(object sender, EventArgs e)
        {
            _xOrO = 2;
            _opposite = 1;
            x_or_o_1pl.Visible = false;
            FirstTic();
        }

        private void FieldVisible(bool a)
        {
            field.Visible = a;
            field_info.Visible = a;
            Cell_0_0.Visible = a;
            Cell_0_1.Visible = a;
            Cell_0_2.Visible = a;
            Cell_1_0.Visible = a;
            Cell_1_1.Visible = a;
            Cell_1_2.Visible = a;
            Cell_2_0.Visible = a;
            Cell_2_1.Visible = a;
            Cell_2_2.Visible = a;
        }

        private void StartGame_Click(object sender, EventArgs e)
        {
            MainMenu_gr.Visible = false;
            x_or_o_1pl.Visible = true;
        }

        private void Restart_but_Click(object sender, EventArgs e)
        {
            ClearCells();
            MainMenu_gr.Visible = false;
            Winner_gr.Visible = false;
            FirstTic();
        }

        private void ToMainMenu_but_Click(object sender, EventArgs e)
        {
            MoveToMain();
        }

        private void MainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveToMain();
        }

        private void MoveToMain()
        {
            ClearCells();
            FieldVisible(false);
            Winner_gr.Visible = false;
            MainMenu_gr.Visible = true;
        }

        private void ClearCells()
        {
            _movesNumber = 1;
            _cells = new Int16[3, 3];
            Cell_0_0.Image = null;
            Cell_0_1.Image = null;
            Cell_0_2.Image = null;
            Cell_1_0.Image = null;
            Cell_1_1.Image = null;
            Cell_1_2.Image = null;
            Cell_2_0.Image = null;
            Cell_2_1.Image = null;
            Cell_2_2.Image = null;
        }


        //--------------------------------------------------------------------------------
        // Game logic
        //--------------------------------------------------------------------------------
        private void FirstTic()
        {
            FieldVisible(true);
            if (_rand.Next(2) == 0)
            {
                field_info.Text = "Ваш ход!";
                --_movesNumber;
            }
            else
            {
                field_info.Text = "Ходит бот...";
                Invalidate();
                Update();
                DrawCellsForBotStep((short) _rand.Next(3), (short) _rand.Next(3));
                field_info.Text = "Ваш ход!";
            }
        }

        private void Cell_0_0_Click(object sender, EventArgs e)
        {
            OnCellClicked(0, 0, Cell_0_0);
        }

        private void Cell_0_1_Click(object sender, EventArgs e)
        {
            OnCellClicked(0, 1, Cell_0_1);
        }

        private void Cell_0_2_Click(object sender, EventArgs e)
        {
            OnCellClicked(0, 2, Cell_0_2);
        }

        private void Cell_1_0_Click(object sender, EventArgs e)
        {
            OnCellClicked(1, 0, Cell_1_0);
        }

        private void Cell_1_1_Click(object sender, EventArgs e)
        {
            OnCellClicked(1, 1, Cell_1_1);
        }

        private void Cell_1_2_Click(object sender, EventArgs e)
        {
            OnCellClicked(1, 2, Cell_1_2);
        }

        private void Cell_2_0_Click(object sender, EventArgs e)
        {
            OnCellClicked(2, 0, Cell_2_0);
        }

        private void Cell_2_1_Click(object sender, EventArgs e)
        {
            OnCellClicked(2, 1, Cell_2_1);
        }

        private void Cell_2_2_Click(object sender, EventArgs e)
        {
            OnCellClicked(2, 2, Cell_2_2);
        }

        private void OnCellClicked(int rowIndex, int columnIndex, PictureBox cell)
        {
            if (_cells[rowIndex, columnIndex] == 0)
            {
                _cells[rowIndex, columnIndex] = _xOrO;
                cell.Image = _xOrO == 1 ? _imgX : _imgO;
                ToNext();
            }
        }

        private void ToNext()
        {
            if (_movesNumber < 9)
            {
                ++_movesNumber;
                if (_movesNumber < 5 || !IsWin(out short a))
                {
                    if (_movesNumber >= 9)
                        ToNext();
                    else
                    {
                        field_info.Text = "Ходит бот...";
                        Invalidate();
                        Update();
                        GeneratePosition();
                        if (_movesNumber >= 9)
                            ToNext();
                        else
                        {
                            if (_movesNumber >= 5 && IsWin(out a))
                                ToNext();
                        }
                    }

                    field_info.Text = "Ваш ход!";
                }
                else
                {
                    ShowWinner(a);
                }
            }
            else
            {
                if (IsWin(out short t))
                {
                    ShowWinner(t);
                }
                else
                {
                    Invalidate();
                    Update();
                    Thread.Sleep(250);
                    FieldVisible(false);
                    MainMenu_gr.Visible = true;
                    Winner_gr.Visible = true;
                    Winner_text.ForeColor = Color.Blue;
                    Winner_text.Text = "Ничья!";
                }
            }
        }

        private bool IsWin(out short a)
        {
            if (_cells[0, 0] != 0 && _cells[0, 1] == _cells[0, 0] && _cells[0, 2] == _cells[0, 0])
            {
                a = _cells[0, 0];
                return true;
            }

            if (_cells[1, 0] != 0 && _cells[1, 1] == _cells[1, 0] && _cells[1, 2] == _cells[1, 0])
            {
                a = _cells[1, 0];
                return true;
            }

            if (_cells[2, 0] != 0 && _cells[2, 1] == _cells[2, 0] && _cells[2, 2] == _cells[2, 0])
            {
                a = _cells[2, 0];
                return true;
            }

            if (_cells[0, 0] != 0 && _cells[1, 0] == _cells[0, 0] && _cells[2, 0] == _cells[0, 0])
            {
                a = _cells[0, 0];
                return true;
            }

            if (_cells[0, 1] != 0 && _cells[1, 1] == _cells[0, 1] && _cells[2, 1] == _cells[0, 1])
            {
                a = _cells[0, 1];
                return true;
            }

            if (_cells[0, 2] != 0 && _cells[1, 2] == _cells[0, 2] && _cells[2, 2] == _cells[0, 2])
            {
                a = _cells[0, 2];
                return true;
            }

            if (_cells[0, 0] != 0 && _cells[1, 1] == _cells[0, 0] && _cells[2, 2] == _cells[0, 0])
            {
                a = _cells[0, 0];
                return true;
            }

            if (_cells[2, 0] != 0 && _cells[2, 0] == _cells[1, 1] && _cells[0, 2] == _cells[1, 1])
            {
                a = _cells[2, 0];
                return true;
            }

            a = 0;
            return false;
        }

        private void GeneratePosition()
        {
            bool fit = false;
            short r, c;
            do
            {
                r = (short) _rand.Next(3);
                c = (short) _rand.Next(3);
                if (_cells[r, c] == 0)
                {
                    fit = true;
                }
            } while (!fit);

            DrawCellsForBotStep(r, c);
            ++_movesNumber;
        }

        private void DrawCellsForBotStep(short row, short col)
        {
            _cells[row, col] = _opposite;
            Thread.Sleep(500);
            switch (row)
            {
                case 0:
                    switch (col)
                    {
                        case 0:
                            Cell_0_0.Image = _opposite == 1 ? _imgX : _imgO;
                            break;
                        case 1:
                            Cell_0_1.Image = _opposite == 1 ? _imgX : _imgO;
                            break;
                        case 2:
                            Cell_0_2.Image = _opposite == 1 ? _imgX : _imgO;
                            break;
                    }

                    break;
                case 1:
                    switch (col)
                    {
                        case 0:
                            Cell_1_0.Image = _opposite == 1 ? _imgX : _imgO;
                            break;
                        case 1:
                            Cell_1_1.Image = _opposite == 1 ? _imgX : _imgO;
                            break;
                        case 2:
                            Cell_1_2.Image = _opposite == 1 ? _imgX : _imgO;
                            break;
                    }

                    break;
                case 2:
                    switch (col)
                    {
                        case 0:
                            Cell_2_0.Image = _opposite == 1 ? _imgX : _imgO;
                            break;
                        case 1:
                            Cell_2_1.Image = _opposite == 1 ? _imgX : _imgO;
                            break;
                        case 2:
                            Cell_2_2.Image = _opposite == 1 ? _imgX : _imgO;
                            break;
                    }

                    break;
            }
        }

        private void ShowWinner(short a)
        {
            Invalidate();
            Update();
            Thread.Sleep(250);
            FieldVisible(false);
            if (_xOrO == a)
            {
                Winner_text.ForeColor = Color.ForestGreen;
                Winner_text.Text = "Вы выиграли!!!";
            }
            else
            {
                Winner_text.Text = "Вы проиграли!";
                Winner_text.ForeColor = Color.Red;
            }

            MainMenu_gr.Visible = true;
            Winner_gr.Visible = true;
        }
    }
}