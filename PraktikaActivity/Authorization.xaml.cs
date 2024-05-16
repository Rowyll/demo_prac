using EasyCaptcha.Wpf;
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
using System.IO;

namespace PraktikaActivity
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public static readonly string appdata = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "userLoginDir");
        string answer = "";
        int counter = 0;

        public Authorization()
        {
            InitializeComponent();
            RemadeCaptcha();

            if (File.Exists(Path.Combine(appdata, "auth.txt")))
            {
                var data = File.ReadAllText(Path.Combine(appdata, "auth.txt")).Split(' ');
                try
                {
                    int id = Convert.ToInt32(data[0]);
                    var user = Authorize(id, data[1]);
                    if (user != null)
                    {
                        OpenUserWindow(user);
                    }
                }
                catch
                {
                    
                }
            }
        }

        private void RemadeCaptcha()
        {
            AuthCaptcha.CreateCaptcha(Captcha.LetterOption.Alphanumeric, 4);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            counter++;
            using (ActivityEntities db = new ActivityEntities())
            {
                answer = Answer.Text;
                if (answer != AuthCaptcha.CaptchaText)
                {
                    MessageBox.Show("Неверная CAPTCHA");
                }
                else
                {
                    int id = 0;
                    try
                    {
                        id = Convert.ToInt32(IdNumberText.Text);

                        var result = Authorize(id, PasswordText.Password);
                        if (result != null)
                        {
                            OpenUserWindow(result);
                        }
                        else
                        {
                            MessageBox.Show("Неправильный email или пароль");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("В поле id было введено не число");
                    }
                }
            }
            if (counter % 3 == 0)
            {
                this.IsEnabled = false;
                await Task.Delay(10000);
                this.IsEnabled = true;
            }
        }

        public static Users Authorize(int IdNumber, string Password)
        {
            using(ActivityEntities activityEntities = new ActivityEntities())
            {
                Users user = activityEntities.Users.Where(x => x.Id == IdNumber && x.Password == Password).FirstOrDefault();

                return user;
            }
        }

        public void OpenUserWindow(Users user)
        {
            if (!Directory.Exists(appdata))
            {
                Directory.CreateDirectory(appdata);
            }

            File.WriteAllText(Path.Combine(Path.Combine(appdata, "auth.txt")), user.Id + " " + user.Password);

            CurrentUser.currentUserId = user.Id;

            if (user.RoleId == 1)
            {
                Participant participant = new Participant();
                participant.Show();
                this.Close();
            }
            else if (user.RoleId == 2)
            {
                Moderator moderator = new Moderator();
                moderator.Show();
                this.Close();
            }
            else if (user.RoleId == 3)
            {
                Jury jury = new Jury();
                jury.Show();
                this.Close();
            }
            else if (user.RoleId == 4)
            {
                Organaizer organaizer = new Organaizer();
                organaizer.Show();
                this.Close();
            }
        }
    }
}
