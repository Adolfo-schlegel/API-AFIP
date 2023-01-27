namespace BuilderPatternExample
{
	internal class Program
	{
		static void Main(string[] args)
		{
			CarBuilder carBuilder = new CarBuilder();

			Car Toyota = carBuilder.Create().SetMake("Toyota").SetModel("Camry").SetYear(2020).SetColor("Blue").Build();
			Car Fiat = carBuilder.Create().SetMake("Fiat").SetNumberOfDoors(3).Build();

			Console.ReadKey();
		}
	}

	public interface Builder <T>
	{
		T Build ();
	}

	class Car
	{
		public string? Make { get; set; }
		public string? Model { get; set; }
		public int? Year { get; set; }
		public string? Color { get; set; }
		public int? NumberOfDoors { get; set; }
	}
	interface ICarBuilder : Builder<Car>
	{
		ICarBuilder Create();
		ICarBuilder SetMake(string make);
		ICarBuilder SetModel(string model);
		ICarBuilder SetYear(int year);
		ICarBuilder SetColor(string color);
		ICarBuilder SetNumberOfDoors(int numberOfDoors);
		Car? Build();
	}

	class CarBuilder : ICarBuilder
	{
		private Car? _car;

		public ICarBuilder Create()
		{
			_car = new Car();
			return this;
		}
		public ICarBuilder SetMake(string make)
		{
			_car.Make = make;
			return this;
		}

		public ICarBuilder SetModel(string model)
		{
			_car.Model = model;
			return this;
		}

		public ICarBuilder SetYear(int year)
		{
			_car.Year = year;
			return this;
		}

		public ICarBuilder SetColor(string color)
		{
			_car.Color = color;
			return this;
		}

		public ICarBuilder SetNumberOfDoors(int numberOfDoors)
		{
			_car.NumberOfDoors = numberOfDoors;
			return this;
		}

		public Car Build()
		{
			return _car;
		}
	}



}