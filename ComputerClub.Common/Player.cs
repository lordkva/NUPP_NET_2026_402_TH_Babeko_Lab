namespace ComputerClub.Common;
public class Player : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Nickname { get; set; }
    public Player(string name, int age, string nickname)
    {
        Id = Guid.NewGuid();
        Name = name;
        Age = age;
        Nickname = nickname;
    }
    public void ShowInfo()
    {
        Console.WriteLine(
            $"Гравець: {Name}, нік: {Nickname}, вік: {Age}");
    }
}