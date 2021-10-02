import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Nominees } from './model/nominees-model';

@Injectable({
  providedIn: 'root'
})
export class DataService {

 
 nom = new Nominees();

  private dataSource = new BehaviorSubject<object>(this.nom);
  data = this.dataSource.asObservable();

  constructor() { }

  setData(data) {
    this.dataSource.next(data);
  }

}
