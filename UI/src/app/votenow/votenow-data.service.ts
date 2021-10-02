import { Injectable } from '@angular/core';
import { VoteNow } from 'app/model/votenow-model';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VotenowDataService {

  votenowlist = new VoteNow();

  private dataSource = new BehaviorSubject<object>(this.votenowlist);
  data = this.dataSource.asObservable();

  constructor() { }

  setData(data) {
    console.log(data);
    this.dataSource.next(data);
  }
}
