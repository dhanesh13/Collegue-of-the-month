import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { InspireTeam } from './model/inspire-team';

@Injectable({
  providedIn: 'root'
})

export class RationaleService {

  nom = new InspireTeam();

  private dataSource = new BehaviorSubject<object>(this.nom);
  data = this.dataSource.asObservable();

  constructor() { }

  setData(data) {
    this.dataSource.next(data);
  }
}
