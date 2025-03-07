using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_6
{
    public class Purple_3
    {
        public struct Participant
        {
            private string _name;
            private string _surname;
            private double[] _marks;
            private int[] _places;

            private int _curMark;

            public string Name => _name;
            public string Surname => _surname;

            public double[] Marks => (double[])_marks?.Clone();
            public int[] Places => (int[])_places?.Clone();

            public int Score => _places == null ? 0 : _places.Sum();

            public Participant(string name, string surname)
            {
                _name = name;
                _surname = surname;
                _marks = new double[7];
                _places = new int[7];
                _curMark = 0;
            }

            public void Evaluate(double result)
            {
                if (_curMark >= 7 || _marks == null) return;

                _marks[_curMark++] = result;
            }

            public static void SetPlaces(Participant[] participants)
            {
                if (participants == null || participants.Length == 0) return;

                var sortedParticipants = participants.Where(x => x._marks != null && x._places != null).ToArray();
                int n = sortedParticipants.Length;
                for (int judge = 0; judge < 7; judge++)
                {
                    sortedParticipants = sortedParticipants.OrderByDescending(x => x._marks[judge]).ToArray();

                    for (int i = 0; i < n; i++)
                    {
                        sortedParticipants[i]._places[judge] = i + 1;
                    }
                }

                sortedParticipants = sortedParticipants
                    .Concat(participants.Where(x => x._marks == null || x._places == null)).ToArray();
                Array.Copy(sortedParticipants, participants, sortedParticipants.Length);
            }

            public static void Sort(Participant[] array)
            {
                if (array == null) return;

                var sortedArray = array
                    .Where(x => x._marks != null && x._places != null)
                    .OrderBy(x => x.Score)
                    .ThenBy(x => x._places.Min())
                    .ThenByDescending(x => x._marks.Sum())
                    .Concat(array.Where(x => x._marks == null || x._places == null))
                    .ToArray();
                Array.Copy(sortedArray, array, sortedArray.Length);
            }

            public void Print()
            {
                Console.WriteLine($"Имя: {_name ?? "не задано"}");
                Console.WriteLine($"Фамилия: {_surname ?? "не задано"}");

                Console.Write("Оценки:\t");
                if (_marks == null) Console.WriteLine("не заданы");
                else
                {
                    foreach (var mark in _marks)
                    {
                        Console.Write($"{Math.Round(mark, 2)}\t");
                    }

                    Console.WriteLine();
                }

                Console.Write("Места:\t");
                if (_places == null) Console.WriteLine("не заданы");
                else
                {
                    foreach (var place in _places)
                    {
                        Console.Write($"{place}\t");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine($"Лучшее место: {_places.Min()}");
                Console.WriteLine($"Сумма оценок: {Math.Round(_marks.Sum(), 2)}");
                Console.WriteLine($"Результат: {Score}");
                Console.WriteLine();
            }
        }

        public abstract class Skating
        {
            protected Participant[] participants;
            protected double[] moods;

            private int _curParticipant;

            public Participant[] Participants => (Participant[])participants?.Clone();
            public double[] Moods => (double[])moods?.Clone();

            public Skating(double[] moods)
            {
                participants = new Participant[0];
                
                _curParticipant = 0;

                if (moods == null) return;
                this.moods = moods;
                ModificateMood();
            }

            protected abstract void ModificateMood();

            public void Evaluate(double[] marks)
            {
                if (marks == null || moods == null || participants == null || _curParticipant == participants.Length) return;

                for (int i = 0; i < marks.Length; i++)
                {
                    participants[_curParticipant].Evaluate(marks[i] * moods[i]);
                }

                _curParticipant++;
            }

            public void Add(Participant participant)
            {
                if (participants == null) return;
                
                Array.Resize(ref participants, participants.Length + 1);
                participants[participants.Length - 1] = participant;
            }

            public void Add(Participant[] participants)
            {
                if (this.participants == null || participants == null) return;

                this.participants = this.participants.Concat(participants).ToArray();
            }
        }

        public class FigureSkating : Skating
        {
            public FigureSkating(double[] moods) : base(moods)
            {
            }

            protected override void ModificateMood()
            {
                if (moods == null) return;
                
                for (int i = 0; i < moods.Length; i++)
                {
                    moods[i] += (double)i / 10;
                }
            }
        }
        public class IceSkating : Skating
        {
            public IceSkating(double[] moods) : base(moods)
            {
            }

            protected override void ModificateMood()
            {
                if (moods == null) return;
                
                for (int i = 0; i < moods.Length; i++)
                {
                    moods[i] += moods[i] * i / 100;
                }
            }
        }
    }
}