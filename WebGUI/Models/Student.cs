namespace WebGUI.Models
{
    public class Student
    {
        public Student()
        {
        }

        public Student(int id, string firstName, string lastName, int pin, int acctNum, int balance, string university, string avatar)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Pin = pin;
            AcctNum = acctNum;
            Balance = balance;
            University = university;
            Avatar = avatar;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> Balance { get; set; }
        public Nullable<int> AcctNum { get; set; }
        public Nullable<int> Pin { get; set; }
        public string Avatar { get; set; }
        public string University { get; set; }
    }
}
