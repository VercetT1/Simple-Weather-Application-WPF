
using System.Collections.ObjectModel;
using System.ComponentModel;
using WeatherAPP.Model;
using WeatherAPP.ViewModel.Commands;
using WeatherAPP.ViewModel.Helpers;

namespace WeatherAPP.ViewModel
{
    internal class WeatherViewModel : INotifyPropertyChanged

    {
        private string query;
        private CurrentCondition currentCondition;
        private City selectedCity;
        public event PropertyChangedEventHandler PropertyChanged;

        public WeatherViewModel()
        {
          /*  if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                SelectedCity = new City
                {
                    LocalizedName = "New York"
                };
                CurrentCondition = new CurrentCondition
                {
                    WeatherText = "Partly cloudy",
                    Temperature = new Temperature
                    {
                        Metric = new Units
                        {
                            Value = "21",

                        }
                    }
                };
            }*/

            SearchCommand = new SearchCommand(this);
            Cities = new ObservableCollection<City>();

        }


        public SearchCommand SearchCommand { get; set; }
        public string Query
        {
            get { return query; }
            set
            {
                query = value;
                OnPropertyChanged("Query");
            }
        }
        public ObservableCollection<City> Cities { get; set; }
        public CurrentCondition CurrentCondition
        {
            get { return currentCondition; }
            set
            {
                currentCondition = value;
                OnPropertyChanged("CurrentCondition");
            }
        }
        public City SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged("SelectedCity");
                GetCurrentConditions();
            }
        }



        private async void GetCurrentConditions()
        {
            Query = string.Empty;
            Cities.Clear();
            CurrentCondition = await AccuWeatherHelper.GetCurrentConditions(SelectedCity.Key);
        }


        public async void MakeQuery()
        {
            var cities = await AccuWeatherHelper.GetCities(Query);

            Cities.Clear();

            foreach (var city in cities)
            {
                Cities.Add(city);
            }

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
