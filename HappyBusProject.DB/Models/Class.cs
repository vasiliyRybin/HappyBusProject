namespace HappyBusProject.DB.Models
{
    public abstract class BaseEntity
    {
    }
    public interface IEntity
    {
        int Id { get; set; }
    }
}