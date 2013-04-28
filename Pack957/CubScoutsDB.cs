using System;
using SQLite;
using System.IO;

namespace Pack957
{
	public class CubScoutsDB
	{
		public CubScoutsDB ()
		{
		}

		public bool TableExists (String tableName)
		{
			string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string dbName = "CubScouts.db";
			string dbPath = Path.Combine ( personalFolder, dbName);
			
			if (File.Exists(dbPath)) {
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public class CubScout
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[MaxLength(8)]
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Nickname { get; set; }
		public string ScoutType { get; set; }
		public string MomsName { get; set; }
		public string DadsName { get; set; }
		public string EmailAddress { get; set; }
		public string HomePhone { get; set; }
		public string CellPhone { get; set; }
	}
}

