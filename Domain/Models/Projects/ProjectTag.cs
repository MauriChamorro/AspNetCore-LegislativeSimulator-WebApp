namespace WebAppMVC.Domain.Models.Projects;

public class ProjectTag
{
    private ProjectTag(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    
    public static ProjectTag Education = new ProjectTag(1,  "Educación");
    public static ProjectTag Emvironment = new ProjectTag(2,  "Medio Ambiente");
    public static ProjectTag PublicBuildings = new ProjectTag(3,  "Obras Públicas");
}