namespace Player
{
    
    public interface IPlayer
    {
        void TakeDamage(int x, int y);
        
        void Dead(int x, int y);
        
        void PickUpTrash();
        
        void HelpOldMan();
        
        void CallPolice();
    }
    
}
