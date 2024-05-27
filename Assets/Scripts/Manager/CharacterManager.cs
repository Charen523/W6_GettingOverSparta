using System.Collections;
using System.Collections.Generic;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CharacterManager : Singleton<CharacterManager> //ΩÃ±€≈Ê»≠.
{
    private Player _player;

    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }
}
