namespace HappyBusProject.Interfaces
{
    interface IPerson<T>
    {
        T GetAll();
        T GetByName(string name);
    }
}
