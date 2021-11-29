using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Passpot.Business
{
    public class ValidPassword: INotifyPropertyChanged, IDataErrorInfo
    {


       public event PropertyChangedEventHandler PropertyChanged;
     
       public void OnPropertyChanged(PropertyChangedEventArgs e)
       {
           if (PropertyChanged != null)
                PropertyChanged(this, e);
     }
     
       private string firstName;
       private string lastName;
       private int age;
       private string email;
       private double salary;
     
      public double Salary
       {
           get 
          { 
                return salary; 
            }
          set 
          { 
               salary = value;
               OnPropertyChanged(new PropertyChangedEventArgs("Salary"));
            }
        }
    
       public string FirstName
      {
           get { return firstName; }
          set 
           { 
                firstName = value; 
               OnPropertyChanged(new PropertyChangedEventArgs("FirstName"));
           }
        }
     
        public string LastName
        {
            get { return lastName; }
            set
            {
               lastName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastName"));
            }
        }
    
        public string EMail
       {
            get { return email; }
           set
            {
               email = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EMail"));
           }
       }
   
       public int Age
       {
           get { return age; }
           set
            {
               age = value;
               OnPropertyChanged(new PropertyChangedEventArgs("Age"));
            }
       }
     
        public override string ToString()
      {
           return String.Format("{0} {1}", firstName, lastName);
       }
  
       string errors = null;
    
        public string Error
       {
          get { return errors; }
        }
    
        public string this[string columnName]
       {
           get
           {
               string result = null;
               if (columnName == "Age")
              {
                   if (Age <= 0)
                 {
                       result = "Age should be positive!";
                  }
                   else if (Age <= 16)
                   {
                      result = "You are too young!";
                   }
               }
             return result;
         }
       }


    }
}
