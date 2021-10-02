import { Subject, Observable, BehaviorSubject } from "rxjs";
import { Injectable } from "@angular/core";
import { EventVotingSession } from "app/model/event-voting-session";

@Injectable({
    providedIn: 'root'
})

export class VotingSessionService {

    // for voting session

    vote :EventVotingSession[]=[];
    
    private dataSource = new BehaviorSubject<object>(this.vote);
    data = this.dataSource.asObservable();
    
    constructor() { }
    
    setData(data) {
        this.dataSource.next(data);
    }
    
}