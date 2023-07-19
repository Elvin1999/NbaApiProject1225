using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.ApiEntities.Teams;
using WpfApp2.Helpers;
using WpfApp2.Models;
using WpfApp2.Services;

namespace WpfApp2.ViewModels
{
    public class MainViewModel:BaseViewModel
    {
		private ObservableCollection<Response> allTeams;

		public ObservableCollection<Response> AllTeams
        {
			get { return allTeams; }
			set { allTeams = value; OnPropertyChanged(); }
		}

		List<Response> result = null;
		public async void LoadData()
		{
			var service = new NbaApiService();

			//if (File.Exists("players.json"))
			//{
			//	var result = JsonHelper<Player>.Deserialize("players.json");
			//	var data = 0;
			//}
			//else
			//{
			//	var result = await service.GetPlayersByTeamIdAsync(1);
			//	JsonHelper<Player>.Serialize(result, "players.json");

			//}

			if (File.Exists("teams2.json"))
			{
				result = JsonHelper<ApiEntities.Teams.Response>.Deserialize("teams.json");
			}
			else
			{
				result = await service.GetTeamsAsync();
				JsonHelper<Response>.Serialize(result, "teams.json");
			}
			AllTeams = new ObservableCollection<Response>(result);
		}


		public MainViewModel()
		{
			LoadData();
		}

	}
}
