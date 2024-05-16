using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class RgistrationOfJuryModerator : Window
    {
        private Users newUser = new Users();

        private static Regex _phoneNumberRegex = new Regex(@"^\+7\((\d{3})\)-(\d{3})-(\d{2})-(\d{2})$");

        private string photoName;
        private string filePath;

        public RgistrationOfJuryModerator()
        {
            InitializeComponent();

            newUser.DateOfBirth = new DateTime(2000, 1, 1);
            using (ActivityEntities activityEntities = new ActivityEntities())
            {

                List<Genders> genders = new List<Genders>();
                genders = activityEntities.Genders.ToList();
                GenderComboBox.ItemsSource = genders;

                GenderComboBox.SelectedItem = 0;

                List<Directions> directions = new List<Directions>();
                directions = activityEntities.Directions.ToList();
                DirectionComboBox.ItemsSource = directions;
                DirectionComboBox.SelectedItem = 0;

                List<Roles> roles = new List<Roles>();
                roles = activityEntities.Roles.Where(x => x.Id == 2 || x.Id == 3).ToList();
                RoleComboBox.ItemsSource = roles;

                DataContext = newUser;

                int newID = (int)(activityEntities.Users.Max(x => (int?)x.Id) + 1);
                NumberId.Text = newID.ToString();
            }


        }

        public static bool IsPhoneNumberValid(string phoneNumber)
        {
            return _phoneNumberRegex.IsMatch(phoneNumber);
        }

        public static int ValidatePassword(string x)
        {
            bool hasDigit = false;
            bool hasLower = false;
            bool hasUpper = false;
            bool hasSpeciaChar = false;
            int kol = 0;

            foreach (char c in x)
            {
                kol++;
                if (char.IsDigit(c))
                {
                    hasDigit = true;
                }
                else if (char.IsLower(c))
                {
                    hasLower = true;
                }
                else if (char.IsUpper(c))
                {
                    hasUpper = true;
                }
                else if ((char.IsSymbol(c)) || (c == '!') || (c == '@') || (c == '$') || (c == '#'))
                {
                    hasSpeciaChar = true;
                }
            }
            if (hasDigit == false)
            {
                return 0;
            }
            else if (hasLower == false)
            {

                return 1;
            }
            else if (hasUpper == false)
            {

                return 2;
            }
            else if (hasSpeciaChar == false)
            {

                return 3;
            }
            else if ((kol < 5) || (kol > 20))
            {

                return 4;
            }
            else
            {
                return 5;
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Выберите картинку";
                op.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files(*.gif) | *.gif";
                if (op.ShowDialog() == true)
                {
                    FileInfo fileInfo = new FileInfo(op.FileName);
                    if (fileInfo.Length > (1024 * 1024 * 2))
                    {
                        throw new Exception("Размер файла должен быть меньше 2Мб");
                    }
                    UserImage.Source = new BitmapImage(new Uri(op.FileName));
                    photoName = op.SafeFileName;
                    filePath = op.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK);
                filePath = null;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            using (ActivityEntities activityEntities = new ActivityEntities())
            {
                List<Events> events = new List<Events>();
                events = activityEntities.Events.Where(x => x.DirectionId == ((Directions)DirectionComboBox.SelectedItem).Id).ToList();
                ActivityComboBox.ItemsSource = events;
            }
            ActivityComboBox.SelectedIndex = 0;
            ActivityComboBox.IsEnabled = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ActivityComboBox.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (FIO.Text != null && Email.Text != null && PhoneNumber.Text != null && UserPasswordBox.Password != null && DirectionComboBox.SelectedItem != null && GenderComboBox.SelectedItem != null && RoleComboBox.SelectedItem != null)
            {
                if (!IsPhoneNumberValid(PhoneNumber.Text))
                {
                    MessageBox.Show("Введите действительный номер телефона");
                }
                else if (UserPasswordBox.Password == RepeatPassword.Password)
                {
                    int result = ValidatePassword(UserPasswordBox.Password);
                    if (result != 5)
                    {
                        if (result == 0)
                        {
                            MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать цифры");
                        }
                        else if (result == 1)
                        {
                            MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать строчые буквы");
                        }
                        else if (result == 2)
                        {
                            MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать заглавные буквы");
                        }
                        else if (result == 3)
                        {
                            MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать специальные символы");
                        }
                        else if (result == 4)
                        {
                            MessageBox.Show("Пароль не соответствует требованиям, пароль должен содержать от 5 до 20 символов");
                        }
                    }
                    else
                    {
                        using (ActivityEntities activityEntities = new ActivityEntities())
                        {
                            newUser.GenderId = (GenderComboBox.SelectedItem as Genders).Id;
                            newUser.Password = UserPasswordBox.Password;
                            newUser.GenderId = ((Genders)GenderComboBox.SelectedItem).Id;
                            newUser.RoleId = ((Roles)RoleComboBox.SelectedItem).Id;
                            newUser.DirectionID = ((Directions)DirectionComboBox.SelectedItem).Id;

                            if (filePath != null)
                            {
                                string photo = ChangePhotoName();
                                string dest = Directory.GetCurrentDirectory() + @"\Pictures\" + photo;
                                File.Copy(filePath, dest);
                                newUser.Photo = photo;
                            }

                            activityEntities.Users.Attach(newUser);
                            activityEntities.Entry(newUser).State = System.Data.Entity.EntityState.Added;
                            activityEntities.SaveChanges();

                            Organaizer organaizer = new Organaizer();
                            organaizer.Show();
                            this.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Повторите пароль");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля");
            }
        }
        string ChangePhotoName()
        {
            string pathToDir = Directory.GetCurrentDirectory() + @"\Pictures\";
            string x = pathToDir + photoName;
            string newPhotoName = photoName;
            int i = 0;
            if (File.Exists(x))
            {
                while (File.Exists(x))
                {
                    i++;
                    x = pathToDir + newPhotoName + i;
                }
                newPhotoName = i.ToString() + newPhotoName;
            }
            return newPhotoName;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Organaizer organaizer = new Organaizer();
            organaizer.Show();
            this.Close();
        }
    }


}
