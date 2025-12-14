namespace Sokoban.Logic.WarehouseObjects.AreaLayer.Areas;

public class BoxArea : IArea
{
     private static BoxArea instance = new BoxArea();

     private BoxArea()
     {
     }
     
     public static BoxArea Get() => instance;
}