import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WinnerService {

  constructor() { }

  private d = new Subject<object>();
  d$: Observable<object> = this.d.asObservable();

  dataWinner: object;

  setData(data) {   
    this.dataWinner = data;
    this.d.next(data);
  }

}
