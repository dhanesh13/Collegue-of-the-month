import { Nominees } from "./nominees-model";

export class VoteNow{

  nomineepayrollid:number;
  voted:boolean;
  period:string;
  eventid:number;
  managerpayrollid:any;
  name: string;
 nominations:Nominees[];
 sessionid: number;
}