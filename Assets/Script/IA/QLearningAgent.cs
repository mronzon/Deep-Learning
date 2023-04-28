using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = System.Random;

namespace Script.IA
{

    public class QLearningAgent
    {
        private int numActions;
        private Dictionary<Tuple<State, Action>, double> Q;
        private double alpha;
        private double gamma;
        private Random random;

        public QLearningAgent(int numActions, double alpha, double gamma, int seed)
        {
            this.numActions = numActions;
            this.Q = new Dictionary<Tuple<State, Action>, double>();
            this.alpha = alpha;
            this.gamma = gamma;
            this.random = new Random(seed);
        }

        public Action ChooseAction(State state)
        {
            if (random.NextDouble() < 0.1) // Epsilon-greedy exploration
            {
                return Action.RandomAction(numActions:numActions, randomSeed:random);
            }
            
            Dictionary<Action, double> values = GetActionValues(state);
            Action action;
            
            if (values.Count == 0)
            {
                return Action.RandomAction(numActions:numActions, randomSeed:random);
            }
            
            double maxValue = -1;
            List<Action> maxIndices = new List<Action>();
            foreach(KeyValuePair<Action, double> elt in values)
            {
                if (elt.Value > maxValue)
                {
                    maxIndices.Clear();
                    maxValue = elt.Value;
                    maxIndices.Add(elt.Key);
                }

                if (elt.Value >= maxValue)
                {
                    maxIndices.Add(elt.Key);
                }
            }
            action = maxIndices[random.Next(maxIndices.Count)];
            
            return action;
        }

        public void UpdateQ(State state, Action action, double reward, State nextState)
        {
            Dictionary<Action, double> nextValues = GetActionValues(nextState);
            double nextMaxValue = 0;
            foreach (KeyValuePair<Action, double> elt in nextValues)
            {
                if (elt.Value > nextMaxValue)
                {
                    nextMaxValue = elt.Value;
                }
            }

            Tuple<State, Action> old = new Tuple<State, Action>(state, action);
            double oldValue = Q.ContainsKey(old) ? Q[old] : 0;
            Q[old] = oldValue + alpha * (reward + gamma * nextMaxValue - oldValue);
        }

        private Dictionary<Action, double> GetActionValues(State state)
        {
            Dictionary<Action, double> values = new Dictionary<Action, double>();
            for (float speed = -1; speed <= 1; speed += 0.1f)
            {
                for (float turningDegree = -1; turningDegree <= 1; turningDegree += 0.1f)
                {
                    Action action = new Action(speed, turningDegree);
                    Tuple<State, Action> key = new Tuple<State, Action>(state, action);
                    if (Q.ContainsKey(key))
                    {
                        values[action] = Q[key];
                    }
                }
            }
            return values;
        }

        public void SaveModel(String path)
        {
            StreamWriter writer = new StreamWriter(path);
            foreach (var elt in Q)
            {
                Tuple<State, Action> value = elt.Key;
                for (int i = 0; i < value.Item1.SensorValues.Length; i++)
                {
                    writer.Write(value.Item1.SensorValues[i] + "|");
                }

                writer.Write(value.Item1.DistanceToParkingSlot + "|");
                writer.Write(value.Item2.Speed + "|");
                writer.Write(value.Item2.TurningDegree + "\n");
            }
            writer.Close();
            Debug.Log("Saved !");
        }
    }
}