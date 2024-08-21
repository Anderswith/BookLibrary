namespace BookLibrary.BE;

public class Book
{
    public string title { get; set; }
    public string author { get; set; }
    public int year { get; set; }
    public double ISBN { get; set; }
    public bool isLoaned { get; set; }

}