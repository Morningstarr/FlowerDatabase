﻿using System;
using System.Collections.Generic;
using System.Data;
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

namespace FlowerClient
{
    /// <summary>
    /// Логика взаимодействия для DetailedDesc.xaml
    /// </summary>
    public partial class DetailedDesc : Window
    {
        public DetailedDesc()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Card context = DataContext as Card;
            loadReference("author");
            author.SelectedIndex = author.Items.IndexOf(context.authorP);
            loadReference("exposition");
            exposition.SelectedIndex = exposition.Items.IndexOf(context.expositionP);
            loadReference("life_form");
            life_form.SelectedIndex = life_form.Items.IndexOf(context.lifeFormP);
            loadReference("species_name");
            species_name.SelectedIndex = species_name.Items.IndexOf(context.speciesNameP);
            loadReference("group");
            group.SelectedIndex = group.Items.IndexOf(context.groupP);
            loadReference("econ_group");
            econ_group.SelectedIndex = econ_group.Items.IndexOf(context.economicGroupP);
            loadReference("people");
            people.SelectedIndex = people.Items.IndexOf(context.peopleP);
            loadReference("history");
            history.SelectedIndex = history.Items.IndexOf(context.historyP);
            loadReference("buildings");
            buildings.SelectedIndex = buildings.Items.IndexOf(context.buildingsP);
            loadReference("category");
            category.SelectedIndex = category.Items.IndexOf(context.categoryP);

            fillYearsSeasons();
            year.SelectedIndex = year.Items.IndexOf(Convert.ToInt32(context.yearP));
            season.SelectedIndex = season.Items.IndexOf(context.seasonP);

        }

        private void fillYearsSeasons()
        {
            List<int> years = new List<int>();
            List<string> seasons = new List<string>();

            for (int i = 2017; i < 2031; i++)
            {
                years.Add(i);
            }
            seasons.Add("Весна");
            seasons.Add("Лето");
            seasons.Add("Осень");
            seasons.Add("Зима");

            season.ItemsSource = seasons;
            year.ItemsSource = years;
        }
        private void loadReference(string refName)
        {
            Mediator.instance.SQL = "select * from " + refName + "_view";
            DataTable results = Mediator.instance.ExecuteQuery();
            List<string> temp = new List<string>();
            for (int i = 0; i < results.Rows.Count; i++)
            {
                temp.Add(results.Rows[i].ItemArray[0].ToString());
            }
            ComboBox c = FindName(refName) as ComboBox;
            c.ItemsSource = temp;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private string itemToString(string name)
        {
            ComboBox c = FindName(name) as ComboBox;
            //ComboBoxItem item = (ComboBoxItem)c.SelectedItem;
            return c.SelectedItem.ToString();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Card temp = (Card)this.DataContext;
                Mediator.instance.SQL = "select * from update_plant('" + itemToString("author") + "','" +
                    itemToString("exposition") + "','" + itemToString("species_name") + "','" +
                    itemToString("life_form") + "','" + itemToString("group") +
                    "','" + itemToString("econ_group") + "','" + itemToString("people") +
                    "','" + itemToString("history") + "','" + itemToString("buildings") +
                    "','" + itemToString("category") + "'," + itemToString("year") +
                    ",'" + itemToString("season") + "'," + temp.idP + ");";

                Mediator.instance.Execute();
                new MsgBox("Запись обновлена!", "Успешно!").ShowDialog();
                DialogResult = true;
                this.Close();
            }
            catch(Exception ex)
            {
                new MsgBox(ex.Message, "Ошибка").ShowDialog();
            }
        }
    }
}
