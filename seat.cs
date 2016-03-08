using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Agent;
using Game;
using Parser;

namespace Seat {
	public class seat {
		static void Main(string[] args) {
			parser p = new parser();
			game g = p.parse(args[0]);
			g.socialPlanner();
		}
	}
}