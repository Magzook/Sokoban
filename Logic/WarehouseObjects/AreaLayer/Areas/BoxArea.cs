namespace Sokoban.Logic.WarehouseObjects.AreaLayer.Areas;

public class BoxArea : IArea
{
     private static BoxArea instance = new ();

     private BoxArea()
     {
     }
     
     public static BoxArea Get() => instance;
}