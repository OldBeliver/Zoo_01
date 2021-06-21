using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo_01
{
    class Program
    {
        static void Main(string[] args)
        {
            Builder create = new Builder();
            List<Animal> animals = new List<Animal>();

            AnimalPark zoo = new AnimalPark();
            zoo.Work();
        }
    }

    class AnimalPark
    {
        private List<Enclosure> _enclosures;
        private Builder _builder;

        public string Name { get; private set; }

        public AnimalPark()
        {
            Name = TakeNameZoo();
            _enclosures = new List<Enclosure>();
            _builder = new Builder();
            int enclosureNumber = 10;

            CreateEnclosure(enclosureNumber);
        }

        public void Work()
        {
            bool isOpen = true;

            Console.WriteLine($"Добро пожаловать");
            Console.WriteLine($"в {Name}\n");

            while (isOpen)
            {
                Console.WriteLine($"Перед Вами план-схема зоопарка\n");
                ShowAllEnclosures();
                
                Console.Write($"\nВведите номер вольера или exit для выхода: ");
                string userInput = Console.ReadLine();
                
                if (userInput == "exit")
                {
                    isOpen = false;
                }
                else if (GetNumber(userInput, out int number))
                {
                    if(number >= 1 && number <= _enclosures.Count)
                    {
                        Console.WriteLine($"\nобитатель вольера: {_enclosures[number-1].Name}");
                        Console.WriteLine($"Всего {_enclosures[number - 1].CountAnimals()} особей");

                        _enclosures[number - 1].CountGender(out int maleCount, out int femaleCount);
                        Console.WriteLine($"из них мальчиков {maleCount} и девочек {femaleCount}");

                        Console.WriteLine($"в период брачных игр или борьбы за территорию издают характерный звук: {_enclosures[number-1].Cry()}");
                    }
                    else
                    {
                        Console.WriteLine($"вольер с таким номером еще не открыли");
                    }
                }
                else
                {
                    Console.WriteLine($"ошибка ввода");
                }

                Console.WriteLine($"любую для продолжения ...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private string TakeNameZoo()
        {
            return "плавучий зоопарк редких животных\nпассажирского лайнера «Доктор наук профессор Шварценгольд»";
        }

        private void CreateEnclosure(int quanity)
        {
            for (int i = 0; i < quanity; i++)
            {
                _enclosures.Add(_builder.MakeEnclosure());
            }
        }

        private void ShowAllEnclosures()
        {
            for (int i = 0; i < _enclosures.Count; i++)
            {
                Console.WriteLine($"Вольер № {i + 1:d2} {_enclosures[i].Name}");
                //_enclosures[i].ShowAnimals();
            }
        }

        private bool GetNumber(string userInput, out int number)
        {
            return int.TryParse(userInput, out number);
        }
    }

    class Enclosure
    {
        private static Random _rand = new Random();

        private List<Animal> _animals;
        private Builder _builder;

        public string Name { get; private set; }

        public Enclosure()
        {
            _animals = new List<Animal>();
            _builder = new Builder();

            int maxAnimalTypes = 6;
            int animalIndex = GetRandomIndex(maxAnimalTypes);


            int maxAnimalsInEnclosure = 6;
            int number = GetRandomIndex(maxAnimalsInEnclosure);
            for (int i = 1; i < number + 1; i++)
            {
                AddAnimal(animalIndex);
            }

            Name = GetEnclosureName();
        }

        public void ShowAnimals()
        {
            for (int i = 0; i < _animals.Count; i++)
            {
                Console.Write($"{i + 1:d2} - ");
                _animals[i].ShowInfo();
            }
        }

        public void CountGender(out int male, out int female)
        {
            int nubmerMale = 0;
            int numberFemale = 0;

            for (int i = 0; i < _animals.Count; i++)
            {
                if (_animals[i].TakeGender() == Gender.male)
                {
                    nubmerMale++;
                }
                else if(_animals[i].TakeGender() == Gender.female)
                {
                    numberFemale++;
                }
            }

            male = nubmerMale;
            female = numberFemale;
        }

        public int CountAnimals()
        {
            return _animals.Count;
        }

        public string Cry()
        {
            return _animals[0].TakeCry();
        }

        private void AddAnimal(int index)
        {
            switch (index)
            {
                case 1:
                    _animals.Add(_builder.August());
                    break;
                case 2:
                    _animals.Add(_builder.Bear());
                    break;
                case 3:
                    _animals.Add(_builder.Butterfly());
                    break;
                case 4:
                    _animals.Add(_builder.Cow());
                    break;
                case 5:
                    _animals.Add(_builder.Rabbit());
                    break;
                case 6:
                    _animals.Add(_builder.Worm());
                    break;
                default:
                    _animals.Add(_builder.Bear("топтыгин"));
                    break;
            }
        }

        private int GetRandomIndex(int count)
        {
            return _rand.Next(1, count + 1);
        }

        private string GetEnclosureName()
        {
            return _animals[0].TakeShortInfo();
        }
    }

    class Animal
    {
        private string _name;
        private string _type;
        private int _age;
        private Gender _gender;

        private string _cry;

        public Animal(string name, string type, Gender gender, int age, string cry)
        {
            _name = name;
            _type = type;
            _gender = gender;
            _age = age;
            _cry = cry;
        }

        public string TakeShortInfo()
        {
            return _type + " " + _name;
        }

        public void ShowInfo()
        {
            string gender = ConvertGenderToString(_gender);

            Console.WriteLine($"{_type} {_name}, {gender}, возраст - {_age} полных лет, издает звуки: {_cry}");
        }

        public Gender TakeGender()
        {
            return _gender;
        }

        public string TakeCry()
        {
            return _cry;
        }

        private string ConvertGenderToString(Gender _gender)
        {
            string gender;

            if (_gender == Gender.male)
            {
                gender = "мальчик";
            }
            else
            {
                gender = "девочка";
            }

            return gender;
        }
    }

    class Builder
    {
        private static Random _rand = new Random();

        private string _type;
        private string _name;
        private Gender _gender;
        private int _age;
        private string _cry;

        public Animal Bear()
        {
            _name = "летун";
            _type = "медведь";
            _gender = GetRandomGender();
            _age = GetRandomAge(20);
            _cry = "лечу-у-у-у";

            return new Animal(_name, _type, _gender, _age, _cry);
        }

        public Animal Bear(string name)
        {
            _name = name;
            _type = "медведь";
            _gender = GetRandomGender();
            _age = GetRandomAge(15);
            _cry = "гр-р-р";

            return new Animal(_name, _type, _gender, _age, _cry);
        }

        public Animal Cow()
        {
            _name = "корова";
            _type = "скунсовидная";
            _gender = GetRandomGender();
            _age = GetRandomAge(10);
            _cry = "фу-у-у-у";

            return new Animal(_name, _type, _gender, _age, _cry);
        }

        public Animal Worm()
        {
            _name = "выползень";
            _type = "подкустовый";
            _gender = Gender.male;
            _age = 10;
            _cry = "тихое сопение";

            return new Animal(_name, _type, _gender, _age, _cry);
        }

        public Animal Rabbit()
        {
            _name = "кролик-зануда";
            _type = "североамериканский";
            _gender = GetRandomGender();
            _age = GetRandomAge(5);
            _cry = "шо, опять?";

            return new Animal(_name, _type, _gender, _age, _cry);
        }

        public Animal August()
        {
            _name = "перпончатокрылый";
            _type = "серпень";
            _gender = GetRandomGender();
            _age = GetRandomAge(7);
            _cry = "кря-кря";

            return new Animal(_name, _type, _gender, _age, _cry);
        }

        public Animal Butterfly()
        {
            _name = "африканская бабочка";
            _type = "одноразовая";
            _gender = GetRandomGender();
            _age = 0;
            _cry = "бяк-бяк-бяк-бяк";

            return new Animal(_name, _type, _gender, _age, _cry);
        }

        public Enclosure MakeEnclosure()
        {
            return new Enclosure();
        }

        public AnimalPark MakeAnimalPark()
        {
            return new AnimalPark();
        }

        private Gender GetRandomGender()
        {
            Gender gender = Gender.male;

            if (_rand.Next(0, 2) == 1)
            {
                gender = Gender.female;
            }

            return gender;
        }

        private int GetRandomAge(int maxAge)
        {
            return _rand.Next(maxAge + 1);
        }
    }

    enum Gender
    {
        male,
        female
    }
}
