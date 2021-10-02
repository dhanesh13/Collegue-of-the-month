import { Winner } from 'app/model/winner-model';
import { Component, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpEvent, HttpHeaders, HttpRequest,HttpResponse } from '@angular/common/http';
import { Employee } from './model/employee-model';
import { Observable, throwError, of, Subscription } from 'rxjs';
import { catchError } from 'rxjs/operators';
import {Subject} from 'rxjs'; //for refreshing
import { tap, startWith, debounceTime, distinctUntilChanged, switchMap, map } from 'rxjs/operators';
import { VotingForm } from './model/voter-form-model';
import { environment } from '../environments/environment';
import { Nominees } from './model/nominees-model';
import { RoleService } from './guards/role.service';
import { VotingSession } from './model/voting-session-model';
import 'rxjs/add/operator/map';
import { EventVotingSession } from './model/event-voting-session';
import { EventModel } from './model/event';
import { VoteNow } from './model/votenow-model';
import { VoteLogNominations } from './model/votelog-nominations';
import { HallOfFameWinners } from './model/hall-of-fame-winners';
import { InspireTeam } from './model/inspire-team';


@Injectable({
  providedIn: 'root'
})

export class ApiService 
{
  managerID: number;
  period: string;
  activeSessionId:string;
  nomineeid:number;

  loginId: number;
  
