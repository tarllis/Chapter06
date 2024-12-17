using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Packt.Shared;

public class Person
{
    #region Properties
    public string? Name { get; set; }
    public DateTimeOffset Born { get; set; }
    public List<Person> Children = new();
    #endregion
    #region Methods
    public void WriteToConsole()
    {
        WriteLine($"{Name} was born on a {Born:dddd}.");
    }
    public void WriteChildrenToConsole()
    {
        string term = Children.Count == 1 ? "child" : "children";
        WriteLine($"{Name} has {Children.Count} {term}.");
    }
    #endregion

    // Allow multiple spouses to stored for a person.
    public List<Person> Spouses = new();

    // A read-only property to show a person is married to anyone.
    public bool Married => Spouses.Count > 0;

    // Static method for marry two people.
    public static void Marry(Person p1, Person p2)
    {

        ArgumentNullException.ThrowIfNull(p1);
        ArgumentNullException.ThrowIfNull(p2);

        if (p1.Spouses.Contains(p2) || p2.Spouses.Contains(p1))
        {
            throw new ArgumentException(string.Format("{0} is already married to a {1}.", arg0: p1.Name, arg1: p2.Name));
        }

        p1.Spouses.Add(p2);
        p2.Spouses.Add(p1);
    }

    // Instance method to marry another person.
    public void Marry(Person partner)

    {
        Marry(this, partner); // "this" is the current person.
    }
    public void OutputSpouses()
    {
        if (Married)
        {
            string term = Spouses.Count == 1 ? "person" : "people";
            WriteLine($"{Name} is married to {Spouses.Count} {term}:");

            foreach (Person spouse in Spouses)
            {
                WriteLine($" {spouse.Name}");
            }
        }
        else
        {
            WriteLine($"{Name} is a singleton.");

        }
    }

        /// <summary>
        /// Static method to "multiply" aka procreate and have a child together.
        /// </summary>
        /// <param name="p1">Parent 1</param>
        /// <param name="p2">Parent 2</param>
        /// <returns>A Person object that is the child of Parent 1 and Parent 2.</ returns >
        /// <exception cref="ArgumentNullException">If p1 or p2 are null.</  exception >
        /// <exception cref="ArgumentException">If p1 and p2 are not married.</  exception >

        public static Person Procreate(Person p1, Person p2)
            {
            ArgumentNullException.ThrowIfNull(p1);
            ArgumentNullException.ThrowIfNull(p2);

            if (!p1.Spouses.Contains(p2) && !p2.Spouses.Contains(p1)) 
            {

                throw new ArgumentException(string.Format("{0} must be married to {1} to procreate with them.",

                arg0: p1.Name, arg1: p2.Name));
            }

            Person baby = new()
            {
                Name = $"baby of {p1.Name}  and {p2.Name}",
                Born = DateTimeOffset.Now
            };

            p1.Children.Add(baby);
            p2.Children.Add(baby);

            return baby;
        }

        // Instance method to "multiply".
        public Person ProcreateWith(Person partner)
                {
            return Procreate(this, partner);
        }
    #region Operators

    // Define the "+" operator to "marry".
    public static bool operator +(Person p1, Person p2)
    {
        Marry(p1, p2);

        // Configure they are both now married.
        return p1.Married && p2.Married;
    }
    #endregion

    // Define the "*" operator to multiply".
    public static Person operator *(Person p1, Person p2)
    {
        // Return a reference to the baby that results from multiplying.
        return Procreate(p1, p2);
        
    }
}
