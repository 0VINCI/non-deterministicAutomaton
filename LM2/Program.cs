using System;
using System.Collections.Generic;
using System.IO;

namespace LM2
{
    // Klasa reprezentująca automat niedeterministyczny
    public class NFA
    {
        // Prywatne pola klasy przechowujące stan początkowy, stany końcowe i funkcje przejścia
        private readonly int _startState;
        private readonly HashSet<int> _finalStates;
        private readonly Dictionary<(int, char), List<int>> _transitions;
        
        // Konstruktor inicjalizujący NFA
        public NFA(Dictionary<(int, char), List<int>> transitions, int startState, HashSet<int> finalStates)
        {
            _transitions = transitions;
            _startState = startState;
            _finalStates = finalStates;
        }
        // Metoda analizująca przekazane słowo i śledząca przejścia między stanami
        public void AnalyzeWord(string word)
        {
            List<int> currentStates = new List<int> { _startState };
            Dictionary<int, List<int>> paths = new Dictionary<int, List<int>>();
            paths[_startState] = new List<int> { _startState };
            Console.WriteLine($"Analiza wczytanego słowa: {word}");

            // Iteracja przez każdy symbol w słowie
            foreach (char symbol in word)
            {
                Dictionary<int, List<int>> newPaths = new Dictionary<int, List<int>>();
                List<int> nextStates = new List<int>();

                foreach (var state in currentStates)
                {
                    if (_transitions.TryGetValue((state, symbol), out var possibleNextStates))
                    {
                        // Przetwarzanie możliwych przejść dla aktualnych stanów
                        foreach (var nextState in possibleNextStates)
                        {
                            if (!nextStates.Contains(nextState))
                            {
                                nextStates.Add(nextState);
                                if (!paths.ContainsKey(state))
                                {
                                    paths[state] = new List<int>();
                                }

                                var newPath = new List<int>(paths[state]) { nextState };
                                newPaths[nextState] = newPath;
                            }
                        }
                    }
                }

                currentStates = nextStates;
                paths = newPaths;
                // Wyświetlanie aktualnych stanów
                Console.WriteLine($"Symbol: {symbol}, Aktualne stany to: {string.Join(", ", currentStates)}");
            }
            
            // Sprawdzanie, czy jakaś ścieżka prowadzi do stanu akceptującego
            bool isAccepted = paths.Any(p => _finalStates.Contains(p.Key));
            Console.WriteLine(isAccepted ? "Słowo zaakceptowane" : "Słowo odrzucone");

            // Wypisanie ścieżek prowadzących do stanu akceptującego
            if (isAccepted)
            {
                foreach (var path in paths)
                {
                    if (_finalStates.Contains(path.Key))
                    {
                        Console.WriteLine($"Ścieżka: {string.Join(" -> ", path.Value)}");
                    }
                }
            }
        }
    }

    // Główna klasa programu
    public class Program
    {
        static void Main()
        {
            // Definicja funkcji przejścia dla NFA
            var transitions = new Dictionary<(int, char), List<int>>()
            {
                {(0, '0'), new List<int> {0, 1}},
                {(0, '1'), new List<int> {0, 3}},
                {(0, '2'), new List<int> {0, 5}},
                {(0, '3'), new List<int> {0, 7}},
                {(1, '0'), new List<int> {2}},
                {(2, '0'), new List<int> {9}},
                {(3, '1'), new List<int> {4}},
                {(4, '1'), new List<int> {9}},
                {(5, '2'), new List<int> {6}},
                {(6, '2'), new List<int> {9}},
                {(7, '3'), new List<int> {8}},
                {(8, '3'), new List<int> {9}},
                {(9, '0'), new List<int> {9}},
                {(9, '1'), new List<int> {9}},
                {(9, '2'), new List<int> {9}},
                {(9, '3'), new List<int> {9}},
            };
            
            // Definicja stanów końcowych automatu
            var finalStates = new HashSet<int>()
            {
                9
            };

            // Definicja stanu początkowego
            int startState = 0;

            // Tworzenie instancji NFA
            NFA nfa = new NFA(transitions, startState, finalStates);

            // Wczytywanie słów z pliku i ich analiza
            string filePath = "/Users/pawelw/Desktop/II st/Lingwistyka matematyczna/Lab/2/przyklad.txt";
            string[] words = File.ReadAllText(filePath).Split('#');

            // Iteracja przez wszystkie słowa wczytane z pliku
            foreach (var word in words)
            {
                // Analiza każdego słowa
                nfa.AnalyzeWord(word);
            }
        }
    }
}
