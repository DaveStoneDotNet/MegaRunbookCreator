export class Hero {

  constructor(
    public Id: number,
    public Name: string,
    public Power: string,
    public AlterEgo?: string, 
    public RayType?: string, 
    public Notes?: string, 
    public IsFavorite?: boolean
  ) {  }

}