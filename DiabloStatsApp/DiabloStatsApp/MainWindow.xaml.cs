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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RestSharp;

namespace DiabloStatsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const string APIKey = "s6kpudv6t2vx6m8h8mbb547kn8eqn8hn";
        private RestClient client = new RestClient("https://eu.api.battle.net/");

        public MainWindow()
        {
            InitializeComponent();
            initListViews();
        }

        public void initListViews()
        {
            var gridViewProfile = new GridView();
            this.lv_Profile.View = gridViewProfile;
            gridViewProfile.Columns.Add(new GridViewColumn { Header = "Id", DisplayMemberBinding = new Binding("id") });
            gridViewProfile.Columns.Add(new GridViewColumn { Header = "Name", DisplayMemberBinding = new Binding("name") });
            gridViewProfile.Columns.Add(new GridViewColumn { Header = "Class", DisplayMemberBinding = new Binding("@class") });

            var gridViewHero = new GridView();
            this.lv_Hero.View = gridViewHero;
            gridViewHero.Columns.Add(new GridViewColumn { Header = "Name", DisplayMemberBinding = new Binding("name") });
            gridViewHero.Columns.Add(new GridViewColumn { Header = "ParagonLvl", DisplayMemberBinding = new Binding("paragonLevel") });
            gridViewHero.Columns.Add(new GridViewColumn { Header = "Life", DisplayMemberBinding = new Binding("stats.life") });
            gridViewHero.Columns.Add(new GridViewColumn { Header = "Damage", DisplayMemberBinding = new Binding("stats.damage") });
            gridViewHero.Columns.Add(new GridViewColumn { Header = "Toughness", DisplayMemberBinding = new Binding("stats.toughness") });
        }

        public List<Hero> GetCareerProfile(string battletag)
        {
            string bt = battletag.Replace("#", "-");
            string reqStr = String.Format("d3/profile/{0}/?locale=en_GB&apikey={1}", bt, APIKey);
            RestRequest request = new RestRequest(reqStr);

            IRestResponse<Profile> response = client.Execute<Profile>(request);


            return response.Data.heroes;
        }

        public Hero GetHeroInformation(string battletag, int heroid)
        {
            string bt = battletag.Replace("#", "-");

            string reqStr = String.Format("d3/profile/{0}/hero/{1}?locale=en_GB&apikey={2}", bt, heroid, APIKey);
            RestRequest request = new RestRequest(reqStr);

            IRestResponse<Hero> response = client.Execute<Hero>(request);
            Hero hero = new Hero {
                id = response.Data.id,
                name = response.Data.name,
                @class = response.Data.@class,
                paragonLevel = response.Data.paragonLevel,
                stats = new Stats3 {
                    life = response.Data.stats.life,
                    damage = response.Data.stats.damage,
                    toughness = response.Data.stats.toughness
                }
            };

            return hero;
        }

        private void btn_Profile_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in GetCareerProfile(tb_BattleTag.Text))
            {
                this.lv_Profile.Items.Add(new Hero { id = item.id, name = item.name, @class = item.@class });
            }
        }

        private void btn_Hero_Click(object sender, RoutedEventArgs e)
        {
            int heroid = Int32.Parse(tb_HeroId.Text);
            Hero hero = GetHeroInformation(tb_BattleTagHero.Text, heroid);

            this.lv_Hero.Items.Add(new Hero
            {
                id = hero.id,
                name = hero.name,
                @class = hero.@class,
                paragonLevel = hero.paragonLevel,
                stats = new Stats3
                {
                    life = hero.stats.life,
                    damage = hero.stats.damage,
                    toughness = hero.stats.toughness
                }
            });
        }
    }
}
