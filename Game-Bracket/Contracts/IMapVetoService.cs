using Game_Bracket.Domain;

namespace Game_Bracket.Contracts;

public interface IMapVetoService
{
    void RunMapVeto();
    Map GetSelectedMap();
    Map getSelectedMap() => GetSelectedMap();
}

public interface IMapVetoFactory
{
    IMapVetoService CreateService(string format);
}