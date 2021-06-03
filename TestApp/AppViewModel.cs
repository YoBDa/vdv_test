using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Data.Entity;


namespace TestApp
{
    class AppViewModel : INotifyPropertyChanged
    {

        /*private Unit _SelectedUnit;
        private Employee _SelectedEmployee;
        private Order _SelectedOrder;*/


        //Collections to bind
        public ObservableCollection<Unit> Units { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }
        public ObservableCollection<Order> Orders { get; set; }


        //Selected Entities Props
        /*public Unit SelectedUnit
        {
            get { return _SelectedUnit; }
            set
            {
                _SelectedUnit = value;
                OnPropertyChanged("SelectedUnit");
            }
        }
        public Order SelectedOrder
        {
            get { return _SelectedOrder; }
            set
            {
                _SelectedOrder = value;
                OnPropertyChanged("SelectedOrder");
            }
        }
        public Employee SelectedEmployee
        {
            get { return _SelectedEmployee; }
            set
            {
                _SelectedEmployee = value;
                OnPropertyChanged("SelectedEmployee");
            }
        }*/

        public AppViewModel()
        {
            Units = new ObservableCollection<Unit>();
            Employees = new ObservableCollection<Employee>();
            Orders = new ObservableCollection<Order>();
            ReloadCollections();
        }

        //Collections reload method
        private void ReloadCollections()
        {
            using (TestContext tc = new TestContext())
            {
                tc.Units.Load();
                tc.Orders.Load();
                tc.Employees.Load();
                Units.Clear();
                Orders.Clear();
                Employees.Clear();
                foreach (var unit in tc.Units.Local)
                    Units.Add(unit);
                foreach (var order in tc.Orders.Local)
                    Orders.Add(order);
                foreach (var employee in tc.Employees.Local)
                    Employees.Add(employee);
            }
            OnPropertyChanged("");
        }





        //Do ADD/EDIT/REMOVE operations here
        //reload collections every time


        //in edit methods used expressions like this:
        //
        //var existing = tc.Entity.Find(entity.Id);
        //tc.Entry(existing).CurrentValues.SetValues(entity);
        //
        //because expression tc.Entry(entity).State = EntityState.Modified doesn't work

        #region DBOps
        public string AddEmployee(Employee employee)
        {
            using (TestContext tc = new TestContext())
            {
                string result = "";
                if (!Validate(employee, out result))
                    return result;

                tc.Employees.Add(employee);

                try
                {

                    tc.SaveChanges();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            ReloadCollections();
            return "Successfuly added employee";
        }


        public string EditEmployee(Employee employee)
        {
           using (TestContext tc = new TestContext())
            {
                tc.Employees.Load();
                string result = "";
                if (!Validate(employee, out result))
                    return result;

                var existing = tc.Employees.Find(employee.Id);
                tc.Entry(existing).CurrentValues.SetValues(employee);

                try
                {
                    tc.SaveChanges();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            ReloadCollections();
            return "Successfuly edited employee";
        }
        public string RemoveEmployee(Employee employee)
        {
            using (TestContext tc = new TestContext())
            {
                tc.Units.Load();
                tc.Orders.Load();
                tc.Employees.Load();
                //Protecting from removing of busy (dependent) employees
                var orders = from o in tc.Orders.Local
                             where o.AuthorId == employee.Id
                             select o;
                var units = from u in tc.Units.Local
                            where u.HeadId == employee.Id
                            select u;

                StringBuilder sborders = new StringBuilder("Cannot remove employee. Employee is author of these orders: \n");
                StringBuilder sbunits = new StringBuilder("Cannot remove employee. Employee is head of these units: \n");
                foreach (Order order in orders)
                    sborders.Append(order.ToString() + '\n');
                foreach (Unit unit in units)
                    sbunits.Append(unit.ToString() + '\n');

                if (orders.Count() > 0) { return sborders.ToString(); }
                if (units.Count() > 0) { return sbunits.ToString(); }

                employee = tc.Employees.Find(employee.Id);
                tc.Employees.Remove(employee);

                try
                {
                    tc.SaveChanges();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            ReloadCollections();
            return "Sucessfully removed employee";
        }
        public string AddUnit(Unit unit)
        {
            using (TestContext tc = new TestContext())
            {
                string result = "";
                if (!Validate(unit, out result))
                    return result;

                tc.Units.Add(unit);
                tc.SaveChanges();
                try
                {

                    tc.SaveChanges();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            ReloadCollections();
            return "Successfuly added unit";

        }

        public string EditUnit(Unit unit)
        {
            using (TestContext tc = new TestContext())
            {
                tc.Units.Load();
                string result = "";
                if (!Validate(unit, out result))
                    return result;

                var existing = tc.Units.Find(unit.Id);
                tc.Entry(existing).CurrentValues.SetValues(unit);
                try
                {
                    tc.SaveChanges();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            ReloadCollections();
            return "Successfully edited unit";
        }
        public string RemoveUnit(Unit unit)
        {
            using (TestContext tc = new TestContext())
            {
                tc.Units.Load();
                tc.Employees.Load();
                //protecting from removing busy (dependent) units
                var employees = tc.Employees.Local.Where(emp => emp.UnitId == unit.Id);

                StringBuilder sbemployees = new StringBuilder("Cannot remove unit. Unit contains these employees: \n");
                foreach (Employee employee in employees)
                    sbemployees.Append(employee.ToString() + '\n');

                if (employees.Count() > 0) { return sbemployees.ToString(); }

                unit = tc.Units.Find(unit.Id);
                tc.Units.Remove(unit);
                try
                {
                    tc.SaveChanges();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            ReloadCollections();
            return "Successfully removed unit";
        }
        public string AddOrder(Order order)
        {
            using (TestContext tc = new TestContext())
            {
                string result = "";
                if (!Validate(order, out result))
                    return result;
                tc.Orders.Add(order);

                try
                {
                    tc.SaveChanges();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            ReloadCollections();
            return "Successfully added order";
        }
        public string EditOrder(Order order)
        {
            using (TestContext tc = new TestContext())
            {
                tc.Orders.Load();
                string result = "";
                if (!Validate(order, out result))
                    return result;
                var existing = tc.Orders.Find(order.Id);
                tc.Entry(existing).CurrentValues.SetValues(order);
                try
                {
                    tc.SaveChanges();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            ReloadCollections();
            return "Successfully edited order";
        }
        public string RemoveOrder(Order order)
        {
            using (TestContext tc = new TestContext())
            {
                tc.Orders.Load();
                order = tc.Orders.Find(order.Id);
                tc.Orders.Remove(order);

                try
                {
                    tc.SaveChanges();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            ReloadCollections();
            return "Successfully removed order";
        }
        #endregion


        //validation method
        private bool Validate<T>(in T Entity, out string Result) where T: IValidatableObject
        {
            var valResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var valContext = new ValidationContext(Entity);

            if (!Validator.TryValidateObject(Entity, valContext, valResults, true))
            {
                StringBuilder sb = new StringBuilder();
                foreach (var result in valResults)
                {
                    sb.Append(result + "\n");
                }
                Result = sb.ToString();
                return false;
            }
            Result = "";
            return true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
