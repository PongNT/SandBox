using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace WebFormSandBox.Data
{
    public class Car
    {
        public Car(string name, double price, string url, Car parent)
        {
            _name = name;
            _price = price;
            _url = url;
            Parent = parent;
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private double _price;
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }
        private string _url;
        public string URL
        {
            get { return _url; }
            set { _url = value; }
        }

        public Car Parent { get; set; }
        public string ParentName => Parent?.Name;
    }
}