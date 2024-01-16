import { User } from "./User";

export class UserParams{
    gender:string;
    minAge=20;
    maxAge=100;
    pageNumber=1;
    PageSize=3;
    orderBy = 'lastActive';
    constructor(user:User){
        this.gender=user.gender==='female'?'male':'female'
     }

}