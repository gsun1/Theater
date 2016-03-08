

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Agent {
	public class agent {
		bool traded;
		public bool Traded{
			get { return traded; }
			set { traded = value; }
		}
		int identity; // 0 indexed number to represent identity of different agents
		public int Identity {
			get { return identity; }
		}
		int position; // 0 indexed number to represent position of current agent
		public int Position {
			get { return position; }
			set { position = value; }
		}
		double [] utility; // 0 indexed array that maps identity to utility
		public void printUtility() {
			for(int i = 0; i < utility.Length; ++i) {
					Console.Write("{0} ",utility[i]);
				}
			Console.WriteLine();
		}
		public agent(int identity, double[] utility) {
			traded = false;
			this.identity = identity;
			this.position = identity;
			this.utility = utility;
		}
		public double getUtility(int pos, agent[] seating) {
			double u1;
			double u2;
			try {
				u1 = utility[seating[pos-1].Identity];
			}
			catch {
				u1 = 0;
			}
			try {
				u2 = utility[seating[pos+1].Identity];
			}
			catch {
				u2 = 0;
			}
			return u1 + u2;
		}
	}
}