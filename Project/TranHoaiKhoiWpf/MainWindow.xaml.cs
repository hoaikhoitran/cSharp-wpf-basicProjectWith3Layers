using BusinessLayer.Services;
using RepositoryLayer;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TranHoaiKhoiWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    //Class main window kế thừa class Window có sẵn trong .net sdk
    //Class main window này có 2 khả năng chính
    //- OOP Như bình thường
    //- Render ra màng hình như mọi cửa sổ của app khác
    //Thêm khả năng 3
    //- Vì có phần render nên ta có thể thay đổi các prop của cửa sổ
    //Thông qua code hoặc qua màng hình design kéo thả chuột, hoặc qua phần gõ tag theo style XAML - Tạm gọi
    //HTML/CSS kiểu của Microsoft
    //Nhưng tag này chỉ render ra UI cửa sổ chạy trên desktop
    public partial class MainWindow : Window
    {
        //Code chuẩn thì chõ này khai báo interface = New Implement
        //Dependency Injection
        private ResearchProjectService _service = new();

        //Ta cần chuyển giao account từ login sang màng hình này để chào và disable các nút
        public UserAccount CurrentAccount { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            DetailWindow detailWindow = new();
            //detailWindow.Show();
            detailWindow.ShowDialog();
            //F5 lại grid khi đòng lại cửa sổ detail window
            FillDataGrid();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
            if(CurrentAccount.Role == 2)
            {
                DeleteButton.IsEnabled = false; //disable nút xóa
            }
            if(CurrentAccount.Role == 3)
            {
                CreateButton.IsEnabled = false; //disable nút tạo mới
                UpdateButton.IsEnabled = false; //disable nút cập nhật
                DeleteButton.IsEnabled = false; //disable nút xóa
            }
        }

        //Hàm helper, trợ giúp cho các hàm khác

        private void FillDataGrid()
        {
            ResearchProjectDataGrid.ItemsSource = null;
            ResearchProjectDataGrid.ItemsSource = _service.GetAllResearchProjects();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            //ResearchProjectDataGrid.SelectedItem //biến object - nhưng đang là đối tượng đc lựa chọn -> casting, ép kiểu
            ResearchProject selected = ResearchProjectDataGrid.SelectedItem as ResearchProject;
            if(selected == null)
            {
                MessageBox.Show("Please select a project to update.", "Select a row", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            //chạy đến đây là đã chọn dòng , ta đẩy nó sang màng hình detail để edit đc gán value bên này
            DetailWindow detailWindow = new();
            detailWindow.EdittedProject = selected;  //2 chàng trỏ 1 nàng
            detailWindow.ShowDialog();
            //F5 lại grid khi đòng lại cửa sổ detail window
            FillDataGrid();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ResearchProject selected = ResearchProjectDataGrid.SelectedItem as ResearchProject;
            if (selected == null)
            {
                MessageBox.Show("Please select a project to delete.", "Select a row", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            MessageBoxResult answer = MessageBox.Show("Do you really want to delete?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.No)
                return;
            //đến đây muốn xóa thì nhờ service xóa
            _service.DeleteResearchProject(selected);
            //F5
            FillDataGrid();

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //Validation : nếu textbox search số mà ghõ chữ thì chửi ở đây
            //Chưởi bằng MessageBox OK rồi return; nếu tryParse không thành công

            var result = _service.SearchProjectsByTitleAndField(SearchTextBox.Text);
            //Đổ kết quả tìm kiếm vào DataGrid
            ResearchProjectDataGrid.ItemsSource = null;
            ResearchProjectDataGrid.ItemsSource = result;
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); //đóng app
        }

        

    }
}