  private SERVER_URL = environment.API_URL + 'api';

 
  
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8'
    })
  };

  constructor(private httpClient: HttpClient, private roleService: RoleService) 
  {

  }

  // get nomination period
  getPeriod()
  {
    return this.httpClient.get(this.SERVER_URL+'/cotm/getPeriod', {responseType: 'text'}); 
  }
    //get session id for active cotm voting session
    getActiveCOTMVotingSessionId()
    {
      return this.httpClient.get(this.SERVER_URL+'/voting/getSessionIDofActiveCOTMEvent', {responseType: 'text'}); 
    }
 

  getEmpDetails(): Observable<Employee[]>
  {
    return this.httpClient.get<Employee[]>(this.SERVER_URL+'/cotm/getEmployeesByFilter'); 
  }

  getTopNomineesDetailsByManager(): Observable<Nominees[]>
  {   
    this.managerID = Number(localStorage.getItem('managerId'));
    this.period = String(localStorage.getItem('period'));
    return this.httpClient.get<Nominees[]>(this.SERVER_URL+'/cotm/rejectednomineesbymanager/1/'+this.managerID+'/'+this.period); 
  }

  //second card for manager
  getTopFiveNomineesDetailsByManager(): Observable<Nominees[]>
  {   
    this.managerID = Number(localStorage.getItem('managerId'));
    this.period = String(localStorage.getItem('period'));
    return this.httpClient.get<Nominees[]>(this.SERVER_URL+'/cotm/topfiverejectednomineesbymanager/1/5/'+this.managerID+'/'+this.period); 
  }

  getTopNomineesDetails(): Observable<Nominees[]>
  {   
    this.period = String(localStorage.getItem('period'));
    return this.httpClient.get<Nominees[]>(this.SERVER_URL+'/cotm/getNominees/1/20'+'/'+this.period); 
  }

  getTopFiveNomineesDetails(): Observable<Nominees[]>
  {   
    this.period = String(localStorage.getItem('period'));
    return this.httpClient.get<Nominees[]>(this.SERVER_URL+'/cotm/getNominees/1/5'+'/'+this.period); 
  }
  
  getShortlistedNomineesDetailsByNomineePayrollID(nomineeID:string)
  { 
    
    this.nomineeid=Number(nomineeID)
    console.log(nomineeID);
    this.period = localStorage.getItem('period');  
    return this.httpClient.get(this.SERVER_URL+'/cotm/shortlistednomineeByNomineePayrollID/1'+'/'+this.nomineeid+'/'+this.period); 
  }
  // dashboard - to retrieve shortlisted nominees
  getTopShortlistedNomineesDetails(): Observable<Nominees[]>
  { 
    const periodTest = localStorage.getItem('period');  
    return this.httpClient.get<Nominees[]>(this.SERVER_URL+'/cotm/shortlistednominees/1'+'/'+periodTest); 
  }

  getTopShortlistedNomineesDetailsForVote(): Observable<Nominees[]>
  { this.period = String(localStorage.getItem('period'));  
    return this.httpClient.get<Nominees[]>(this.SERVER_URL+'/cotm/shortlistednominees/1'+'/'+this.period); 
  }
  getTopNonShortlistedNomineesDetails(): Observable<Nominees[]>
  {  this.period = String(localStorage.getItem('period')); 
    return this.httpClient.get<Nominees[]>(this.SERVER_URL+'/cotm/nonshortlistednominees/1/20'+'/'+this.period); 
  }

  getTopFiveNonShortlistedNominees(): Observable<Nominees[]>
  { 
    this.period = localStorage.getItem('period');  
    return this.httpClient.get<Nominees[]>(this.SERVER_URL+'/cotm/nonshortlistednominees/1/5'+'/'+this.period); 
  }

  saveDetails(voting) 
  {  
    return this.httpClient.post(this.SERVER_URL+'/cotm/create', voting, this.httpOptions );     
  }
  getManagerResult(): Observable<Winner[]>
  {
    this.period = String(localStorage.getItem('period'));
    return this.httpClient.get<Winner[]>(this.SERVER_URL+'/cotm/admingetvotednominees/1'+'/'+this.period); 
  }

  getEvents(): Observable<EventModel[]>
  {
    return this.httpClient.get<EventModel[]>(this.SERVER_URL+'/cotm/getEvents'); 
  }

 

  createVotingSession(votingSessionForm) {  
    return this.httpClient.post(this.SERVER_URL+'/voting/createVotingSession', votingSessionForm, this.httpOptions );     
  }

  getVotingSessions(): Observable<EventVotingSession[]>
  {
    return this.httpClient.get<EventVotingSession[]>(this.SERVER_URL+'/voting/getEventsByFilter'); 
  }

  closeVotingSession(sessionId:number): Observable<EventVotingSession[]> {  
    return this.httpClient.put<EventVotingSession[]>(this.SERVER_URL+'/voting/closeVotingSession/' + sessionId, this.httpOptions );     
  }

  editVotingSession(sessionId:number, editVotingSessionForm) {  
    return this.httpClient.put(this.SERVER_URL+'/voting/editVotingSession/' + sessionId, editVotingSessionForm, this.httpOptions );     
  }

  getStatusClosingDate()
  {
    return this.httpClient.put(this.SERVER_URL+'/voting/statusClosingDate', this.httpOptions); 
  }

  

  adminWinnerNominee(winner)
  {
    this.period = String(localStorage.getItem('period'));
    return this.httpClient.put(environment.API_URL+'adminWinnerNominee/1/'+ winner.query.PayrollID+'/'+this.period,winner);
  }
  adminDeleteWinner(winner)
  {
    this.period = String(localStorage.getItem('period'));
    return this.httpClient.put(environment.API_URL+'adminDeleteWinner/1/'+ winner.query.PayrollID+'/'+this.period,winner);
  }

  GetShortListedNomineesByManager(): Observable<Nominees[]>
  {   
    this.managerID = Number(localStorage.getItem('managerId'));
    this.period = String(localStorage.getItem('period'));
  
    return this.httpClient.get<Nominees[]>(this.SERVER_URL+'/cotm/shortlistednomineesbymanager/1/20/'+this.managerID+'/'+this.period); 
  }

  // first card for manager
  GetTopFiveShortListedNomineesByManager(): Observable<Nominees[]>
  {   
    this.managerID = Number(localStorage.getItem('managerId'));
    this.period = String(localStorage.getItem('period'));
    return this.httpClient.get<Nominees[]>(this.SERVER_URL+'/cotm/shortlistednomineesbymanager/1/5/'+this.managerID+'/'+this.period); 
  }

  adminBasketShortlistNomineesWithRationale(eventid:number,payrollid:number,rationale:string){
    this.period = String(localStorage.getItem('period'));
    return this.httpClient.patch(this.SERVER_URL+'/cotm/shortlistNomineeWithRationale/' +eventid+'/'+payrollid+'/'+rationale+'/'+this.period, this.httpOptions );
  }
  managerShortlistNomineesWithRationale(eventid:number,payrollid:number,rationale:string){
    this.period = String(localStorage.getItem('period'));
    return this.httpClient.patch(this.SERVER_URL+'/cotm/shortlistNomineeWithManagerRationale/' +eventid+'/'+payrollid+'/'+rationale+'/'+this.period, this.httpOptions );
  }
  managerRejectionRationale(eventid:number,payrollid:number,rationale:string){
    this.period = String(localStorage.getItem('period'));
    return this.httpClient.patch(this.SERVER_URL+'/cotm/managerRejectionRationale/' +eventid+'/'+payrollid+'/'+rationale+'/'+this.period, this.httpOptions );
  }
  managerShortlistNominees(eventid:number,payrollid:number){
    this.period = String(localStorage.getItem('period'));
    return this.httpClient.put(this.SERVER_URL+'/cotm/managerShortlistNominee/' +eventid+'/'+payrollid+'/'+this.period, this.httpOptions );
  }

  postVoteNow(votes: VoteNow[])
  {
    return this.httpClient.post<VoteNow[]>(this.SERVER_URL+'/voting/vote', votes, this.httpOptions);
  }
  getVoteLog(): Observable<VoteNow[]>
  {
    this.managerID = Number(localStorage.getItem('managerId'));
    this.period = String(localStorage.getItem('period'));
    return this.httpClient.get<VoteNow[]>(this.SERVER_URL+'/voting/getVotelog');
  }
  getVoteLogByManager(eventid:number): Observable<VoteLogNominations[]>
  {
    this.managerID = Number(localStorage.getItem('managerId'));
    this.period = String(localStorage.getItem('period'));
    this.activeSessionId = String(localStorage.getItem('activeSessionId'));
    return this.httpClient.get<VoteLogNominations[]>(this.SERVER_URL+'/voting/getVotelog'+'/'+this.activeSessionId+'/'+eventid+'/'+this.managerID+'/'+this.period);
  }

 shortlistednomineesbymanager():Observable<Nominees[]>
 { 
  this.period = String(localStorage.getItem('period'));  
   return this.httpClient.get<Nominees[]>(this.SERVER_URL+'/cotm/shortlistednomineesbymanager/1/5/'+this.roleService.mId+'/'+this.period); 
 }

  uploadFile(formData)
  {
    return this.httpClient.put(this.SERVER_URL+'/upload/updateEmployeeList', formData, {reportProgress: true, observe: 'events'});
  }

  getHallOfFameWinner(): Observable<HallOfFameWinners[]>
  {
    return this.httpClient.get<HallOfFameWinners[]>(this.SERVER_URL+'/cotm/getHallOfFameWinner/1/2021', this.httpOptions); 
  }

  // inspire team 
  saveDetailsInspire(inspire): Observable<InspireTeam> 
  {  
    return this.httpClient.post<InspireTeam>(this.SERVER_URL+'/inspireteam/createInspireTeam', inspire, this.httpOptions );   
  }

  getNomineesInspire(): Observable<InspireTeam[]> 
  {  
    this.period = String(localStorage.getItem('period')); 
    return this.httpClient.get<InspireTeam[]>(this.SERVER_URL+'/inspireteam/getIT/3/' + this.period, this.httpOptions );   
  }
 
  //send email
  sendEmailForVotingSessionToManagers(){
    return this.httpClient.get(this.SERVER_URL+'/Email/sendEmailManagersForVotingSession', this.httpOptions);
  }

  //email dispatched to all winners
  sendEmailForWinners(){
    return this.httpClient.get(this.SERVER_URL+'/Email/sendEmailForWinners', this.httpOptions);
  }

  //email check for cotm event
  checkEmailForCOTMEvent(){
    this.period = String(localStorage.getItem('period')); 
    return this.httpClient.get(this.SERVER_URL+'/COTM/checkEmailSentForCOTMEvent'+'/'+this.period, this.httpOptions);
  }

  //email dispatched to all loc_mu when COTM event open
  sendEmailForCOTMEventWhenOpen(){
    return this.httpClient.get(this.SERVER_URL+'/Email/sendEmailForCOTMEventStart', this.httpOptions);
  }
  //update mail table after email sent
  updateMailAfterEmailSent(){
    this.period = String(localStorage.getItem('period'));
    return this.httpClient.patch(this.SERVER_URL+'/Email/updateMailSentWhenCOTMOpen'+'/'+this.period, this.httpOptions);
  }

  //email check for shortlistPeriod
  checkEmailForShortlistPeriod(){
    this.period = String(localStorage.getItem('period')); 
    return this.httpClient.get(this.SERVER_URL+'/COTM/checkEmailSentForShortlistPeriod'+'/'+this.period, this.httpOptions);
  }

  //email dispatched to all managers when shortlist period on
  sendEmailForShortlistPeriodOpen(){
    return this.httpClient.get(this.SERVER_URL+'/Email/sendEmailForShortlistPeriodOpen', this.httpOptions);
  }

  //update mail table after email sent
  updateMailSentWhenShortlistPeriodOpen(){
    this.period = String(localStorage.getItem('period'));
    return this.httpClient.patch(this.SERVER_URL+'/Email/updateMailSentWhenShortlistPeriodOpen'+'/'+this.period, this.httpOptions);
  }

  // voucher 
  saveDetailsVoucher(voucher) 
  {  
    return this.httpClient.post(this.SERVER_URL+'/cotm/vouchercreate', voucher, this.httpOptions );   
  }

  // show voucher if winner
  getShowVoucher()
  {
    this.loginId = Number(localStorage.getItem('voterPayrollId')); 
    this.period = String(localStorage.getItem('period')); 
    return this.httpClient.get(this.SERVER_URL+'/cotm/showVoucher/'+ this.loginId + '/' + this.period + '/1', this.httpOptions); 
  }

}


