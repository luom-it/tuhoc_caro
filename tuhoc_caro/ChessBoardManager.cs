using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace tuhoc_caro
{
    public class ChessBoardManager
    {
        #region properties
        private Panel chessBoard;
        public Panel ChessBoard
        {
            get { return chessBoard; }
            set { chessBoard = value; }
        }

        //tạo 1 mảng danh sách người chơi
        private List<Player> player;
        public List<Player> Player
        { 
            get { return player;}
            set { player = value; }
        }

        

        //biến lưu lại xem ai là người đánh
        private int currentPlayer;
        public int CurrentPlayer { 
            get => currentPlayer; 
            set => currentPlayer = value; 
        }


        private TextBox playerName;
        public TextBox PlayerName 
        { 
            get => playerName;
            set => playerName = value; 
        }

        private PictureBox playerMark;
        public PictureBox PlayerMark 
        { 
            get => playerMark; 
            set => playerMark = value; 
        }

        //mảng lòng trong mảng
        private List<List<Button>> Matrix;
        public List<List<Button>> Matrix1 
        { 
            get => Matrix; 
            set => Matrix = value; 
        }
        #endregion

        #region Initialize
        public ChessBoardManager(Panel chessBoard, TextBox playerName, PictureBox mark) 
        {
            this.ChessBoard = chessBoard;
            this.PlayerName = playerName;
            this.PlayerMark = mark;
            this.Player = new List<Player>()
            {
                new Player("Player_1", Image.FromFile(Application.StartupPath + "\\Resources\\O.jpg")),
                new Player("Player_2", Image.FromFile(Application.StartupPath + "\\Resources\\X.jpg"))
            };
            CurrentPlayer = 0;
            //bắt đầu với người chơi ?
            ChangePlayer();
        }
        #endregion

        #region Methos
        public void DrawChessBoard()
        {
            //khởi tạo ma trận
            //tạo mảng
            Matrix = new List<List<Button>>();

            Button oldButton = new Button() { Width = 0, Location = new Point(0) };
            for (int i = 0; i < cons.CHESS_BOARD_HEIGTH; i++)
            {
                //tạo mảng để lưu lại các phần tử dùng cho việc xác định các phần tử xung quanh 
                Matrix.Add(new List<Button>());

                for (int j = 0; j < cons.CHESS_BOARD_WIDTH; j++)
                {
                    //tao button va gan dai va cao cho btn
                    Button btn = new Button()
                    {
                        //set độ rộng và cao cho ô(button)
                        Width = cons.CHESS_WIDTH,
                        Height = cons.CHESS_HEIGTH,
                        Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y),
                        //set kích thước ảnh bg phù hợp với button
                        BackgroundImageLayout = ImageLayout.Stretch,
                        //mẹo dùng tag để  xác định được vị trí của button 
                        Tag = i.ToString()
                    };
                    btn.Click += btn_Click;
                    //them vao panel co ten la pnlChessBoard
                    chessBoard.Controls.Add(btn);

                    Matrix[i].Add(btn);
                    //luu lai vi tri 
                    oldButton = btn;
                }
                oldButton.Location = new Point(0, oldButton.Location.Y + cons.CHESS_HEIGTH);
                oldButton.Width = 0;
                oldButton.Height = 0;
            }
        }
        void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            //kiểm tra nếu bg đang trống thì mới hiển thị bg
            if(btn.BackgroundImage != null)
            {
                return;
            }
            //gọi lại 2 hàm dưới lên sử dụng // tạo hàm để sau này dể sửa chữa
            Mark(btn);
            ChangePlayer();

            //kiểm tra EndGame
            if (isEndGame(btn))
            {
                //nếu dduk đúng game kết thức
                EndGame();
            }
            
        }
        private void EndGame()
        {
            MessageBox.Show("Game over!");
        }

        //hàm lấy tọa độ
        private Point getChessPoint(Button btn) 
        {
            int vertical = Convert.ToInt32(btn.Tag);
            int horizontal = Matrix[vertical].IndexOf(btn);

            Point point = new Point(horizontal,vertical);

            return point;
        }
        //case kiểm tra 
        private bool isEndGame(Button btn) 
        {
            return isEndHorizontal(btn) || isEndVertical(btn) || isEndPrimary(btn) || isEndSub(btn);
        }
        //case kiểm tra 1 (hàng ngang)
        private bool isEndHorizontal(Button btn)
        {
            Point point = getChessPoint(btn);
            int countLeft = 0;
            //đi về phía bên trái
            for(int i = point.X; i >= 0; i--)
            {
                //tại vị trí đó có bằng bg ta truyền vào hay không
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countLeft++;
                }
                else
                    break;
            }
            int countRight = 0;
            //đi về phía bên phải
            //tại vì point.x đã đếm 1 lần ở trên nên ta phải cộng thêm 1
            for (int i = point.X + 1; i < cons.CHESS_BOARD_WIDTH; i++)
            {
                //tại vị trí đó có bằng bg ta truyền vào hay không
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countRight++;
                }
                else
                    break;
            }
            return countLeft + countRight == 5;
        }
        //case kiểm tra 2 (hàng dọc)
        private bool isEndVertical(Button btn)
        {
            Point point = getChessPoint(btn);
            int countTop = 0;
            //đi lên
            for (int i = point.Y; i >= 0; i--)
            {
                //tại vị trí đó có bằng bg ta truyền vào hay không
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }
            int countBottom = 0;
            //đi xuống
            //tại vì point.x đã đếm 1 lần ở trên nên ta phải cộng thêm 1
            for (int i = point.Y + 1; i < cons.CHESS_BOARD_HEIGTH; i++)
            {
                //tại vị trí đó có bằng bg ta truyền vào hay không
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }
            return countTop + countBottom == 5;
        }
        //case kiểm tra 3 (đường chéo chính)
        private bool isEndPrimary(Button btn)
        {
            Point point = getChessPoint(btn);
            int countTop = 0;
      
            for (int i = 0; i <= point.X; i++)
            {
                //kiểm tra xem nó ra khỏi bản hay chưa
                if (point.X - i < 0 || point.Y - i < 0)
                    break;
                //tại vị trí đó có bằng bg ta truyền vào hay không
                if (Matrix[point.Y - i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }
            int countBottom = 0;
            
            for (int i = 1; i <= cons.CHESS_BOARD_WIDTH - point.X; i++)
            {
                //kiểm tra xem nó ra khỏi bản hay chưa
                if (point.Y + i >= cons.CHESS_BOARD_HEIGTH || point.X + i >= cons.CHESS_BOARD_WIDTH)
                    break;
                //tại vị trí đó có bằng bg ta truyền vào hay không
                if (Matrix[point.Y + i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }
            return countTop + countBottom == 5;
        }
        //case kiểm tra 4 (đường chéo phụ)
        private bool isEndSub(Button btn)
        {
            Point point = getChessPoint(btn);
            int countTop = 0;

            for (int i = 0; i <= point.X; i++)
            {
                //kiểm tra xem nó ra khỏi bản hay chưa
                if (point.X + i > cons.CHESS_BOARD_WIDTH || point.Y - i < 0)
                    break;
                //tại vị trí đó có bằng bg ta truyền vào hay không
                if (Matrix[point.Y - i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }
            int countBottom = 0;

            for (int i = 1; i <= cons.CHESS_BOARD_WIDTH - point.X; i++)
            {
                //kiểm tra xem nó ra khỏi bản hay chưa
                if (point.Y + i >= cons.CHESS_BOARD_HEIGTH || point.X - i < 0)
                    break;
                //tại vị trí đó có bằng bg ta truyền vào hay không
                if (Matrix[point.Y + i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }
            return countTop + countBottom == 5;
        }

        private void Mark(Button btn)
        {
            //bg ô hiện theo người chơi 
            btn.BackgroundImage = Player[CurrentPlayer].Mark;
            //đổi CurrentPlayer, đổi người chơi sau mỗi lượt đi
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
        }
        private void ChangePlayer()
        {
            //đổi tên người chơi(bên phải)
            PlayerName.Text = Player[CurrentPlayer].Name;
            //đổi ảnh (O hoặc X của người đang chơi quân cờ gì) người chơi(bên phải)
            PlayerMark.Image = Player[CurrentPlayer].Mark;
        }
        #endregion

    }
}
