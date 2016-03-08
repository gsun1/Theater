
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Game;
using Agent;

namespace Parser {
	public class parser {
		public double[] stringToDoubleArray(string s) {
			List<double> dbll = new List<double>();
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < s.Length; i++) {
				if(s[i] != ' ') {
					sb.Append(s[i]);
				}
				else {
					dbll.Add(Convert.ToDouble(sb.ToString()));
					sb = new StringBuilder();
				}
			}
			dbll.Add(Convert.ToDouble(sb.ToString()));
			return dbll.ToArray();
		}
		public game parse(string s) {
			string[] lines = File.ReadLines(s).ToArray();
			int num_players = Convert.ToInt32(lines[0]);
			agent[] seating = new agent[num_players];
			for(int i = 0; i < num_players; ++i) {
				double[] utilities = stringToDoubleArray(lines[i+1]);
				seating[i] = new agent(i,utilities);
			}
			game result = new game(seating);
			return result; 
		}
	}
}