import { Subject, Observable } from "rxjs";
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class DateService {

    // for opening and closing date
    private date = new Subject<boolean>();
    date$: Observable<boolean> = this.date.asObservable();

    dateToVote: boolean;

    setDate(data) {  
        this.dateToVote = data;
        this.date.next(data);
    }
}