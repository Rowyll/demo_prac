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

namespace PraktikaActivity
{
    /// <summary>
    /// Логика взаимодействия для Organaizer.xaml
    /// </summary>
    public partial class Organaizer : Window
    {
        public Organaizer()
        {
            InitializeComponent();

            Users user = new Users();

            using(ActivityEntities activityEntities = new ActivityEntities())
            {
                user = activityEntities.Users.Where(x => x.Id == CurrentUser.currentUserId).FirstOrDefault();
            }

            DataContext = user;

            if (DateTime.Now > DateTime.Parse("9:00:00") && DateTime.Now < DateTime.Parse("11:00:00"))
            {
                greeting_text.Text = "Доброе утро!";
            }
            else if (DateTime.Now > DateTime.Parse("11:01:00") && DateTime.Now < DateTime.Parse("18:00:00"))
            {
                greeting_text.Text = "Добрый день!";
            }
            else if (DateTime.Now > DateTime.Parse("18:01:00") && DateTime.Now < DateTime.Parse("23:59:59"))
            {
                greeting_text.Text = "Добрый вечер!";
            }
            using (ActivityEntities db = new ActivityEntities())
            {
                Users user1 = db.Users.Where(x => x.Id == CurrentUser.currentUserId).First();
                string[] fio = user1.FullName.Split();
                username.Text = fio[1] + ' ' + fio[2];
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RgistrationOfJuryModerator rgistrationOfJuryModerator = new RgistrationOfJuryModerator();
            rgistrationOfJuryModerator.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Profile profile = new Profile();
            profile.Show();
            this.Close();
        }
    }
}
