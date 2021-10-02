import { BehaviorSubject, Subject, Observable } from "rxjs";
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export class RoleService {

    private empRole = new Subject<number>();
    empRole$: Observable<number> = this.empRole.asObservable();

    role: number;

    // get manager id
    private id = new Subject<string>();
    mId$: Observable<string> = this.id.asObservable();

    mId: string;

    // get employee id
    private eid = new Subject<number>();
    eId$: Observable<number> = this.eid.asObservable();

    eId: number;

    setEmpRole(data) {   
        this.role = data;
        this.empRole.next(data);
    }

    setManagerId(data) {  
        this.mId = data;
        this.id.next(data);
    }

    setEmployeeId(data) {          
        this.eId = data;
        this.eid.next(data);
    }

    // for opening and closing date
    private date = new Subject<boolean>();
    date$: Observable<boolean> = this.date.asObservable();

    dateToVote: boolean;

    setDate(data) {  
        this.dateToVote = data;
        this.date.next(data);
    }
}