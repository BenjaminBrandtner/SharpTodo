using HabiticaSimpleToDo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class VMMainWindow
    {
        private ObservableCollection<VMHabiticaTodo> todoList;
        private HabiticaTodoCollection todocollection;
        private ICommand fetchCommand;

        public VMMainWindow()
        {
            //todoList = new ObservableCollection<VMHabiticaTodo>();
            FetchCommand = new UserCommand(new Action<object>(FetchTodos));
            todocollection = new HabiticaTodoCollection();
        }

        private async void FetchTodos(object o)
        {
            Console.WriteLine("test");
            await todocollection.deserializeAllTodos();
            //TodoList = new ObservableCollection<VMHabiticaTodo>(((List<HabiticaTodo>)todocollection.TodoList));
        }
        public ObservableCollection<VMHabiticaTodo> TodoList { get => todoList; set => todoList = value; }
        public ICommand FetchCommand { get => fetchCommand; set => fetchCommand = value; }
    }
}
