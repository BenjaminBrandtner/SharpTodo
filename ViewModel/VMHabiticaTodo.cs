﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HabiticaSimpleToDo;

namespace ViewModel
{
    public class VMHabiticaTodo :INotifyPropertyChanged
    {
        private HabiticaTodo todo;

        public VMHabiticaTodo(HabiticaTodo todo)
        {
            this.Todo = todo;
        }

        public string Text { get => Todo.Text; set => Todo.Text = value; }
        public string Notes { get => Todo.Notes; set => Todo.Notes = value; }

        public  Boolean Completed { get => Todo.Completed; set
            {
                Todo.Completed = value;
                
                ChangeCompletionStatus(value);
            }
        }

        private async void ChangeCompletionStatus(bool value)
        {
            HabiticaClient client = HabiticaClient.GetInstance();
            if (value == true)
            {
                await client.CheckOffTodo(this.Todo);
            }
            else
            {
                await client.UncheckTodo(this.Todo);
            }
        }

        public DateTime DueDate { get => Todo.Date; set => Todo.Date = value; }
        public HabiticaTodo Todo { get => todo; set => todo = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return Todo.ToString();
        }
    }
}