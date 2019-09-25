namespace YetCQRS.Domain
{
    public interface IOriginator
    {
        Memento GetMemento();
        void SetMemento(Memento memento);
    }
}
