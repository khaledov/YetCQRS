namespace YetCQRS.Domain.Mementos
{
    public interface IOriginator<T> where T:Memento
    {
        T GetMemento();
        void SetMemento(T memento);
    }
}
