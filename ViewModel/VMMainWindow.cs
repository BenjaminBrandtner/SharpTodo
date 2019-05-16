using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class VMMainWindow
    {
        private ObservableCollection<VMHabiticaTodo> todoList;

        public VMMainWindow()
        {
            todoList = new ObservableCollection<VMHabiticaTodo>();
        }

        public ObservableCollection<VMHabiticaTodo> TodoList { get => todoList; set => todoList = value; }
    }
}
