using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.ApiEntities.Teams;
using WpfApp2.Commands;
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

		private ObservableCollection<PageNo> allPages;

		public ObservableCollection<PageNo> AllPages
        {
			get { return allPages; }
			set { allPages = value; OnPropertyChanged(); }
		}

		private PageNo selectedPageNo;

		public PageNo SelectedPageNo
        {
			get { return selectedPageNo; }
			set { selectedPageNo = value; }
		}

		public RelayCommand SelectPageCommand { get; set; }

		List<Response> result = null;
		public async void LoadData()
		{
			var service = new NbaApiService();


			SelectPageCommand = new RelayCommand((obj) =>
			{
				var no=SelectedPageNo.No;
				AllTeams = new ObservableCollection<Response>(result.Skip((no - 1) * 10).Take(10));
			});

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

			if (File.Exists("teams.json"))
			{
				result = await JsonHelper<ApiEntities.Teams.Response>.DeserializeAsync("teams.json");
			}
			else
			{
				result = await service.GetTeamsAsync();
				await JsonHelper<Response>.SerializeAsync(result, "teams.json");
			}
			AllPages = new ObservableCollection<PageNo>();
			var pageSize = 10;
			var pageCount=decimal.Parse(result.Count().ToString())/pageSize;
			var count=Math.Ceiling(pageCount);
			for (int i = 0; i < count; i++)
			{
				AllPages.Add(new PageNo
				{
					No = i + 1
				});
			}

			AllTeams = new ObservableCollection<Response>(result.Skip(0).Take(pageSize));
		}


		public MainViewModel()
		{
			LoadData();
		}

	}
}
