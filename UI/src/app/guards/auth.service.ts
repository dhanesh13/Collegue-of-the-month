import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';
import { Login } from 'app/model/login';
import { ChangePassword } from 'app/model/change-password';

@Injectable({
  providedIn: 'root'
})

export class AuthService  
{
  private SERVER_URL = environment.API_URL + 'api';

  //isLoggedIn = false;
  
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8'
    })
  };

  constructor(private http: HttpClient) 
  { 

  }

  userLogin(login): Observable<Login> {  
    //this.isLoggedIn = true; 
    return this.http.post<Login>(this.SERVER_URL+'/auth/login', login, this.httpOptions);  
       
  }

  // opening and closing date
  getAccess()
  {
    return this.http.get(this.SERVER_URL+'/cotm/getDateAccess'); 
  }

  //opening and closing date for Manager ShorlistPeriod
  getAccessShortlistPeriod()
  {
    return this.http.get(this.SERVER_URL+'/cotm/getDateAccessShortlist'); 
  }
  
  changePassword(changePassword){  
    return this.http.put(this.SERVER_URL+'/auth/changePassword', changePassword, this.httpOptions);       
  }

  // get voting session
  getVotingSession()
  {
    return this.http.get(this.SERVER_URL+'/voting/getActiveCOTMEvent'); 
  }

}


