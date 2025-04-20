
public class IDEA_Categories {
    public IDEA_Category[] categories { get; set; }
}

public class IDEA_Category {
    public int id { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
    public int sortOrder { get; set; }
    public int status { get; set; }
    public float percent { get; set; }
    public string imageFile { get; set; }
    public string imageUrl { get; set; }
    public int hasChildren { get; set; }
    public Parent parent { get; set; }
    public int isSearchable { get; set; }
    public DateTime createdAt { get; set; }
}

public class Parent {
    public int id { get; set; }
    public string name { get; set; }
    public int status { get; set; }
}
