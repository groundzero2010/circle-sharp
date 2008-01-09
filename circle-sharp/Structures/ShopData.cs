using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CircleSharp.Structures
{
	class ShopData
	{
		public int Number;
		public List<int> Producing = new List<int>();
		public float ProfitBuy;
		public float ProfitSell;
		public List<ShopBuyData> Type = new List<ShopBuyData>();
		public string NoSuchItem1;
		public string NoSuchItem2;
		public string MissingCash1;
		public string MissingCash2;
		public string DoNotBuy;
		public string MessageBuy;
		public string MessageSell;
		public int Temper;
		public long Bitvector;
		public int Keeper;
		public int WithWho;
		public List<int> InRoom = new List<int> ();
		public int Open1;
		public int Open2;
		public int Close1;
		public int Close2;
		public int BankAccount;
		public int LastSort;
	}
}
