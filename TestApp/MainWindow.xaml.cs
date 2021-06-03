using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AppViewModel apv = new AppViewModel();
        public MainWindow()
        {
            InitializeComponent();

            //Gender comboBoxes filling
            cbEmpAddGender.Items.Add(Gender.Male);
            cbEmpAddGender.Items.Add(Gender.Female);
            cbEmpAddGender.Items.Add(Gender.Unknown);
            cbEmpAddGender.SelectedIndex = 2;

            cbEmpEditGender.Items.Add(Gender.Male);
            cbEmpEditGender.Items.Add(Gender.Female);
            cbEmpEditGender.Items.Add(Gender.Unknown);
            cbEmpEditGender.SelectedIndex = 2;
            //data context init
            DataContext = apv;
            apv.PropertyChanged += Apv_PropertyChanged;
            Apv_PropertyChanged(null, null);
        }

        //appViewModel event handler
        private void Apv_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            //need for free number finding
            int number = 0;
            if (apv.Orders.Count > 0)
            {
                number = apv.Orders.Last()?.Number ?? 0;
                do number += 1;
                while (apv.Orders.Contains(new Order() { Number = number }));
                    
            }
            lbUnitAddFreeNumber.Content = number.ToString();

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //binding data here

            cbEmpAddUnitSelector.ItemsSource = apv.Units;
            cbEmpEditUnitSelector.ItemsSource = apv.Units;
            //cbEmpEditUnitSelector.SelectedItem = apv.SelectedEmployee?.Unit;
            cbEmpEditSelector.ItemsSource = apv.Employees;
            //cbEmpEditSelector.SelectedItem = apv.SelectedEmployee;


            cbUnitAddHeadSelector.ItemsSource = apv.Employees;
            cbUnitEditHeadSelector.ItemsSource = apv.Employees;
            //cbUnitEditHeadSelector.SelectedItem = apv.SelectedUnit?.Head;
            cbUnitEditSelector.ItemsSource = apv.Units;
            //cbUnitEditSelector.SelectedItem = apv.SelectedUnit;


            cbOrderAddAuthorSelector.ItemsSource = apv.Employees;
            cbOrderEditAuthorSelector.ItemsSource = apv.Employees;
            //cbOrderEditAuthorSelector.SelectedItem = apv.SelectedOrder?.Author;
            cbOrderEditSelector.ItemsSource = apv.Orders;
            //cbOrderEditSelector.SelectedItem = apv.SelectedOrder;
            

        }

        private void MainWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }



        #region Employee_methods
        private void btEmpAddAddButton_Click(object sender, RoutedEventArgs e)
        {
            //insert employee to db
            string Name = tbEmpAddName.Text.Trim();
            string Surname = tbEmpAddSurname.Text.Trim();
            string Patronymic = tbEmpAddPatronymic.Text.Trim();
            DateTime? Birthdate = dpEmpAddBirthdate.SelectedDate;
            Gender Gender = (Gender)cbEmpAddGender.SelectedIndex;
            Unit unit = (Unit)cbEmpAddUnitSelector.SelectedItem;

            Employee emp = new Employee
            {
                Name = Name,
                Surname = Surname,
                Patronymic = Patronymic,
                BirthDate = Birthdate,
                Gender = Gender,
                UnitId = unit.Id

            };
            MessageBox.Show(apv.AddEmployee(emp));
            
        }
        private void cbEmpEditSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //load employee info
            ComboBox cb = (ComboBox)sender;
            if (cb.SelectedIndex == -1) return;
            Employee emp = (Employee)cb.SelectedItem;
            tbEmpEditName.Text = emp.Name;
            tbEmpEditSurname.Text = emp.Surname;
            tbEmpEditPatronymic.Text = emp.Patronymic;
            dpEmpEditBirthdate.SelectedDate = emp.BirthDate;
            cbEmpEditGender.SelectedItem = emp.Gender;
            cbEmpEditUnitSelector.SelectedItem = emp.Unit;


        }
        private void btEmpEditRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            //remove employee from db
            if (cbEmpEditSelector.SelectedItem == null) { MessageBox.Show("Select employee to remove it"); return; }
            Employee emp = (Employee)cbEmpEditSelector.SelectedItem;
            MessageBox.Show(apv.RemoveEmployee(emp));


        }
        private void btEmpEditEditButton_Click(object sender, RoutedEventArgs e)
        {
            //update 
            string Name = tbEmpEditName.Text.Trim();
            string Surname = tbEmpEditSurname.Text.Trim();
            string Patronymic = tbEmpEditPatronymic.Text.Trim();
            DateTime? Birthdate = dpEmpEditBirthdate.SelectedDate;
            Gender Gender = (Gender)cbEmpEditGender.SelectedIndex;
            Employee emp = (Employee)cbEmpEditSelector.SelectedItem;
            Unit unit = (Unit)cbEmpEditUnitSelector.SelectedItem;
            emp.Name = Name;
            emp.Surname = Surname;
            emp.Patronymic = Patronymic;
            emp.Gender = Gender;
            emp.BirthDate = Birthdate.Value;
            emp.UnitId = unit.Id;
            MessageBox.Show(apv.EditEmployee(emp));
        }


        #endregion
        #region Unit_methods

        private void btUnitAddAddButton_Click(object sender, RoutedEventArgs e)
        {
            //insert unit to db
            string Name = tbUnitAddName.Text.Trim();
            if (Name.Length == 0) { MessageBox.Show("Empty name field."); return; }
            Employee head = (Employee)cbUnitAddHeadSelector.SelectedItem;

            Unit unit = new Unit
            {
                Name = Name,
                HeadId = head?.Id
            };
            MessageBox.Show(apv.AddUnit(unit));

        }

        private void cbUnitEditSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //load unit info
            ComboBox cb = (ComboBox)sender;
            if (cb.SelectedIndex == -1) return;
            Unit unit = (Unit)cb.SelectedItem;
            tbUnitEditName.Text = unit.Name;
            cbUnitEditHeadSelector.SelectedItem = unit.Head;
        }
        private void btUnitEditRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            //remove unit from db
            if (cbUnitEditSelector.SelectedItem == null) { MessageBox.Show("Select employee to remove it"); return; }
            Unit unit = (Unit)cbUnitEditSelector.SelectedItem;
            MessageBox.Show(apv.RemoveUnit(unit));


        }

        private void btUnitEditEditButton_Click(object sender, RoutedEventArgs e)
        {
            //update unit
            string Name = tbUnitEditName.Text.Trim();
            if (Name.Length == 0) { MessageBox.Show("Empty name field."); return; }
            Employee Head = (Employee)cbUnitEditHeadSelector.SelectedItem;
            Unit unit = (Unit)cbUnitEditSelector.SelectedItem;
            unit.Name = Name;
            unit.HeadId = Head?.Id;
            MessageBox.Show(apv.EditUnit(unit));

        }


        #endregion
        #region Order_methods
        private void btOrderAddAddButton_Click(object sender, RoutedEventArgs e)
        {
            //insert order to db
            int Number = 0;
            if (!int.TryParse(tbOrderAddNumber.Text.Trim(), out Number)) { MessageBox.Show("Wrong number field."); return; }
            string Counterparty = tbOrderAddCounterparty.Text.Trim();
            DateTime? Orderdate = dpOrderAddOrderdate.SelectedDate;
            Employee author = (Employee)cbOrderAddAuthorSelector.SelectedItem;

            Order order = new Order
            {
                Number = Number,
                Counterparty = Counterparty,
                OrderDate = Orderdate,
                AuthorId = author.Id
            };
            MessageBox.Show(apv.AddOrder(order));
        }

        private void cbOrderEditSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //load order info
            ComboBox cb = (ComboBox)sender;
            if (cb.SelectedIndex == -1) return;
            Order ord = (Order)cb.SelectedItem;
            tbOrderEditNumber.Text = ord.Number.ToString();
            tbOrderEditCounterparty.Text = ord.Counterparty;
            dpOrderEditOrderdate.SelectedDate = ord.OrderDate;
            cbOrderEditAuthorSelector.SelectedItem = ord.Author;

        }
        private void btOrderEditRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            //remove order
            if (cbOrderEditSelector.SelectedItem == null) { MessageBox.Show("Select order to remove it"); return; }
            Order order = (Order)cbOrderEditSelector.SelectedItem;
            MessageBox.Show(apv.RemoveOrder(order));

        }

        private void btOrderEditEditButton_Click(object sender, RoutedEventArgs e)
        {
            //update order
            int Number = 0;
            if (!int.TryParse(tbOrderEditNumber.Text.Trim(), out Number)) { MessageBox.Show("Wrong number field."); return; }
            string Counterparty = tbOrderEditCounterparty.Text.Trim();
            DateTime? Orderdate = dpOrderEditOrderdate.SelectedDate;
            Employee Author = (Employee)cbOrderEditAuthorSelector.SelectedItem;

            Order order = (Order)cbOrderEditSelector.SelectedItem;
            order.Number = Number;
            order.Counterparty = Counterparty;
            order.OrderDate = Orderdate.Value;
            order.AuthorId = Author.Id;

            MessageBox.Show(apv.EditOrder(order));

        }


        #endregion

        //free number changer
        private void lbUnitAddFreeNumber_MouseDown(object sender, MouseButtonEventArgs e)
        {
            tbOrderAddNumber.Text = lbUnitAddFreeNumber.Content.ToString();
        }
    }
}
