public interface ICommand
{
    void Execute();
    void Unexecute(); // Supports the bracket rollback requirement
}
