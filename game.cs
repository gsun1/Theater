

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Agent;

namespace Game {
	public class game {
		agent[] seating;
		agent[] store;
		public game(agent[] init){
			seating = init;
			store = init;
		}
		public void printSeating(){
			foreach (agent a in seating){
				Console.WriteLine("Agent: {0}, Utility: {1}",a.Identity, a.getUtility(a.Position,seating));
			}
		}
		public void printPosition(){
			foreach (agent a in seating){
				Console.WriteLine(a.Position);
			}
		}
		public bool trade(int i, int j) {
			agent ai = seating[i];
			agent aj = seating[j];
			int posi = ai.Position;
			int posj = aj.Position;
			if(ai.getUtility(posi, store) > ai.getUtility(posj, store) 
			|| aj.getUtility(posj, store) > aj.getUtility(posi, store)) {
				return false;
			}
			else if(ai.getUtility(posi, store) == ai.getUtility(posj, store) 
			&& aj.getUtility(posj, store) == aj.getUtility(posi, store)) {
				return false;
			}
			else {
				aj.Position = posi;
				ai.Position = posj;
				ai.Traded = true;
				aj.Traded = true;
				store[i] = aj;
				store[j] = ai;
				return true;
			}
		}
		public void reset() {
			int size = seating.Length;
			agent[] temp = new agent[size];
			for(int i = 0; i < size; ++i) {
				for(int j = 0; j < size; ++j) {
					if(seating[j].Identity == i) {
						temp[i] = seating[j];
						temp[i].Position = i;
						break;
					}
				}
			}
			seating = temp;
		}
		public void update() {
			seating = store;
			foreach(agent a in seating){
				a.Traded = false;
			}
		}
		public int round() {
			int result = 0;
			int size = seating.Length;
			for (int i = 0; i < size; ++i) {
				if(!seating[i].Traded) {
					for(int j = i+1; j < size; ++j) {
						if(trade(i,j)) {
							result++;
							break;
						}
					}
				}
			}
			update();
			return result;
		}
		public int equilibrate() {
			printSeating();
			Console.WriteLine();
			bool check = false;
			int round_counter = 0;
			int store = 0;
			int move_counter = 0;
			while(!check && round_counter < 1000) {
				store = round();
				if(store != 0) {
					move_counter += store;
					++round_counter;
					printSeating();
					Console.WriteLine();
				}
				else {
					check = true;
				}
			}
			if(check) {
				Console.WriteLine("Equilibrium reached in {0} round(s).",round_counter);
			}
			else {
				Console.WriteLine("Equilibrium not reached in {0} round(s).",round_counter);
			}
			Console.WriteLine("{0} moves made",move_counter);
			Console.WriteLine("Aggregate utility: {0}",aggregateUtility(seating));
			return move_counter;
		}
		public double aggregateUtility(agent[] seating) {
			double sum = 0;
			for(int i = 0; i < seating.Length; ++i) {
				sum += seating[i].getUtility(i,seating);
			}
			return sum;
		}
		public bool socialRound() {
			store = seating;
			bool result = false;
			int storei = 0;
			int storej = 0;
			agent ai;
			agent aj;
			double best = aggregateUtility(seating);
			int size = seating.Length;
			for(int i = 0; i < size; ++i) {
				for(int j = i+1; j < size; ++j) {
					agent tempi = seating[i];
					agent tempj = seating[j];
					store[i] = tempj;
					store[j] = tempi;
					store[i].Position = j;
					store[j].Position = i;
					if(aggregateUtility(store) > best) {
						storei = i;
						storej = j;
						best = aggregateUtility(store);
						result = true;
					}
					seating[i] = tempi;
					seating[j] = tempj;
					seating[i].Position = i;
					seating[j].Position = j;
				}
			}
			ai = seating[storei];
			aj = seating[storej];
			ai.Position = storej;
			aj.Position = storei;
			seating[storei] = aj;
			seating[storej] = ai;
			//printSeating();
			//Console.WriteLine();
			return result;
		}
		public void socialPlanner() {
			int moves = equilibrate();
			reset();
			Console.WriteLine();
			int move_counter = 0;
			printSeating();
			Console.WriteLine();
			while(move_counter++ < moves && socialRound()){
				printSeating();
				Console.WriteLine();
			}
			if(!socialRound()) {
				Console.WriteLine("Social Planner Problem solved in {0} round(s).\nAggregate utility: {1}.", move_counter-1, aggregateUtility(seating));
			}
			else {
				Console.WriteLine("Social Planner Problem not solved after {0} round(s).\nAggregate utility: {1}.", move_counter-1, aggregateUtility(seating));
			}
		}
	}
}