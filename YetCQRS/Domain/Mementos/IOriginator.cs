namespace YetCQRS.Domain.Mementos
{
    public interface IOriginator
    {
        Memento GetMemento();
        void SetMemento(Memento memento);
    }
}
