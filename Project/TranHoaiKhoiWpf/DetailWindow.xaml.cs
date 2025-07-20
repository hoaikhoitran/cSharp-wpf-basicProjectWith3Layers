using BusinessLayer.Services;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer;
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
using System.Windows.Shapes;

namespace TranHoaiKhoiWpf
{
    /// <summary>
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        private ResearchProjectService _service = new(); 
        private ResearcherService _researcherService = new(); 

        public ResearchProject EdittedProject { get; set; } = null;
        //biến này là biến cờ,
        //nếu nó đc giơ lên, nghĩa là null, nghĩa là mành hình này là màng hình tạo mới
        //nếu biến này phất lên bằng 1 project nào có sẵn(đuọcw truyền từ grid đem sang)
        //do user chọn dòng muốn edit => EdittedProject = dòng bên main 
        //nó != null => ta đã có máy lạnh muốn edit, khi đó ta đổ vào các ô nhập

        public DetailWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(!ValidateElements())
                return; //nếu validate không thành công thì return, không thực hiện tiếp

            //gọi service để lưu đối tượng xuống table, phải new() trước
            ResearchProject x = new();
            x.ProjectId = int.Parse(IdTextBox.Text);
            x.ProjectTitle = TitleTextBox.Text;
            x.ResearchField = FieldTextBox.Text;
            x.StartDate = DateOnly.FromDateTime(StartDatePicker.SelectedDate.Value);
            x.EndDate = DateOnly.FromDateTime(EndDatePicker.SelectedDate.Value);
            x.Budget = decimal.Parse(BudgetTextBox.Text);
            //x.LeadResearcherId = 204;
            x.LeadResearcherId = int.Parse(ResearcherIdComboBox.SelectedValue.ToString());
            //kiếm tra cờ để xem hình huống nào add và edit
            if(EdittedProject == null)
            {
                _service.AddResearchProject(x);
            }
            else
            {
                _service.UpdateResearchProject(x);
            }
                this.Close(); 
        }

        private bool ValidateElements()
        {
            if (TitleTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("The title is required!", "Field required", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Start Date and End Date are required!", "Field required", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            DateTime start = StartDatePicker.SelectedDate.Value;
            DateTime end = EndDatePicker.SelectedDate.Value;

            if (start > end)
            {
                MessageBox.Show("Start Date must be before or equal to End Date!", "Invalid Date Range", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (FieldTextBox.Text.IsNullOrEmpty()) 
            {
                MessageBox.Show("Field research are required!", "Field required", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (BudgetTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Budget are required!", "Field required", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            decimal q;

            if (!decimal.TryParse(BudgetTextBox.Text, out q))
            {
                MessageBox.Show("Budget must be a valid number!", "Invalid Budget", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (ResearcherIdComboBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Researcher are required!", "Field required", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (IdTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Id are required!", "Field required", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBox();
            FillElements(EdittedProject);
            //fill label cho rõ màn hình làm gì thuộc chế độ add hay edit
            if(EdittedProject != null)
            {
                DetailWindowModeLabel.Content = "Update Research Project";
            }
            else
            {
                DetailWindowModeLabel.Content = "Create New Research Project";
            }
        }

        //Hàm này đổ vào các ô nhập
        private void FillElements(ResearchProject x)
        {
            if(x == null)
            {
                return;
            }
            //khác null mới đổ vào các ô nhập
            else
            {
                IdTextBox.Text = x.ProjectId.ToString();
                //disable ô nhập id
                IdTextBox.IsEnabled = false;
                TitleTextBox.Text = x.ProjectTitle;
                FieldTextBox.Text = x.ResearchField;
                StartDatePicker.SelectedDate = x.StartDate.ToDateTime(TimeOnly.MinValue);
                EndDatePicker.SelectedDate = x.EndDate.ToDateTime(TimeOnly.MinValue);
                BudgetTextBox.Text = x.Budget.ToString();
                ResearcherIdComboBox.SelectedValue = x.LeadResearcherId;
            }
        }

        //hàm helper để đổ data vào combobox
        private void FillComboBox()
        {
            ResearcherIdComboBox.ItemsSource = _researcherService.GetAllResearchers();
            //hiển thị column nào
            ResearcherIdComboBox.DisplayMemberPath = "FullName"; 
            //khi chọn thì lấy value là id để làm FK
            ResearcherIdComboBox.SelectedValuePath = "ResearcherId";
            //khi user chọn cột, thì Id này sẽ được đổ vào prop SelectedValue
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
